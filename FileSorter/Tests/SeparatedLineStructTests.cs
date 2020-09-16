using System;
using System.Diagnostics.CodeAnalysis;
using FileSorter.Models;
using Xunit;

namespace Tests
{
    [ExcludeFromCodeCoverage]
    public class SeparatedLineStructTests
    {
        private SeparatedLine _sourceValue = new SeparatedLine {Number = 5, Text = "AAA"};
        private readonly SeparatedLine _equalSource = new SeparatedLine {Number = 5, Text = "AAA"};
        private readonly SeparatedLine _greaterText = new SeparatedLine {Number = 5, Text = "ABB"};
        private readonly SeparatedLine _lowerText = new SeparatedLine {Number = 5, Text = "A"};
        private readonly SeparatedLine _greaterNumber = new SeparatedLine {Number = 128, Text = "AAA"};
        private readonly SeparatedLine _lowerNumber = new SeparatedLine {Number = 1, Text = "AAA"};
        private readonly SeparatedLine _lowerAll = new SeparatedLine {Number = 1, Text = "A"};
        private readonly SeparatedLine _greaterAll = new SeparatedLine {Number = 1024, Text = "ZZZZZ"};

        [Fact]
        public void Test_CompareTo()
        {
            Assert.True(_sourceValue.CompareTo(_equalSource) == 0);
            Assert.True(_sourceValue.CompareTo(_greaterText) == -1);
            Assert.True(_sourceValue.CompareTo(_lowerText) == 1);
            Assert.True(_sourceValue.CompareTo(_greaterNumber) == -1);
            Assert.True(_sourceValue.CompareTo(_lowerNumber) == 1);
            Assert.True(_sourceValue.CompareTo(_lowerAll) == 1);
            Assert.True(_sourceValue.CompareTo(_greaterAll) == -1);

            var z = 5;
            try
            {
                var res = _sourceValue.CompareTo(z);
            }
            catch (Exception ex)
            {
                Assert.True(ex != null);
            }
        }

        [Fact]
        public void Test_GreaterOrEqual()
        {
            Assert.True(_sourceValue >= _equalSource);

            Assert.True(_sourceValue >= _lowerText);
            Assert.True(_sourceValue >= _lowerNumber);
            Assert.True(_sourceValue >= _lowerAll);

            Assert.False(_sourceValue >= _greaterText);
            Assert.False(_sourceValue >= _greaterNumber);
            Assert.False(_sourceValue >= _greaterAll);
        }

        [Fact]
        public void Test_LessOrEqual()
        {
            Assert.True(_sourceValue <= _equalSource);

            Assert.False(_sourceValue <= _lowerText);
            Assert.False(_sourceValue <= _lowerNumber);
            Assert.False(_sourceValue <= _lowerAll);

            Assert.True(_sourceValue <= _greaterText);
            Assert.True(_sourceValue <= _greaterNumber);
            Assert.True(_sourceValue <= _greaterAll);
        }

        [Fact]
        public void Test_Equals_SeparatedLine()
        {
            Assert.True(_sourceValue.Equals(_equalSource));

            Assert.False(_sourceValue.Equals(_lowerText));
            Assert.False(_sourceValue.Equals(_lowerNumber));
            Assert.False(_sourceValue.Equals(_lowerAll));
            Assert.False(_sourceValue.Equals(_greaterText));
            Assert.False(_sourceValue.Equals(_greaterNumber));
            Assert.False(_sourceValue.Equals(_greaterAll));
        }
        
        [Fact]
        public void Test_Equals_Object()
        {
            Assert.True(_sourceValue.Equals((object)_equalSource));

            Assert.False(_sourceValue.Equals((object)_lowerText));
            Assert.False(_sourceValue.Equals((object)_lowerNumber));
            Assert.False(_sourceValue.Equals((object)_lowerAll));
            Assert.False(_sourceValue.Equals((object)_greaterText));
            Assert.False(_sourceValue.Equals((object)_greaterNumber));
            Assert.False(_sourceValue.Equals((object)_greaterAll));
        }
    }
}