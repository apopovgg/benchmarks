using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Core.Benchmarks.Barclays.Models;

namespace Core.Benchmarks.Barclays
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ManualConfig
                .Create(DefaultConfig.Instance)
                .With(StatisticColumn.Min)
                .With(StatisticColumn.Median)
                .With(StatisticColumn.P95)
                .With(StatisticColumn.Max)
                .With(Job.Default.WithLaunchCount(1).WithWarmupCount(10)
                    .WithIterationTime(new TimeInterval(1, TimeUnit.Second))
                    .WithTargetCount(Params.Instance.Value.TargetCount));

            BenchmarkRunner.Run<Thin.GetBenchmark>(config);
            BenchmarkRunner.Run<Thin.PutBenchmark>(config);
            BenchmarkRunner.Run<Thick.GetBenchmark>(config);
            BenchmarkRunner.Run<Thick.PutBenchmark>(config);
        }
    }
}
