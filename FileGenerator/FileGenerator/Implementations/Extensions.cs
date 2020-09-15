using System.IO;
using System.Linq;
using FileGenerator.Models;

namespace FileGenerator.Implementations
{
    public static class Extensions
    {
        public static void ValidateConfig(this MainConfig cfg)
        {
            if (cfg.FileSizeBytes <= 0) cfg.FileSizeBytes = 1048576; //1Mb
            var validatedFiles = cfg.DictionaryFilesPaths.ToList();
            foreach (var file in cfg.DictionaryFilesPaths)
                if (!File.Exists(file))
                    validatedFiles.Remove(file);

            cfg.DictionaryFilesPaths = validatedFiles;
            //todo validate that Kafka is reachable

            //https://stackoverflow.com/questions/3137097/check-if-a-string-is-a-valid-windows-directory-folder-path
            if (string.IsNullOrWhiteSpace(cfg.ResultFilePath))
            {
                cfg.ResultFilePath = "0.txt";
                return;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(cfg.ResultFilePath))
                    Path.GetFullPath(cfg.ResultFilePath);
            }
            catch
            {
                cfg.ResultFilePath = Path.GetFullPath("0.txt");
            }
        }

        //https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number/51099524
        //We'll assume that int is enough and n>0, and we'll believe that benchmark (but should be tested in future)
        public static int Digits_IfChain(this int n)
        {
            if (n < 10) return 1;
            if (n < 100) return 2;
            if (n < 1000) return 3;
            if (n < 10000) return 4;
            if (n < 100000) return 5;
            if (n < 1000000) return 6;
            if (n < 10000000) return 7;
            if (n < 100000000) return 8;
            if (n < 1000000000) return 9;
            return 10;
        }
    }
}