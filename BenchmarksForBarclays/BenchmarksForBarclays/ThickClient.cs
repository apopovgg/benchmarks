using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Binary;
using Apache.Ignite.Core.Communication.Tcp;
using Apache.Ignite.Core.Compute;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Static;

namespace BenchmarksForBarclays
{
    public class ThickClient<T> : ClientWorker<T> where T : SomeNativeClass
    {
        private readonly IgniteConfiguration _clientCfg;

        public ThickClient(List<T> objects, string host) : base("Thick", objects)
        {
            var currentDir = Directory.GetCurrentDirectory();

            _clientCfg = new IgniteConfiguration
            {
                ClientMode = true,
                DiscoverySpi = new TcpDiscoverySpi
                {
                    IpFinder = new TcpDiscoveryStaticIpFinder
                    {
                        Endpoints = new List<string> { host }
                    },
                    LocalPort = 47500
                },
                CommunicationSpi = new TcpCommunicationSpi()
                {
                    LocalPort = 47100
                },
                JvmClasspath = $@"-Djava.class.path={currentDir}\libs",
                IgniteHome = currentDir,
                JvmOptions = new List<string> { "-Xms2g", "-Xmx2g", "-XX:+AggressiveOpts", "-XX:+UseG1GC", "-Djava.net.preferIPv4Stack=true" }
            };
        }

        public void Test(int runs)
        {
            using (var client = Ignition.Start(_clientCfg))
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

        public void TestBinary(int runs)
        {
            using (var client = Ignition.Start(_clientCfg))
            {
                var cacheName = System.Reflection.MethodBase.GetCurrentMethod().Name +typeof(T).Name + "Binary";
                var cache = client.GetOrCreateCache<int, IBinaryObject>(cacheName).WithKeepBinary<int, IBinaryObject>();

                var builder = client.GetBinary().GetBuilder(typeof(T).Name + "Binary");

                var binaryObjects = Objects.Select(obj => builder.SetDoubleArrayField("data", obj.Data).Build()).ToList();

                cache.RemoveAll();

                var worker = new ClientWorker<IBinaryObject>("ThickBinary", binaryObjects);

                worker.TestPut(runs, cache.Put);

                worker.TestGet(runs, cache.Get);

                worker.TestPutAll(runs, cache.PutAll);

                worker.TestGetAll(runs, cache.GetAll);

                client.DestroyCache(cacheName);
            }
        }
    }
}
