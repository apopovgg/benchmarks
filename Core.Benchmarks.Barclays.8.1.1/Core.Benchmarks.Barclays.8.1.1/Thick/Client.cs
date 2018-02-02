using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
                JvmOptions = "-Xms2g -Xmx2g -XX:+AlwaysPreTouch -XX:+UseG1GC -XX:+ScavengeBeforeFullGC -XX:+DisableExplicitGC -Djava.net.preferIPv4Stack=true -XX:MaxGCPauseMillis=10".Split(new[] { ' ' }).ToList()

                //JvmOptions = new List<string> { "-Xms2g", "-Xmx2g", "-XX:+AggressiveOpts", "-XX:+UseG1GC", "-Djava.net.preferIPv4Stack=true" }
            };
        }

        public IIgnite Start()
        {
            return Ignition.Start(_clientCfg);
        }
    }
}
