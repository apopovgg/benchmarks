using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Benchmarks.Barclays.Models
{
    public class ModelB
    {
        public double[] Data = new double[1000 * 66];

        public ModelB()
        {
        }

        public ModelB(Random random)
        {
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = random.NextDouble();
            }
        }
    }
}
