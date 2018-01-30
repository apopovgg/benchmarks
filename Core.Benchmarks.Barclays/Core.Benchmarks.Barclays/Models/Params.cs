﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Core.Benchmarks.Barclays.Models
{
    public class Params
    {
        public string Host { get; }
        public int TotalObjects { get; }
        public int TargetCount { get; }
        public int BatchSize { get; }
        public bool SingleOperationsOnly { get; }

        public static Lazy<Params> Instance = new Lazy<Params>(() => new Params());

        private Params()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var cfg = builder.Build();

            Host = cfg["Host"];
            TotalObjects = int.Parse(cfg["TotalObjects"]);
            TargetCount = int.Parse(cfg["TargetCount"]);
            BatchSize = int.Parse(cfg["BatchSize"]);
            SingleOperationsOnly = bool.Parse(cfg["SingleOperationsOnly"]);
        }
    }
}
