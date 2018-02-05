using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thick
{
    public class PutBenchmark : BaseBenchmark
    {
        private TestModel[] _models;
        private byte[] _pingBuffer;

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            _models = new Data().GetModels();

            // 32k-bytes ping (please note, that TestModel is 250k-bytes)
            _pingBuffer = Encoding.ASCII.GetBytes("a".PadRight(32 * 1024, 'b'));
        }

        [BenchmarkCategory("SingleOperation")]
        [Benchmark(Description = "Thick.Put")]
        public void Put()
        {
            var idx = Random.Next(0, Params.Instance.Value.TotalObjects);
            Cache.Put(idx, _models[idx]);
        }

        [BenchmarkCategory("SingleOperation")]
        [Benchmark(Description = "Thick.Sleep")]
        public TestModel Sleep()
        {
            // do at least one memory allocation
            var x = new TestModel();

            Thread.Sleep(2);
            return x;
        }

        [BenchmarkCategory("SingleOperation")]
        [Benchmark(Description = "Thick.Ping")]
        public void Ping()
        {
            var pingSender = new Ping();

            pingSender.Send(Params.Instance.Value.Host, 1000, _pingBuffer);
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
