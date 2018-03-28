using System.Collections.Generic;
using System.IO;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Communication.Tcp;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Static;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays.Thick
{
    public class Client
    {
        private readonly IgniteConfiguration _clientCfg;

        public Client(string host)
        {
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
                IgniteHome = Params.Instance.Value.IgniteHome,
                JvmOptions = new List<string> { "-Xms8g", "-Xmx8g", "-XX:+AggressiveOpts", "-XX:+UseG1GC", "-Djava.net.preferIPv4Stack=true" }
            };
        }

        public IIgnite Start()
        {
            return Ignition.Start(_clientCfg);
        }
    }
}
