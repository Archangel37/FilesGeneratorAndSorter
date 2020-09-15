using BenchmarkDotNet.Running;

namespace MainBenchmark
{
    internal static class Program
    {
        private static void Main()
        {
            //https://www.researchgate.net/publication/271642655_Performance_Analysis_of_Sorting_Algorithms_with_C
            // Summary from the article: Ubuntu - insertion sort wins, for Windows - Quick Sort
            // I've tested .Net Core Win/Ubuntu - Quick Sort wins in both OS
            // But it's not a 100% Truth for any array - for big number of elements Quick Sort may be not a leader..
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}