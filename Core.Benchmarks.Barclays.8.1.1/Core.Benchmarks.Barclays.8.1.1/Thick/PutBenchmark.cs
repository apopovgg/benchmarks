using System.Collections.Generic;
using Apache.Ignite.Core.Binary;
using Apache.Ignite.Core.Cache;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thick
{
    public class PutBenchmark : BaseBenchmark
    {
        private TestModel[] _models;

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            _models = new Data().GetModels();
        }

        [BenchmarkCategory("SingleOperation")]
        [Benchmark(Description = "Thick.Put")]
        public void Put()
        {
            var idx = Random.Next(0, Params.Instance.Value.TotalObjects);
            Cache.Put(idx, _models[idx]);
        }

        [Benchmark(Description = "Thick.PutAll")]
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
    }
}
