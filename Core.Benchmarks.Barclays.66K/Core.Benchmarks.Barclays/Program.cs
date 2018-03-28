using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Mathematics;
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
                .With(StatisticColumn.CiLower(ConfidenceLevel.L999))
                .With(StatisticColumn.CiUpper(ConfidenceLevel.L999))
                .With(Job.Default.WithLaunchCount(1).WithWarmupCount(Params.Instance.Value.WarmupCount)
                    //.WithIterationTime(new TimeInterval(1, TimeUnit.Second))
                    .WithUnrollFactor(1).WithInvocationCount(1)
                    .WithTargetCount(Params.Instance.Value.TargetCount)
                    .WithRemoveOutliers(false));


            //BenchmarkRunner.Run<Thin.PutBenchmark>(config);
            //BenchmarkRunner.Run<Thick.PutBenchmark>(config);
            BenchmarkRunner.Run<Thin.GetBenchmark>(config);
            BenchmarkRunner.Run<Thick.GetBenchmark>(config);
        }
    }
}
