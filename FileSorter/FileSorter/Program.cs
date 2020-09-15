using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileSorter.Implementations;
using FileSorter.Models;
using Newtonsoft.Json;
using Serilog;

namespace FileSorter
{
    internal static class Program
    {
        private static async Task Main()
        {
            //todo temp - for manual tests 
            // var message = new KafkaMessage
            // {
            //     UtcStartDateTime = DateTime.Now.AddMinutes(-1).ToUniversalTime(),
            //     FilePath = @"<some path>"
            // };

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

            await KafkaConsumer.ConsumeMessage(cfg, new CancellationToken(), Log.Logger);
            //// <comment upper line and uncomment this to use without Kafka in testing>
            //await SortWorker.ProcessSorting(cfg, message, Log.Logger);
            //Console.ReadKey();
        }
    }
}