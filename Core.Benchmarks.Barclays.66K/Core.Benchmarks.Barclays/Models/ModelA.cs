using System;


namespace Core.Benchmarks.Barclays.Models
{
    public class ModelA
    {
        public double[] Data = new double[1000 * 66];

        public ModelA()
        {
        }

        public ModelA(Random random)
        {
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = random.NextDouble();
            }
        }
    }
}
