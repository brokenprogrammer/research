using BenchmarkDotNet.Attributes;

namespace Research.Benchmark;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class SimpleAllocations
{
    public struct PointStruct
    {
        public int X;
        public int Y;
    }

    public class PointClass
    {
        public int X;
        public int Y;
    }
    
    [Benchmark(Baseline = true)]
    public PointStruct PlainStruct() => new PointStruct { X = 1, Y = 2 };

    [Benchmark]
    public object BoxedStruct()
    {
        PointStruct s = new PointStruct { X = 1, Y = 2 };
        return s;
    }

    [Benchmark]
    public PointClass PlainClass() => new PointClass { X = 1, Y = 2 };

    [Benchmark]
    public object ClassAsObject()
    {
        PointClass c = new PointClass { X = 1, Y = 2 };
        return c;
    }
}