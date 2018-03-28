using System;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Client;

namespace Core.Benchmarks.Barclays.Thin
{
    public class Client
    {
        private readonly IgniteClientConfiguration _clientCfg;

        public Client(string host)
        {
            _clientCfg = new IgniteClientConfiguration
            {
                Host = host,
                SocketTimeout = new TimeSpan(0, 1, 0)
            };
        }

        public IIgniteClient Start()
        {
            return Ignition.StartClient(_clientCfg);
        }
    }
}
