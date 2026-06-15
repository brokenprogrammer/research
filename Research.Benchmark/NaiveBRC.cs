using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Research.Shared._1BRC;
using Research.Shared._1BRC.Iteration1;

namespace Research.Benchmark;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class NaiveBRC
{
    private string? _path;

    private Naive? _naiveRunner;
    private NaiveParallel? _naiveParallel;
    private BRC? _brc;
    
    [GlobalSetup]
    public void Setup()
    {
        _path = "C:\\Users\\mende\\Documents\\Github\\research\\Data\\measurements-1M.txt";
        _naiveRunner = new Naive(_path);
        _naiveParallel = new NaiveParallel(_path);
        _brc = new BRC(_path);
    }

    [Benchmark(Baseline = true)]
    public Dictionary<string, StationResult> Naive()
    {
        return _naiveRunner!.Collect();
    }
    
    [Benchmark]
    public Dictionary<string, StationResult> NaiveParallel()
    {
        return _naiveParallel!.Collect();
    }
    
    [Benchmark]
    public Dictionary<string, StationResult> Iteration1()
    {
        return _brc!.Collect();
    }
}