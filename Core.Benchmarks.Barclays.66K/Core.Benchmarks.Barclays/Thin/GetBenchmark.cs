using System;
using System.Collections.Generic;
using System.Linq;
using Apache.Ignite.Core.Cache.Affinity;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thin
{
    public class GetBenchmark
    {
        private ICacheClient<int, ModelA> _cacheA;
        private ICacheClient<int, ModelB> _cacheB;
        private ICacheClient<int, ModelC> _cacheC;
        private ICacheClient<int, ModelD> _cacheD;
        private ICacheClient<int, ModelE> _cacheE;
        private ICacheClient<AffinityKey, ModelA> _cacheF;
        private ICacheClient<AffinityKey, ModelB> _cacheG;
        private ICacheClient<AffinityKey, ModelC> _cacheH;
        private ICacheClient<AffinityKey, ModelD> _cacheI;
        private ICacheClient<AffinityKey, ModelE> _cacheJ;

        private readonly Random _random = new Random();
        private IIgniteClient _client;
        private int _max;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _client = new Client(Params.Instance.Value.Host).Start();

            _cacheA = _client.GetOrCreateCache<int, ModelA>(typeof(ModelA).Name);
            _cacheB = _client.GetOrCreateCache<int, ModelB>(typeof(ModelB).Name);
            _cacheC = _client.GetOrCreateCache<int, ModelC>(typeof(ModelC).Name);
            _cacheD = _client.GetOrCreateCache<int, ModelD>(typeof(ModelD).Name);
            _cacheE = _client.GetOrCreateCache<int, ModelE>(typeof(ModelE).Name);

            _cacheF = _client.GetOrCreateCache<AffinityKey, ModelA>(typeof(ModelA).Name + "Affinity");
            _cacheG = _client.GetOrCreateCache<AffinityKey, ModelB>(typeof(ModelB).Name + "Affinity");
            _cacheH = _client.GetOrCreateCache<AffinityKey, ModelC>(typeof(ModelC).Name + "Affinity");
            _cacheI = _client.GetOrCreateCache<AffinityKey, ModelD>(typeof(ModelD).Name + "Affinity");
            _cacheJ = _client.GetOrCreateCache<AffinityKey, ModelE>(typeof(ModelE).Name + "Affinity");

            _max = Params.Instance.Value.TotalObjects;

            var data = new Data();
            var keys = Enumerable.Range(0, _max).ToArray();

            FillCache(_cacheA, keys, data.GetModelsA());
            FillCache(_cacheB, keys, data.GetModelsB());
            FillCache(_cacheC, keys, data.GetModelsC());
            FillCache(_cacheD, keys, data.GetModelsD());
            FillCache(_cacheE, keys, data.GetModelsE());

            var affinityKeys = Enumerable.Range(0, _max).Select(k => new AffinityKey(k, k / 10)).ToArray();

            FillCache(_cacheF, affinityKeys, data.GetModelsA());
            FillCache(_cacheG, affinityKeys, data.GetModelsB());
            FillCache(_cacheH, affinityKeys, data.GetModelsC());
            FillCache(_cacheI, affinityKeys, data.GetModelsD());
            FillCache(_cacheJ, affinityKeys, data.GetModelsE());
        }

        private static void FillCache<K, V>(ICacheClient<K, V> cache, IEnumerable<K> keys, IEnumerable<V> values)
        {
            var batch = new List<KeyValuePair<K, V>>();

            using (var vEnum = values.GetEnumerator())
            {
                foreach (var key in keys)
                {
                    vEnum.MoveNext();
                    batch.Add(new KeyValuePair<K, V>(key, vEnum.Current));
                    if (batch.Count == 100)
                    {
                        cache.PutAll(batch);
                        batch.Clear();
                    }
                }
            }

            if (batch.Count > 0)
            {
                cache.PutAll(batch);
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _client.DestroyCache(_cacheA.Name);
            _client.DestroyCache(_cacheB.Name);
            _client.DestroyCache(_cacheC.Name);
            _client.DestroyCache(_cacheD.Name);
            _client.DestroyCache(_cacheE.Name);

            _client.DestroyCache(_cacheF.Name);
            _client.DestroyCache(_cacheG.Name);
            _client.DestroyCache(_cacheH.Name);
            _client.DestroyCache(_cacheI.Name);
            _client.DestroyCache(_cacheJ.Name);

            _client.Dispose();
        }

        [Benchmark(Description = "Thin.Get")]
        public int Get()
        {
            var idxA = _random.Next(0, _max);
            var idxB = _random.Next(0, _max);
            var idxC = _random.Next(0, _max);
            var idxD = _random.Next(0, _max);
            var idxE = _random.Next(0, _max);

            var a = _cacheA.Get(idxA);
            var b = _cacheB.Get(idxB);
            var c = _cacheC.Get(idxC);
            var d = _cacheD.Get(idxD);
            var e = _cacheE.Get(idxE);

            return a.Data.Length + b.Data.Length + c.Field10 + d.Field10 + e.Field10;
        }

        [Benchmark(Description = "Thin.Get.Affinity")]
        public int GetWithAffinity()
        {
            var aff = _random.Next(0, _max) / 10;
            var idxA = _random.Next(0, 10) + aff * 10;
            var idxB = _random.Next(0, 10) + aff * 10;
            var idxC = _random.Next(0, 10) + aff * 10;
            var idxD = _random.Next(0, 10) + aff * 10;
            var idxE = _random.Next(0, 10) + aff * 10;

            var a = _cacheF.Get(new AffinityKey(idxA, aff));
            var b = _cacheG.Get(new AffinityKey(idxB, aff));
            var c = _cacheH.Get(new AffinityKey(idxC, aff));
            var d = _cacheI.Get(new AffinityKey(idxD, aff));
            var e = _cacheJ.Get(new AffinityKey(idxE, aff));

            return a.Data.Length + b.Data.Length + c.Field10 + d.Field10 + e.Field10;
        }
    }
}
