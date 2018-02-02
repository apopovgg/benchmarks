using System;

namespace Core.Benchmarks.Barclays.Models
{
    public class TestModel
    {
        public double[] Data = new double[1000 * 32];

        public TestModel()
        {
        }

        public TestModel(Random random)
        {
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = random.NextDouble();
            }
        }
    }
}
