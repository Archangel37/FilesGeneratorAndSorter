using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using FileSorter.Implementations;
using FileSorter.Models;

namespace MainBenchmark
{
    [ExcludeFromCodeCoverage]
    [MemoryDiagnoser]
    [RyuJitX64Job]
    [Config(typeof(ConfigBench))]
    public class Benchmark
    {
        private readonly SeparatedLine[] _test;
       
        private class ConfigBench: ManualConfig
        {
            public ConfigBench()
            {
                AddColumn(StatisticColumn.Mean);
                AddColumn(StatisticColumn.Error);
                AddColumn(StatisticColumn.StdDev);
                AddColumn(StatisticColumn.Median);
                AddColumn(StatisticColumn.OperationsPerSecond);
                AddColumn(StatisticColumn.Min);
            }
        }

        
        public Benchmark()
        {
            var list = new List<string>();
            using (var sr = new StreamReader("test.txt"))
            {
                for(var i=0; i<1000; i++)
                    list.Add(sr.ReadLine());
            }

            _test = list.Select(x => x.GetSeparatedLine()).ToArray();
        }
        
        [Benchmark(Description = "BubbleSort")]
        public SeparatedLine[] BubbleSort_Benchmark()
        {
            return Sorting.BubbleSort(_test);
        }
        
        [Benchmark(Description = "InsertionSort")]
        public SeparatedLine[] InsertionSort_Benchmark()
        {
            return Sorting.InsertionSort(_test);
        }
        
        [Benchmark(Description = "OrderBy")]
        public SeparatedLine[] OrderBy_Benchmark()
        {
            return Sorting.OrderBy(_test);
        }

        [Benchmark(Description = "ShakerSort")]
        public SeparatedLine[] ShakerSort_Benchmark()
        {
            return Sorting.ShakerSort(_test);
        }
        
        [Benchmark(Description = "ShellSort")]
        public SeparatedLine[] ShellSort_Benchmark()
        {
            return Sorting.ShellSort(_test);
        }
        
        [Benchmark(Description = "Sort")]
        public SeparatedLine[] Sort_Benchmark()
        {
            return Sorting.Sort(_test);
        }

        [Benchmark(Description = "QuickSort")]
        public SeparatedLine[] QuickSort_Benchmark()
        {
            return Sorting.QuickSort(_test);
        }
        
        [Benchmark(Description = "HybridOptimizedQuickSort")]
        public SeparatedLine[] HybridOptimizedQuickSort_Benchmark()
        {
            return Sorting.HybridOptimizedQuickSort(_test);
        }
    }
}