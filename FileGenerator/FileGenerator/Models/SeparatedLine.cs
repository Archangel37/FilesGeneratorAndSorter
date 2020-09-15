using System;
using FileGenerator.Implementations;

namespace FileGenerator.Models
{
    public struct SeparatedLine
    {
        public int Number { get; set; }
        public char[] Text { get; set; }

        //2 - for '.' and space
        public int GetLength() => Number.Digits_IfChain() + Text.Length + 2;

        // faster than $"{Number}. {Text}" and faster than using SB etc
        public override string ToString()
        {
            var numCopy = Number;
            var countNums = Number.Digits_IfChain();
            var countText = Text.Length;
            //buffer on stack
            // ReSharper disable once SuggestVarOrType_Elsewhere
            Span<char> buffer = stackalloc char[countNums + countText + 2];

            //from Number to char array
            for (var i = 0; i < countNums; i++)
            {
                buffer[countNums - 1 - i] = (char) (numCopy % 10 + '0');
                numCopy /= 10;
            }

            //now new position is countNums
            buffer[countNums++] = '.';
            buffer[countNums++] = ' ';

            for (var i = 0; i < countText; i++) buffer[countNums + i] = Text[i];

            return buffer.ToString();
        }
    }
}