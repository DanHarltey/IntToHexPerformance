using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        //[Benchmark(Baseline = true)]
        //public string IntToString() => value.ToString("x");

        //[Benchmark]
        //public string NumberFormating_Current() => NumberFormatingCurrent.FormatInt32( value, "x", null);

        //[Benchmark]
        //public string NumberFormating_New() => NumberFormatingNew.FormatInt32(value, "x", null);


        [Benchmark(Baseline = true)]
        public string[] SelectToArray() => objBars.Select(x => x.Barcode).ToArray();

        [Benchmark]
        public string[] HandSelectToArray()
        {
            string[] array = new string[objBars.Count];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = objBars[i].Barcode;
            }
            return array;
        }

        [Benchmark]
        public Dictionary<int, List<ObjBar>> GroupByToDictionaryToList() => objBars.GroupBy(x => x.DoorId).ToDictionary(x => x.Key, x => x.ToList());


        [Benchmark]
        public Dictionary<int, List<ObjBar>> HandGroupByToDictionaryToList() => GetPackagesByDoorId(objBars);
        //[Benchmark]
        //public string[] New22()
        //{
        //    string[] array = new string[objBars.Count];

        //    for (int i = 0; i < array.l.Count; i++)
        //    {
        //        array[i] = objBars[i].Barcode;
        //    }
        //    return array;
        //}

        //[Benchmark]
        //public string[] New2()
        //{
        //    string[] array = new string[objBars.Count];

        //    for (int i = array.Length - 1; i >= 0; i--)
        //    {
        //        array[i] = objBars[i].Barcode;
        //    }
        //    return array;
        //}

        //[Benchmark]
        //public string[] New3()
        //{
        //    string[] array = new string[objBars.Count];

        //    for (int i = 0; i < objBars.Count; i++)
        //    {
        //        array[i] = objBars[i].Barcode;
        //    }
        //    return array;
        //}


        private static Dictionary<int, List<ObjBar>> GetPackagesByDoorId(List<ObjBar> packages)
        {
            var packagesByDoorId = new Dictionary<int, List<ObjBar>>();


            foreach (var package in packages)
            {
                if (packagesByDoorId.TryGetValue(package.DoorId, out var doorsPackages))
                {
                    doorsPackages.Add(package);
                }
                else
                {
                    packagesByDoorId[package.DoorId] = new List<ObjBar> { package };
                }
            }

            return packagesByDoorId;
        }

        //[Benchmark]
        //public string[] New22()
        //{
        //    string[] array = new string[objBars.Count];

        //    for (int i = 0; i < array.l.Count; i++)
        //    {
        //        array[i] = objBars[i].Barcode;
        //    }
        //    return array;
        //}

        //[Benchmark]
        //public string[] New2()
        //{
        //    string[] array = new string[objBars.Count];

        //    for (int i = array.Length - 1; i >= 0; i--)
        //    {
        //        array[i] = objBars[i].Barcode;
        //    }
        //    return array;
        //}

        //[Benchmark]
        //public string[] New3()
        //{
        //    string[] array = new string[objBars.Count];

        //    for (int i = 0; i < objBars.Count; i++)
        //    {
        //        array[i] = objBars[i].Barcode;
        //    }
        //    return array;
        //}

        private static List<ObjBar> objBars;

        static Program()
        {
            const int size = 10_000;
            objBars = new List<ObjBar>(size);
            for (int i = 0; i < objBars.Count; i++)
            {
                objBars.Add(new ObjBar { DoorId = i / 2 });
            }
        }

        public class ObjBar
        {
            public int DoorId { get; set; }
            public string Barcode => "Barcode";
        }
    }
}
