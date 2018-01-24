using System;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thin
{
    public abstract class BaseBenchmark
    {
        protected ICacheClient<int, TestModel> Cache;
        protected readonly Random Random = new Random();
        protected IIgniteClient Client;

        public virtual void GlobalSetup()
        {
            Client = new Client(Params.Instance.Value.Host).Start();
            Cache = Client.GetOrCreateCache<int, TestModel>(GetType().Name);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Client.DestroyCache(Cache.Name);
            Client.Dispose();
        }
    }
}
