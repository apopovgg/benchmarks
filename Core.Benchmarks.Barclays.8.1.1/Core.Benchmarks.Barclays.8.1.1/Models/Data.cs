using System;
using Apache.Ignite.Core.Binary;

namespace Core.Benchmarks.Barclays.Models
{
    public class Data
    {
        public TestModel[] GetModels()
        {
            var data = new TestModel[Params.Instance.Value.TotalObjects];
            var random = new Random();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new TestModel(random);
            }
            return data;
        }

        public IBinaryObject[] GetBinaryModels(IBinaryObjectBuilder builder)
        {
            var entry = new double[1000 * 32];
            var data = new IBinaryObject[Params.Instance.Value.TotalObjects];
            var random = new Random();

            for (var i = 0; i < data.Length; i++)
            {
                for (var j = 0; j < entry.Length; j++)
                {
                    entry[j] = random.NextDouble();
                }

                data[i] = builder.SetDoubleArrayField("data", entry).Build();
            }
            return data;
        }
    }
}
