using System.Buffers;

namespace Research.Shared.Leetcode;

public static class TwoSum
{
    public static int[] TwoSumDictionary(int[] nums, int target)
    {
        var map = new Dictionary<int, int>(nums.Length);
        
        for (var i = 0; i < nums.Length; ++i)
        {
            var missing = target - nums[i];

            if (map.TryGetValue(missing, out var index))
            {
                return new int[] { index, i };
            }

            map[nums[i]] = i;
        }

        return null;
    }

    public static int[] TwoSumCustomHash(int[] nums, int target)
    {
        const uint GoldenRatio = 0x9E3779B1;
        
        int n = nums.Length;
        int capacity = 1;
        // Turn capacity to power of two
        while (capacity < n * 2) capacity <<= 1;

        uint mask = (uint)(capacity - 1);
        Span<int> keys = capacity <= 1024 ? stackalloc int[capacity] : new int[capacity];
        Span<int> vals =  capacity <= 1024 ? stackalloc int[capacity] : new int[capacity];
        Span<bool> used = capacity <= 1024 ? stackalloc bool[capacity] : new bool[capacity];

        for (int i = 0; i < n; ++i)
        {
            int missing = target - nums[i];
            
            // Fibonacci hashing based on golden ratio
            uint hash = (uint)(missing * GoldenRatio);
            int index = (int)(hash & mask);

            while (used[index])
            {
                if (keys[index] == missing)
                {
                    return new[] { i, vals[index] };
                }
                
                // Linear probing
                index = (int)((uint)(index + 1) & mask);
            }

            uint hi = (uint)(nums[i] * GoldenRatio);
            int ii = (int)(hi & mask);

            while (used[ii])
            {
                ii = (int)((uint)(ii + 1) & mask);
            }

            keys[ii] = nums[i];
            vals[ii] = i;
            used[ii] = true;
        }
        
        return null;
    }
    
    public static int[] TwoSumCustomHashAoS(int[] nums, int target)
    {
        const uint GoldenRatio = 0x9E3779B1;
        
        int n = nums.Length;
        int capacity = 1;
        // Turn capacity to power of two
        while (capacity < n * 2) capacity <<= 1;

        uint mask = (uint)(capacity - 1);
        Span<Entry> table = capacity <= 1024 ? stackalloc Entry[capacity] : new Entry[capacity];

        for (int i = 0; i < n; ++i)
        {
            int missing = target - nums[i];
            
            // Fibonacci hashing based on golden ratio
            uint hash = (uint)(missing * GoldenRatio);
            int index = (int)(hash & mask);

            while (table[index].Used)
            {
                if (table[index].Key == missing)
                {
                    return new[] { i, table[index].Value };
                }
                
                // Linear probing
                index = (int)((uint)(index + 1) & mask);
            }

            uint hi = (uint)(nums[i] * GoldenRatio);
            int ii = (int)(hi & mask);

            while (table[ii].Used)
            {
                ii = (int)((uint)(ii + 1) & mask);
            }

            table[ii] = new Entry { Key = nums[i], Value = i, Used = true };
        }
        
        return null;
    }
    
    public static int[] TwoSumCustomHashAoSArrayPool(int[] nums, int target)
    {
        const uint GoldenRatio = 0x9E3779B1;
        
        int n = nums.Length;
        int capacity = 1;
        // Turn capacity to power of two
        while (capacity < n * 2) capacity <<= 1;
        uint mask = (uint)(capacity - 1);

        Entry[] rented = null;
        Span<Entry> table = capacity <= 1024 ? 
            stackalloc Entry[capacity] : 
            (rented = ArrayPool<Entry>.Shared.Rent(capacity));

        if (rented != null)
        {
            table.Slice(0, capacity).Clear();
        }

        try
        {
            for (int i = 0; i < n; ++i)
            {
                int missing = target - nums[i];

                // Fibonacci hashing based on golden ratio
                uint hash = (uint)(missing * GoldenRatio);
                int index = (int)(hash & mask);

                while (table[index].Used)
                {
                    if (table[index].Key == missing)
                    {
                        return new[] { i, table[index].Value };
                    }

                    // Linear probing
                    index = (int)((uint)(index + 1) & mask);
                }

                uint hi = (uint)(nums[i] * GoldenRatio);
                int ii = (int)(hi & mask);

                while (table[ii].Used)
                {
                    ii = (int)((uint)(ii + 1) & mask);
                }

                table[ii] = new Entry { Key = nums[i], Value = i, Used = true };
            }
        }
        finally
        {
            if (rented != null)
            {
                ArrayPool<Entry>.Shared.Return(rented);
            }
        }
        
        return null;
    }

