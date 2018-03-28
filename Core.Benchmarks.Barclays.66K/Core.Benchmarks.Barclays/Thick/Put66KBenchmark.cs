using System.Collections.Generic;
using Apache.Ignite.Core.Binary;
using Apache.Ignite.Core.Cache;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thick
{
    public class Put66KBenchmark : BaseBenchmark
    {
        private TestModel66K[] _models;
        private IBinaryObject[] _binaryModels;
        private ICache<int, IBinaryObject> _binaryCache;

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            _models = new Data66K().GetModels();

            var builder = Client.GetBinary().GetBuilder(GetType().Name);

            _binaryModels = new Data().GetBinaryModels(builder);

            _binaryCache = Cache.WithKeepBinary<int, IBinaryObject>();
        }

        [BenchmarkCategory("SingleOperation")]
        [Benchmark(Description = "Thick.Put.66k")]
        public void Put()
        {
            var idx = Random.Next(0, Params.Instance.Value.TotalObjects);
            Cache.Put(idx, _models[idx]);
        }

        [Benchmark(Description = "Thick.PutAll.66k")]
        public void PutAll()
        {
            var first = Random.Next(0, Params.Instance.Value.TotalObjects - Params.Instance.Value.BatchSize);
            var keyValues = new KeyValuePair<int, TestModel>[Params.Instance.Value.BatchSize];

            for (var i = 0; i < Params.Instance.Value.BatchSize; i++)
            {
                keyValues[i] = new KeyValuePair<int, TestModel>(i + first, _models[i + first]);
            }

            Cache.PutAll(keyValues);
        }

        //[BenchmarkCategory("SingleOperation")]
        //[Benchmark(Description = "Thick.Put.Binary")]
        public void PutBinary()
        {
            var idx = Random.Next(0, Params.Instance.Value.TotalObjects);
            _binaryCache.Put(idx, _binaryModels[idx]);
        }

        [Benchmark(Description = "Thick.PutAll.66k.Binary")]
        public void PutAllBinary()
        {
            var first = Random.Next(0, Params.Instance.Value.TotalObjects - Params.Instance.Value.BatchSize);
            var keyValues = new KeyValuePair<int, IBinaryObject>[Params.Instance.Value.BatchSize];

            for (var i = 0; i < Params.Instance.Value.BatchSize; i++)
            {
                keyValues[i] = new KeyValuePair<int, IBinaryObject>(i + first, _binaryModels[i + first]);
            }

            _binaryCache.PutAll(keyValues);
        }
    }
}
