using BenchmarkDotNet.Attributes;
using Research.Shared.Leetcode;
using Random = System.Random;

namespace Research.Benchmark.Leetcode;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
[RankColumn]
public class TwoSumBenchmarks
{
    [Params(10, 100, 1_000, 10_000, 100_000)]
    public int N;
    
    private int[] _nums;
    private int _target;
    
    [GlobalSetup]
    public void Setup()
    {
        var rnd = new Random(42); // fixed seed => reproducible runs
        _nums = new int[N];
        for (int i = 0; i < N; i++)
            _nums[i] = rnd.Next(-1_000_000, 1_000_000);
        
        _nums.Sort();
        
        // Force a known answer at the extremes of the array
        _target = _nums[N - 1] + _nums[0];
    }
    
    [Benchmark(Baseline = true)]
    public int[] DictionaryHashMap() => TwoSum.TwoSumDictionary(_nums, _target);

    [Benchmark]
    public int[] CustomOpenAddressing() => TwoSum.TwoSumCustomHash(_nums, _target);
    
    [Benchmark]
    public int[] CustomOpenAddressingAoS() => TwoSum.TwoSumCustomHashAoS(_nums, _target);
    
    // [Benchmark]
    // public int[] CustomOpenAddressingAoSArrayPool() => TwoSum.TwoSumCustomHashAoSArrayPool(_nums, _target);
    
    [Benchmark]
    public int[] CustomOpenAddressingSentinel() => TwoSum.TwoSumCustomHashSentinel(_nums, _target);
    
    [Benchmark]
    public int[] CustomOpenAddressingSentinelNoFib() => TwoSum.TwoSumCustomHashSentinelNoFib(_nums, _target);
}