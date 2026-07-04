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
    
    private struct Entry
    {
        public int Key;
        public int Value;
        public bool Used;
    }
}