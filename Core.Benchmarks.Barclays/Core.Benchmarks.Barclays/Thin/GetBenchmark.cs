using System.Collections.Generic;
using System.Linq;
using Apache.Ignite.Core.Binary;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Client.Cache;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thin
{
    public class GetBenchmark : BaseBenchmark
    {
        private ICacheClient<int, IBinaryObject> _binaryCache;

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();
            var models = new Data().GetModels();

            for (var i = 0; i < models.Length; i++)
            {
                Cache.Put(i, models[i]);
            }

            _binaryCache = Cache.WithKeepBinary<int, IBinaryObject>();
        }

        [BenchmarkCategory("SingleOperation")]
        [Benchmark(Description = "Thin.Get")]
        public TestModel Get()
        {
            return Cache.Get(Random.Next(0, Params.Instance.Value.TotalObjects));
        }

        [Benchmark(Description = "Thin.GetAll")]
        public ICollection<ICacheEntry<int, TestModel>> GetAll()
        {
            var first = Random.Next(0, Params.Instance.Value.TotalObjects - Params.Instance.Value.BatchSize);
            var keys = Enumerable.Range(first, Params.Instance.Value.BatchSize);

            return Cache.GetAll(keys);
        }

        [BenchmarkCategory("SingleOperation")]
        [Benchmark(Description = "Thin.Get.Binary")]
        public IBinaryObject GetBinary()
        {
            return _binaryCache.Get(Random.Next(0, Params.Instance.Value.TotalObjects));
        }

        [Benchmark(Description = "Thin.GetAll.Binary")]
        public ICollection<ICacheEntry<int, IBinaryObject>> GetAllBinary()
        {
            var first = Random.Next(0, Params.Instance.Value.TotalObjects - Params.Instance.Value.BatchSize);
            var keys = Enumerable.Range(first, Params.Instance.Value.BatchSize);

            return _binaryCache.GetAll(keys);
        }
    }
}
