using System;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thick
{
    public abstract class BaseBenchmark
    {
        protected ICache<int, TestModel> Cache;
        protected readonly Random Random = new Random();
        protected IIgnite Client;

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
