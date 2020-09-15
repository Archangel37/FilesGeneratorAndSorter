using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSorter.Models;
using Serilog;

namespace FileSorter.Implementations
{
    public static class SortWorker
    {
        private static Task FileSplitter(IDictionary<string, StringBuilder> dictionary, MainConfig cfg, string path) =>
            Task.Run(() =>
            {
                var processedLines = File.ReadLines(path).AsParallel()
                    .Select(l => l);
                foreach (var processedLine in processedLines)
                {
                    if (string.IsNullOrWhiteSpace(processedLine)) continue;

                    var tmp = processedLine.GetSeparatedLine();

                    string dictKeyToMatch;
                    if (tmp.Text.Length > cfg.SubstringLength - 1)
                        dictKeyToMatch = tmp.Text.Substring(0, cfg.SubstringLength);
                    else
                        dictKeyToMatch = tmp.Text;

                    if (!dictionary.ContainsKey(dictKeyToMatch))
                        dictionary.Add(dictKeyToMatch, new StringBuilder());

                    var sb = dictionary[dictKeyToMatch];
                    sb.AppendLine(processedLine);

                    if (sb.Length > cfg.WriteMemoryBufferBytes)
                    {
                        using var sw =
                            new StreamWriter(
                                Path.Combine(cfg.OutputFolderPath, dictKeyToMatch + cfg.TemporaryFilesExtension), true,
                                Encoding.ASCII);
                        // Write the buffer and clear sb
                        sw.WriteLine(sb.ToString());
                        sb.Clear();
                    }
                }

                // Write all the data remaining in memory
                Parallel.ForEach(dictionary, info =>
                {
                    if (info.Value.Length <= 0) return;
                    using var sw =
                        new StreamWriter(Path.Combine(cfg.OutputFolderPath, info.Key + cfg.TemporaryFilesExtension),
                            true, Encoding.ASCII);
                    // Write the buffer and clear
                    sw.WriteLine(info.Value.ToString());
                    info.Value.Clear();
                });
            });

        private static Task FilesSortAndAggregate(IEnumerable<string> keys, MainConfig cfg) =>
            Task.Run(async () =>
            {
                await using var totalWriter =
                    new StreamWriter(Path.Combine(cfg.OutputFolderPath, cfg.OutputFileName), true);
                foreach (var key in keys)
                {
                    var parallelQueryLines = File
                        .ReadLines(Path.Combine(cfg.OutputFolderPath, key + cfg.TemporaryFilesExtension)).AsParallel()
                        .Select(l => l);

                    var forSorting =
                        Enumerable.ToArray(
                            from processedLine
                                in parallelQueryLines
                            where !string.IsNullOrWhiteSpace(processedLine)
                            select processedLine.GetSeparatedLine());
                    // Sort and write files line-by-line from aa.sort to zz.sort
                    foreach (var spLine in Sorting.QuickSort(forSorting)) totalWriter.WriteLine(spLine);

                    File.Delete(Path.Combine(cfg.OutputFolderPath, key + cfg.TemporaryFilesExtension));
                }
            });
        
        
        public static async Task ProcessSorting(MainConfig cfg, KafkaMessage message, ILogger logger)
        {
            var dtStart = DateTime.Now.ToUniversalTime();

            var data = new Dictionary<string, StringBuilder>();
            await FileSplitter(data, cfg, message.FilePath);

            var dtEndSplitFiles = DateTime.Now.ToUniversalTime();
            logger.Information($"Split files ready in: {dtEndSplitFiles - dtStart}");
            
            // Get ordered keys from dict
            // todo Warning!! Trim end not letters and digits
            var keys = data.OrderBy(x => x.Key)
                .Select(y => y.Key.TrimEnd(' '));

            await FilesSortAndAggregate(keys, cfg);

            var dtEndSorting = DateTime.Now.ToUniversalTime();
            logger.Information($"Sorting and writing: {dtEndSorting - dtEndSplitFiles}");

            Log.Information("Done..");

            var source = new FileInfo(message.FilePath);
            var result = new FileInfo(Path.Combine(cfg.OutputFolderPath, cfg.OutputFileName));

            logger.Information($"Total Time For Sorting: {dtEndSorting - dtStart}");
            logger.Information($"Files Length:");
            logger.Information($"\tSource file (bytes): {source.Length}");
            logger.Information($"\tResult file (bytes): {result.Length}");
            logger.Information($"TOTAL TIME (GENERATE+KAFKA+SORT):\t{dtEndSorting-message.UtcStartDateTime}");
            logger.Information($"Output file:\t{result.FullName}");
        }
    }
}