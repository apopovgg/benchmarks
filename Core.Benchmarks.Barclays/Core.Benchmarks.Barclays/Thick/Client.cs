using System.Collections.Generic;
using System.IO;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Communication.Tcp;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Static;

namespace Core.Benchmarks.Barclays.Thick
{
    public class Client
    {
        private readonly IgniteConfiguration _clientCfg;

        public Client(string host)
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

        public IIgnite Start()
        {
            return Ignition.Start(_clientCfg);
        }
    }
}
