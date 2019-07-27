using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
//using Xunit;

namespace ToHexPerformance
{
    [MemoryDiagnoser]
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
        }

        private const int value = 10;


        [Benchmark]
        public string IntToString() => value.ToString("x");

        [Benchmark]
        public string NumberFormating_Current() => NumberFormatingCurrent.FormatInt32( value, "x", null);

        [Benchmark]
        public string NumberFormating_New() => NumberFormatingNew.FormatInt32(value, "x", null);
    }
}
