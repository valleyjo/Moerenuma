namespace Moerenuma.Benchmark
{
  using System;
  using System.Text;
  using BenchmarkDotNet.Attributes;

  [MemoryDiagnoser]
  public class ToLowerBenchmark
  {
    private readonly string data;

    public ToLowerBenchmark()
    {
      var sb = new StringBuilder();
      var random = new Random();
      int strlen = 10_000;
      for (int i = 0; i < strlen; i++)
      {
        char c = (char)random.Next('A', 'z');
        sb.Append(c);
      }

      this.data = sb.ToString();
    }

    [Benchmark]
    public string ToLower() => this.data.ToLower();

    [Benchmark]
    public string ToLowerInvariant() => this.data.ToLowerInvariant();
  }
}
