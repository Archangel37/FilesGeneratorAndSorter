using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileGenerator.Models;
using Newtonsoft.Json;

namespace FileGenerator.Implementations
{
    internal static class FileWorker
    {
        private static readonly MainConfig Cfg = JsonConvert.DeserializeObject<MainConfig>(File.ReadAllText("config.json"));
        private static long _resultFileSize = Cfg.FileSizeBytes;
        private static readonly int WriteFileBuffer = Cfg.WriteFileBufferBytes;

        internal static Task WriteResultFile(Dictionary<int, char[]> dictionary, string fileName)
        {
            var newLineSize = Encoding.ASCII.GetByteCount(Environment.NewLine);
            var count = dictionary.Count;

            return Task.Run(() =>
            {
                var rnd = new Random();
                using var sw = new StreamWriter(fileName, false, Encoding.ASCII, WriteFileBuffer);
                //allocates because of separatedLine.ToString()
                foreach (var line in FetchData(rnd, dictionary, count, newLineSize))
                    sw.WriteLine(line);
            });
        }


        private static IEnumerable<string> FetchData(Random rnd, IReadOnlyDictionary<int, char[]> dictionary,
            int count, int newLineSize)
        {
            //var sb = new StringBuilder();
            //slower for 1Gb file: ~14.6s for StringBuilder, string line by line - ~14.06s
            //var newLine = $"{rnd.Next(0, int.MaxValue)}. {new string(dictionary[rnd.Next(0, count)])}";
            //allocates ~3Gb (but less, than SB - ~4Gb)
            //even with buffer
            //Encoding.ASCII.GetByteCount(newLine) - slower than newLine.Length
            //We'll assume that in dictionaries we will be within ASCII symbols (in ASCII 1 char == 1 byte)
            while (_resultFileSize > 0)
            {
                //for 1Gb file: 9.422s with that, allocates ~3Gb, but faster because of operations on stack
                var separatedLine = new SeparatedLine
                    {Number = rnd.Next(0, int.MaxValue), Text = dictionary[rnd.Next(0, count)]};
                _resultFileSize -= newLineSize + separatedLine.GetLength();
                yield return separatedLine.ToString();
            }
        }
    }
}