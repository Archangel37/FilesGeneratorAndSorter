using System;
using FileSorter.Implementations;

namespace FileSorter.Models
{
    public struct SeparatedLine : IComparable
    {
        public int Number { get; set; }
        public string Text { get; set; }

        // faster than $"{Number}. {Text}" and faster than using SB etc
        public override string ToString()
        {
            var numCopy = Number;
            var countNums = Number.Digits_IfChain();
            var countText = Text.Length;
            //buffer on stack
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

        public int CompareTo(object obj)
        {
            if (!(obj is SeparatedLine other)) throw new ArgumentException("Wrong comparison object type");
            if (this == other) return 0;
            if (this > other) return 1;
            return -1;
        }

        public static bool operator >(SeparatedLine a, SeparatedLine x)
        {
            if (a == x) return false;
            var intComparison = string.Compare(a.Text, x.Text, StringComparison.InvariantCulture);
            if (intComparison > 0) return true;
            if (intComparison < 0) return false;
            return a.Number > x.Number;
        }

        public static bool operator <(SeparatedLine a, SeparatedLine x)
        {
            if (a > x) return false;
            return a != x;
        }

        public static bool operator >=(SeparatedLine a, SeparatedLine x)
        {
            if (a > x) return true;
            return a == x;
        }

        public static bool operator <=(SeparatedLine a, SeparatedLine x) => !(a > x);

        public static bool operator ==(SeparatedLine a, SeparatedLine x) => a.Text == x.Text && a.Number == x.Number;

        public static bool operator !=(SeparatedLine a, SeparatedLine x) => !(a == x);

        public bool Equals(SeparatedLine other) => Number == other.Number && Text == other.Text;

        public override bool Equals(object obj) => obj is SeparatedLine other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Number, Text);
    }
}