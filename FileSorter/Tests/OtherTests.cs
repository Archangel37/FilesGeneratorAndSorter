using System;
using System.Diagnostics.CodeAnalysis;
using FileSorter.Implementations;
using FileSorter.Models;
using Xunit;

namespace Tests
{
    [ExcludeFromCodeCoverage]
    public class OtherTests
    {
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
    }
}