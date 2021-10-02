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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "This is the method we want to benchmark")]
    public string ToLowerInvariant() => this.data.ToLowerInvariant();
  }
}