    public static int[] TwoSumCustomHashSentinel(int[] nums, int target)
    {
        const uint GoldenRatio = 0x9E3779B1;
        const int Empty = int.MinValue;
        
        // stats = default;
        
        int n = nums.Length;
        int capacity = 1;
        while (capacity < (n * 10 / 7 + 1)) capacity <<= 1;

        uint mask = (uint)(capacity - 1);
        Span<int> keys = capacity <= 1024 ? stackalloc int[capacity] : new int[capacity];
        Span<int> vals =  capacity <= 1024 ? stackalloc int[capacity] : new int[capacity];
        
        keys.Fill(Empty);
        
        for (int i = 0; i < n; ++i)
        {
            int lookupProbeCount = 1;
            
            int missing = target - nums[i];
            uint hash = (uint)(missing * GoldenRatio);
            int index = (int)(hash & mask);

            while (keys[index] != Empty)
            {
                if (keys[index] == missing)
                {
                    // stats.LookupOperations++;
                    // stats.LookupProbes += lookupProbeCount;
                    //
                    // if (lookupProbeCount > stats.MaxLookupProbe)
                    //     stats.MaxLookupProbe = lookupProbeCount;
                    
                    return new[] { i, vals[index] };
                }
                
                lookupProbeCount++;
                index = (int)((uint)(index + 1) & mask);
            }
            // stats.LookupOperations++;
            // stats.LookupProbes += lookupProbeCount;
            //
            // if (lookupProbeCount > stats.MaxLookupProbe)
            //     stats.MaxLookupProbe = lookupProbeCount;

            int insertProbeCount = 1;
            uint hi = (uint)(nums[i] * GoldenRatio);
            int ii = (int)(hi & mask);
 
            while (keys[ii] != Empty)
            {
                insertProbeCount++;
                ii = (int)((uint)(ii + 1) & mask);
            }
            
            // stats.InsertOperations++;
            // stats.InsertProbes += insertProbeCount;
            //
            // if (insertProbeCount > stats.MaxInsertProbe)
            //     stats.MaxInsertProbe = insertProbeCount;
 
            keys[ii] = nums[i];
            vals[ii] = i;
        }
        
        return null;
    }
    
    public static int[] TwoSumCustomHashSentinelNoFib(int[] nums, int target)
    {
        const int Empty = int.MinValue;
        
        int n = nums.Length;
        int capacity = 1;
        while (capacity < (n * 10 / 7 + 1)) capacity <<= 1;

        uint mask = (uint)(capacity - 1);
        Span<int> keys = capacity <= 1024 ? stackalloc int[capacity] : new int[capacity];
        Span<int> vals =  capacity <= 1024 ? stackalloc int[capacity] : new int[capacity];
        
        keys.Fill(Empty);
        
        for (int i = 0; i < n; ++i)
        {
            int missing = target - nums[i];
            uint hash = (uint)(missing);
            int index = (int)(hash & mask);

            while (keys[index] != Empty)
            {
                if (keys[index] == missing)
                {
                    return new[] { i, vals[index] };
                }
                index = (int)((uint)(index + 1) & mask);
            }

            uint hi = (uint)(nums[i]);
            int ii = (int)(hi & mask);
 
            while (keys[ii] != Empty)
            {
                ii = (int)((uint)(ii + 1) & mask);
            }
 
            keys[ii] = nums[i];
            vals[ii] = i;
        }
        
        return null;
    }

    private struct Entry
    {
        public int Key;
        public int Value;
        public bool Used;
    }
    
    public struct HashStatistics
    {
        public long LookupProbes;
        public long InsertProbes;

        public int LookupOperations;
        public int InsertOperations;

        public int MaxLookupProbe;
        public int MaxInsertProbe;

        public override string ToString()
        {
            return $"""
                    Lookups
                      Operations : {LookupOperations}
                      Total Probes : {LookupProbes}
                      Avg Probes : {(double)LookupProbes / LookupOperations:F3}
                      Max Probe : {MaxLookupProbe}

                    Inserts
                      Operations : {InsertOperations}
                      Total Probes : {InsertProbes}
                      Avg Probes : {(double)InsertProbes / InsertOperations:F3}
                      Max Probe : {MaxInsertProbe}
                    """;
        }
    }
}