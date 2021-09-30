namespace Moerenuma.Benchmark
{
  using BenchmarkDotNet.Attributes;

  [MemoryDiagnoser]
  public class ArrayAllocationBenchmark
  {
    private const int Size = 1 << 15;

    [Benchmark]
    public void Ulongs()
    {
      ulong[] data = new ulong[Size / sizeof(ulong)];
      for (int i = 0; i < data.Length; i++)
      {
        data[i] |= (ulong)i % sizeof(ulong);
      }
    }

    [Benchmark]
    public void Bools()
    {
      bool[] data = new bool[Size];
      for (int i = 0; i < data.Length; i++)
      {
        data[i] = true;
      }
    }
  }
}
