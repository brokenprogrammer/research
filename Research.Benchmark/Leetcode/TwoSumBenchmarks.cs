using BenchmarkDotNet.Attributes;
using Research.Shared.Leetcode;
using Random = System.Random;

namespace Research.Benchmark.Leetcode;

[ShortRunJob]
[MemoryDiagnoser]           // reports allocations per operation, not just time
[RankColumn]                // ranks results within each N group
public class TwoSumBenchmarks
{
    // BenchmarkDotNet will run the whole suite once per value here
    [Params(10, 100, 1_000, 10_000, 100_000)]
    public int N;
    
    private int[] _nums;
    private int _target;
    
    // Runs once before each N group — build a realistic worst-ish case:
    // the matching pair is deliberately placed far apart (first and last
    // elements) so we're not accidentally flattering algorithms that
    // benefit from an "easy" answer position.
    [GlobalSetup]
    public void Setup()
    {
        var rnd = new Random(42); // fixed seed => reproducible runs
        _nums = new int[N];
        for (int i = 0; i < N; i++)
            _nums[i] = rnd.Next(-1_000_000, 1_000_000);

        // Force a known answer at the extremes of the array
        _nums[0] = 5;
        _nums[N - 1] = 15;
        _target = 20;
    }
    
    [Benchmark(Baseline = true)]
    public int[] DictionaryHashMap() => TwoSum.TwoSumDictionary(_nums, _target);

    [Benchmark]
    public int[] CustomOpenAddressing() => TwoSum.TwoSumCustomHash(_nums, _target);
    
    [Benchmark]
    public int[] CustomOpenAddressingAoS() => TwoSum.TwoSumCustomHashAoS(_nums, _target);
    
    [Benchmark]
    public int[] CustomOpenAddressingAoSArrayPool() => TwoSum.TwoSumCustomHashAoSArrayPool(_nums, _target);
}