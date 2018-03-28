using System;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cache.Affinity;
using BenchmarkDotNet.Attributes;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thick
{
    public class PutBenchmark
    {
        private ModelA[] _modelsA;
        private ModelB[] _modelsB;
        private ModelC[] _modelsC;
        private ModelD[] _modelsD;
        private ModelE[] _modelsE;
        private ICache<int, ModelA> _cacheA;
        private ICache<int, ModelB> _cacheB;
        private ICache<int, ModelC> _cacheC;
        private ICache<int, ModelD> _cacheD;
        private ICache<int, ModelE> _cacheE;
        private ICache<AffinityKey, ModelA> _cacheF;
        private ICache<AffinityKey, ModelB> _cacheG;
        private ICache<AffinityKey, ModelC> _cacheH;
        private ICache<AffinityKey, ModelD> _cacheI;
        private ICache<AffinityKey, ModelE> _cacheJ;

        private readonly Random _random = new Random();
        private IIgnite _client;
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

            _modelsA = new Data().GetModelsA();
            _modelsB = new Data().GetModelsB();
            _modelsC = new Data().GetModelsC();
            _modelsD = new Data().GetModelsD();
            _modelsE = new Data().GetModelsE();

            _max = Params.Instance.Value.TotalObjects;
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

        [Benchmark(Description = "Thick.Put")]
        public void Put()
        {
            var idxA = _random.Next(0, _max);
            var idxB = _random.Next(0, _max);
            var idxC = _random.Next(0, _max);
            var idxD = _random.Next(0, _max);
            var idxE = _random.Next(0, _max);

            _cacheA.Put(idxA, _modelsA[idxA]);
            _cacheB.Put(idxB, _modelsB[idxB]);
            _cacheC.Put(idxC, _modelsC[idxC]);
            _cacheD.Put(idxD, _modelsD[idxD]);
            _cacheE.Put(idxE, _modelsE[idxE]);
        }

        [Benchmark(Description = "Thick.Put.Affinity")]
        public void PutWithAffinity()
        {
            var idxA = _random.Next(0, _max);
            var idxB = _random.Next(0, _max);
            var idxC = _random.Next(0, _max);
            var idxD = _random.Next(0, _max);
            var idxE = _random.Next(0, _max);
            var aff = _random.Next(0, _max);

            _cacheF.Put(new AffinityKey(idxA, aff), _modelsA[idxA]);
            _cacheG.Put(new AffinityKey(idxB, aff), _modelsB[idxB]);
            _cacheH.Put(new AffinityKey(idxC, aff), _modelsC[idxC]);
            _cacheI.Put(new AffinityKey(idxD, aff), _modelsD[idxD]);
            _cacheJ.Put(new AffinityKey(idxE, aff), _modelsE[idxE]);
        }
    }
}
