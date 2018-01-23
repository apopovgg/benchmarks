using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BenchmarksForBarclays
{
    class Program
    {
        public static int Count = 10000;
        public static int Runs = 5;
        public static string Host = "localhost";//"172.25.1.40"; //"18.218.107.126";

        static void Main(string[] args)
        {
            Trace.TraceInformation($"{DateTime.UtcNow}: Host={Host}, Count={Count}");

            var someNativeObjects = GenerateNativeObjects(Count);

            new ThinClient<SomeNativeClass>(someNativeObjects, Host).Test(Runs);

            new ThickClient<SomeNativeClass>(someNativeObjects, Host).Test(Runs);

            new ThickClient<SomeNativeClass>(someNativeObjects, Host).TestBinary(Runs);
        }

        private static List<SomeNativeClass> GenerateNativeObjects(int count)
        {
            var result = new List<SomeNativeClass>();
            for (var i = 0; i < count; i++)
            {
                result.Add(new SomeNativeClass());
            }
            return result;
        }

    }
}
