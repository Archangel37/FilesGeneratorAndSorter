using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileGenerator.Implementations;
using FileGenerator.Models;
using Newtonsoft.Json;
using Serilog;


namespace FileGenerator
{
    internal static class Program
    {
        private static async Task Main()
        {
            var dtStartMain = DateTime.Now.ToUniversalTime();

            Log.Logger =
                new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.With(new UtcTimestampEnricher())
                    .WriteTo.ColoredConsole(
                        outputTemplate:
                        "{UtcTimestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {CallerMemberName} {Message:lj}{NewLine}{Exception}",
                        formatProvider: CultureInfo.InvariantCulture)
                    .CreateLogger();


            var cfg = JsonConvert.DeserializeObject<MainConfig>(await File.ReadAllTextAsync("config.json"));
            cfg.ValidateConfig();
            
            if (cfg.Kafka.Enabled)
                await KafkaProducer.CreateTopic(cfg, Log.Logger);

            var dtConfigs = DateTime.Now.ToUniversalTime();

            Log.Information(
                $"Preparing Logger and validating configs done in: {dtConfigs - dtStartMain}");

            var filesSummaryToDict = new List<string>();

            foreach (var file in cfg.DictionaryFilesPaths)
                filesSummaryToDict.AddRange(await File.ReadAllLinesAsync(Path.GetFullPath(file),
                    Encoding.ASCII));

            var dictionary = filesSummaryToDict
                .Select((v, i) => new {Key = i, Value = v})
                .ToDictionary(o => o.Key, o => o.Value.ToArray());

            var dtStart = DateTime.Now.ToUniversalTime();
            Log.Information(
                $"Preparing Dictionary done in: {dtStart - dtConfigs}");

            //todo allocates
            await FileWorker.WriteResultFile(dictionary, cfg.ResultFilePath);

            var dtEnd = DateTime.Now.ToUniversalTime();

            Log.Information($"Preparing resulting file done in: {dtEnd - dtStart}");

            var fi = new FileInfo(cfg.ResultFilePath);
            Log.Information($"Config File Size (Bytes):\t{cfg.FileSizeBytes}");
            Log.Information($"Result File Size (Bytes):\t{fi.Length}");

            if (cfg.Kafka.Enabled)
                await KafkaProducer.ProduceMessage(cfg,
                    new KafkaMessage {UtcStartDateTime = dtStartMain, FilePath = fi.FullName}, Log.Logger);
            
            Log.Information($"Result File:\t{fi.FullName}");
            Log.Information("Done");
            //Console.ReadKey();
        }
    }
}