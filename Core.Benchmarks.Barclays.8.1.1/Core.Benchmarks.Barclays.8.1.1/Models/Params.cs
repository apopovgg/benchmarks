using System;
using System.Configuration;

namespace Core.Benchmarks.Barclays.Models
{
    public class Params
    {
        public string Host { get; }
        public int TotalObjects { get; }
        public int TargetCount { get; }
        public int WarmupCount { get; }
        public int BatchSize { get; }
        public bool SingleOperationsOnly { get; }
        public string IgniteHome { get; }

        public static Lazy<Params> Instance = new Lazy<Params>(() => new Params());

        private Params()
        {
            var cfg = ConfigurationManager.AppSettings;

            Host = cfg["Host"];
            TotalObjects = int.Parse(cfg["TotalObjects"]);
            TargetCount = int.Parse(cfg["TargetCount"]);
            WarmupCount = int.Parse(cfg["WarmupCount"]);
            BatchSize = int.Parse(cfg["BatchSize"]);
            SingleOperationsOnly = bool.Parse(cfg["SingleOperationsOnly"]);
            IgniteHome = cfg["IgniteHome"];
        }
    }
}
