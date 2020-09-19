using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FileSorter.Implementations;
using FileSorter.Models;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [ExcludeFromCodeCoverage]
    public class SortingTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private static readonly SeparatedLine[] ArrayWithSameNumber = {
            new SeparatedLine {Number = 1, Text = "ocean"},
            new SeparatedLine {Number = 1, Text = "tray"},
            new SeparatedLine {Number = 1, Text = "important"},
            new SeparatedLine {Number = 1, Text = "carpenter"},
            new SeparatedLine {Number = 1, Text = "relax"},
            new SeparatedLine {Number = 1, Text = "kind"},
            new SeparatedLine {Number = 1, Text = "mask"},
            new SeparatedLine {Number = 1, Text = "crayon"},
            new SeparatedLine {Number = 1, Text = "stale"},
            new SeparatedLine {Number = 1, Text = "striped"},
            new SeparatedLine {Number = 1, Text = "soak"},
            new SeparatedLine {Number = 1, Text = "verdant"},
        };

        private static readonly string[] ArrayWithSameNumberResult =
        {
            "1. carpenter",
            "1. crayon",
            "1. important",
            "1. kind",
            "1. mask",
            "1. ocean",
            "1. relax",
            "1. soak",
            "1. stale",
            "1. striped",
            "1. tray",
            "1. verdant"
        };
        
        private static readonly SeparatedLine[] ArrayWithDifferentNums = {
            new SeparatedLine {Number = 128, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 5, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 13, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 17, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 182179, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 122, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 133, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 144, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 155, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 132424, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 10, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 2, Text = "Array With Different Nums"},
            new SeparatedLine {Number = 4, Text = "Array With Different Nums"},
        };
        
        private static readonly string[] ArrayWithDifferentNumsResult =
        {
            "2. Array With Different Nums",
            "4. Array With Different Nums",
            "5. Array With Different Nums",
            "10. Array With Different Nums",
            "13. Array With Different Nums",
            "17. Array With Different Nums",
            "122. Array With Different Nums",
            "128. Array With Different Nums",
            "133. Array With Different Nums",
            "144. Array With Different Nums",
            "155. Array With Different Nums",
            "132424. Array With Different Nums",
            "182179. Array With Different Nums"
        };


        public SortingTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test_ShouldSortProperlyWithSameNumber()
        {
            var result = Sorting.QuickSort(ArrayWithSameNumber);
            for(var i=0; i < result.Length; i++)
            {
                Assert.True(result[i].ToString() == ArrayWithSameNumberResult[i]);
                _testOutputHelper.WriteLine($"SeparatedLine: <{result[i]}> Equals <{ArrayWithSameNumberResult[i]}> == {result[i].ToString() == ArrayWithSameNumberResult[i]}");
            }
        }
        
        [Fact]
        public void Test_GetHashCode_Uniqueness()
        {
            var concat = ArrayWithSameNumber.Concat(ArrayWithDifferentNums);
            var allHashCodes = concat.Select(x => x.GetHashCode());

            // not so critical multiple enumeration here
            // ReSharper disable once PossibleMultipleEnumeration
            var uniqueHashCodes = allHashCodes.Distinct();
            
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.True(allHashCodes.Count() == uniqueHashCodes.Count());
        }

        [Fact]
        public void Test_InvalidStringToSeparated()
        {
            var spLine = ". ABC".GetSeparatedLine();
            Assert.True(spLine.Equals(new SeparatedLine {Number = 0, Text = "ABC"}));
            try
            {
                var res = "".GetSeparatedLine();
            }
            catch (Exception e)
            {
                Assert.True(e != null);
            }
        }
        
        
        [Fact]
        public void Test_ShouldSortProperlyWithSameText()
        {
            var result = Sorting.QuickSort(ArrayWithDifferentNums);
            for(var i=0; i < result.Length; i++)
            {
                Assert.True(result[i].ToString() == ArrayWithDifferentNumsResult[i]);
                _testOutputHelper.WriteLine($"SeparatedLine: <{result[i]}> Equals <{ArrayWithDifferentNumsResult[i]}> == {result[i].ToString() == ArrayWithDifferentNumsResult[i]}");
            }
        }
        
        public static IEnumerable<object[]> SameNumberConvertData()
        {
            return Sorting.QuickSort(ArrayWithSameNumber).Select((t, i) => new object[] {t, ArrayWithSameNumberResult[i]});
        }
        
        public static IEnumerable<object[]> DifferentNumberConvertData()
        {
            return Sorting.QuickSort(ArrayWithDifferentNums).Select((t, i) => new object[] {t, ArrayWithDifferentNumsResult[i]});
        }

        [Theory]
        [MemberData(nameof(SameNumberConvertData))]
        [MemberData(nameof(DifferentNumberConvertData))]
        public void Test_ConvertingToString(SeparatedLine input, string result)
        {
            //checks ToString
            Assert.True(input.ToString() == result);
            //checks explicit converter
            Assert.True((string)input == result);
        }
        
        public static IEnumerable<object[]> SameNumberStringsData()
        {
            return ArrayWithSameNumberResult.Select(stringResult => new object[] {stringResult});
        }
        
        public static IEnumerable<object[]> DifferentNumberStringsData()
        {
            return ArrayWithDifferentNumsResult.Select(stringResult => new object[] {stringResult});
        }
        
        [Theory]
        [MemberData(nameof(SameNumberStringsData))]
        [MemberData(nameof(DifferentNumberStringsData))]
        public void Test_FromStringToSeparatedLine(string result)
        {
            var split = result.Split(". ");
            var spLine = new SeparatedLine {Number = int.Parse(split[0]), Text = split[1]};
            Assert.True(result.GetSeparatedLine() == spLine);
        }

        #region OtherSorts

        private static readonly Func<SeparatedLine[], SeparatedLine[]> BubbleSort = Sorting.BubbleSort;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> InsertionSort = Sorting.InsertionSort;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> OrderBy = Sorting.OrderBy;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> QuickSort = Sorting.QuickSort;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> ShakerSort = Sorting.ShakerSort;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> ShellSort = Sorting.ShellSort;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> Sort = Sorting.Sort;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> HybridQs = Sorting.HybridOptimizedQuickSort;
        private static readonly Func<SeparatedLine[], SeparatedLine[]> HeapSort = Sorting.HeapSorting;
        
        public static IEnumerable<object[]> Functions()
        {
            yield return new object[] { BubbleSort };
            yield return new object[] { InsertionSort };
            yield return new object[] { OrderBy };
            yield return new object[] { QuickSort };
            yield return new object[] { ShakerSort };
            yield return new object[] { ShellSort };
            yield return new object[] { Sort };
            yield return new object[] { HybridQs };
            yield return new object[] { HeapSort };
        }
        
        
        [Theory]
        [MemberData(nameof(Functions))]
        public void Test_OthersShouldSortProperlyWithSameNumber(Func<SeparatedLine[], SeparatedLine[]> func)
        {
            var result = func(ArrayWithSameNumber);
            for(var i=0; i < result.Length; i++)
            {
                Assert.True(result[i].ToString() == ArrayWithSameNumberResult[i]);
                _testOutputHelper.WriteLine($"SeparatedLine: <{result[i]}> Equals <{ArrayWithSameNumberResult[i]}> == {result[i].ToString() == ArrayWithSameNumberResult[i]}");
            }
        }
        
        [Theory]
        [MemberData(nameof(Functions))]
        public void Test_OthersShouldSortProperlyWithSameText(Func<SeparatedLine[], SeparatedLine[]> func)
        {
            var result = func(ArrayWithDifferentNums);
            for(var i=0; i < result.Length; i++)
            {
                Assert.True(result[i].ToString() == ArrayWithDifferentNumsResult[i]);
                _testOutputHelper.WriteLine($"SeparatedLine: <{result[i]}> Equals <{ArrayWithDifferentNumsResult[i]}> == {result[i].ToString() == ArrayWithDifferentNumsResult[i]}");
            }
        }

        #endregion
    }
}