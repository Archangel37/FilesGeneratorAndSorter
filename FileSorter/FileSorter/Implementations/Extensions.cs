using System;
using System.IO;
using FileSorter.Models;

namespace FileSorter.Implementations
{
    public static class Extensions
    {
        public static void ValidateConfig(this MainConfig cfg)
        {
            if (cfg.SubstringLength < 2) cfg.SubstringLength = 2;
            if (cfg.WriteMemoryBufferBytes < 128) cfg.WriteMemoryBufferBytes = 524260; //less than 512Kb

            cfg.TemporaryFilesExtension ??= "";

            try
            {
                if (!string.IsNullOrWhiteSpace(cfg.TemporaryFilesExtension))
                    Path.GetFullPath(cfg.TemporaryFilesExtension);
            }
            catch
            {
                cfg.TemporaryFilesExtension = ".sort";
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(cfg.OutputFolderPath))
                    Path.GetFullPath(cfg.OutputFolderPath);
            }
            catch
            {
                cfg.OutputFolderPath = "";
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(cfg.OutputFileName))
                    Path.GetFullPath(cfg.OutputFileName);
                else cfg.OutputFileName = "result.txt";
            }
            catch
            {
                cfg.OutputFileName = "result.txt";
            }
        }

        //https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number/51099524
        //We'll assume that int is enough
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

        // Warning!!! Only for strings like $"{num}. {some string}"
        // Faster than sourceLine.Split(". ", StringSplitOptions.RemoveEmptyEntries) etc
        // do not use string.Create for spans (2-times slower) [link below]
        // https://www.stevejgordon.co.uk/creating-strings-with-no-allocation-overhead-using-string-create-csharp
        public static SeparatedLine GetSeparatedLine(this string sourceLine)
        {
            var strLength = sourceLine.Length;
            //Span<char> stack = sourceLine.ToCharArray(); - a little slower and allocates
            // ReSharper disable once SuggestVarOrType_Elsewhere
            Span<char> stack = stackalloc char[strLength];
            for (var i = 0; i < strLength; i++) stack[i] = sourceLine[i];

            var numberEndIndex = 0;
            var resultNumber = 0;
            for (var i = 0; i < strLength; i++)
                if (char.IsDigit(stack[i]))
                {
                    resultNumber *= 10;
                    resultNumber += stack[i] - '0';
                }
                else
                {
                    numberEndIndex = i;
                    return new SeparatedLine
                        {Number = resultNumber, Text = new string(stack.Slice(numberEndIndex + 2))};
                }

            //2 for '.' and space
            numberEndIndex += 2;
            return new SeparatedLine {Number = resultNumber, Text = new string(stack.Slice(numberEndIndex))};
        }

        public static int Compare(this string str, string other)
        {
            var strLen = str.Length;
            var otherLen = other.Length;

            for (var k = 0; k < Math.Min(strLen, otherLen); k++)
                if (str[k] != other[k])
                    return str[k] > other[k] ? 1 : -1;

            // todo test ternary operator
            if (strLen == otherLen)
                return 0;
            if (strLen > otherLen)
                return 1;
            return -1;
        }
    }
}