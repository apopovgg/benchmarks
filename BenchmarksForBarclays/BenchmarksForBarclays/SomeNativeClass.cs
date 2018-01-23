using System;

namespace BenchmarksForBarclays
{
    public class SomeNativeClass
    {
        private static readonly Random Random = new Random();

        public double[] Data = new double[1000 * 32];

        public SomeNativeClass()
        {
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = Random.NextDouble();
            }
        }
    }
}
