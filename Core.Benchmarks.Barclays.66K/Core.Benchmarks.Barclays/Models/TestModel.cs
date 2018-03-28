using System;
using Apache.Ignite.Core.Binary;

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

        public static IBinaryObject BinaryModel(Random random, IBinaryObjectBuilder builder)
        {
            var entry = new double[1000 * 32];

            for (var j = 0; j < entry.Length; j++)
            {
                entry[j] = random.NextDouble();
            }

            return builder.SetDoubleArrayField("data", entry).Build();
        }
    }
}
