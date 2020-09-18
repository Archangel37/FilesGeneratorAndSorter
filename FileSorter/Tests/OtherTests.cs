using System.Diagnostics.CodeAnalysis;
using FileSorter.Implementations;
using FileSorter.Models;
using Xunit;

namespace Tests
{
    [ExcludeFromCodeCoverage]
    public class OtherTests
    {
        private static readonly string[] ArrayForCompare =
        {
            "A",
            "A ",
            "AB",
            "Z",
            "ZZ",
            "a",
            "a ",
            "ab"
        };
        
        [Fact]
        public void Test_Digits_IfChain()
        {
            //int.MaxValue = 2147483647 - 10 digits
            for (var i = 1; i < 11; i++)
            {
                var num = int.Parse(new string('1', i));
                Assert.True(num.Digits_IfChain() == i);
            }
        }
        
        [Fact]
        public void Test_ValidateConfig()
        {
            var cfg = new MainConfig();
            cfg.ValidateConfig();

            Assert.True(cfg.SubstringLength == 2);
            Assert.True(cfg.WriteMemoryBufferBytes == 524260);
            Assert.True(cfg.OutputFileName == "result.txt");
            Assert.True(cfg.TemporaryFilesExtension == "");
        }
        
        [Fact]
        public void Test_CompareExtension()
        {
            for (var i = 0; i < ArrayForCompare.Length - 1; i++)
                Assert.True(ArrayForCompare[i].Compare(ArrayForCompare[i + 1]) == -1);
            
            for (var j = ArrayForCompare.Length - 1; j > 1; j--)
                Assert.True(ArrayForCompare[j].Compare(ArrayForCompare[j - 1]) == 1);
            
            Assert.True("ABC".Compare("ABC") == 0);
        }
    }
}