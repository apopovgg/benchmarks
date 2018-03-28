using System;
using Apache.Ignite.Core.Binary;

namespace Core.Benchmarks.Barclays.Models
{
    public class Data
    {
        public ModelA[] GetModelsA()
        {
            var data = new ModelA[Params.Instance.Value.TotalObjects];
            var random = new Random();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new ModelA(random);
            }
            return data;
        }

        public ModelB[] GetModelsB()
        {
            var data = new ModelB[Params.Instance.Value.TotalObjects];
            var random = new Random();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new ModelB(random);
            }
            return data;
        }

        public ModelC[] GetModelsC()
        {
            var data = new ModelC[Params.Instance.Value.TotalObjects];
            var random = new Random();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new ModelC(random);
            }
            return data;
        }

        public ModelD[] GetModelsD()
        {
            var data = new ModelD[Params.Instance.Value.TotalObjects];
            var random = new Random();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new ModelD(random);
            }
            return data;
        }

        public ModelE[] GetModelsE()
        {
            var data = new ModelE[Params.Instance.Value.TotalObjects];
            var random = new Random();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new ModelE(random);
            }
            return data;
        }
    }
}
