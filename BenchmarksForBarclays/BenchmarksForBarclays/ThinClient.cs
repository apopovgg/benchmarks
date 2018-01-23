using System;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Client;

namespace BenchmarksForBarclays
{
    public class ThinClient<T> : ClientWorker<T>
    {
        private readonly IgniteClientConfiguration _clientCfg;

        public ThinClient(List<T> objects, string host) : base ("Thin", objects)
        {
            _clientCfg = new IgniteClientConfiguration
            {
                Host = host,
                SocketTimeout = new TimeSpan(0, 1, 0)
            };
        }
        
        public void Test(int runs)
        { 
            using (var client = Ignition.StartClient(_clientCfg))
            {
                var cacheName = System.Reflection.MethodBase.GetCurrentMethod().Name + typeof(T).Name;
                var cache = client.GetOrCreateCache<int, T>(cacheName);

                cache.RemoveAll();

                TestPut(runs, cache.Put);

                TestGet(runs, cache.Get);

                TestPutAll(runs, cache.PutAll);

                TestGetAll(runs, cache.GetAll);

                client.DestroyCache(cacheName);
            }
        }
    }
}
