using System;
using System.Diagnostics;

namespace BenchmarksForBarclays
{
    public class LatencyChecker : IDisposable
    {
        private readonly DateTime _start;
        private readonly string _name;
        private readonly int _count;

        public LatencyChecker(string name, int count)
        {
            Trace.TraceInformation($"{DateTime.UtcNow}: Started {name} measurements...");
            _name = name;
            _count = count;
            _start = DateTime.UtcNow;
        }

        public void Dispose()
        {
            var ts = DateTime.UtcNow - _start;
            var avgMs = ts.TotalMilliseconds / _count;
            var opsPerSec = _count / ts.TotalSeconds;

            Trace.TraceInformation($"{DateTime.UtcNow}: Finished {_name} measurements: {avgMs:0.00} ms, {opsPerSec:0.00} op/sec");
        }
    }
}
