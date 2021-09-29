namespace Moerenuma.Benchmark
{
  using BenchmarkDotNet.Reports;
  using BenchmarkDotNet.Running;

  public class Program
  {
    public static void Main(string[] args)
    {
      Summary summary = BenchmarkRunner.Run<ToLowerBenchmark>();
    }
  }
}
