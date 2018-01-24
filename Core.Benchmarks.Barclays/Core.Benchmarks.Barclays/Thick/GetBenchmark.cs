using System.Collections.Generic;
using System.Linq;
using Apache.Ignite.Core.Cache;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thick
{
    public class GetBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();
            var models = new Data().GetModels();

            for (var i = 0; i < models.Length; i++)
            {
                Cache.Put(i, models[i]);
            }
        }

        [Benchmark(Description = "Thick.Get")]
        public TestModel Get()
        {
            return Cache.Get(Random.Next(0, Params.Instance.Value.TotalObjects));
        }

        [Benchmark(Description = "Thick.GetAll")]
        public ICollection<ICacheEntry<int, TestModel>> GetAll()
        {
            var first = Random.Next(0, Params.Instance.Value.TotalObjects - Params.Instance.Value.BatchSize);
            var keys = Enumerable.Range(first, Params.Instance.Value.BatchSize);

            return Cache.GetAll(keys);
        }
    }
}
