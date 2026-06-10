namespace Research.Console.Memory;

public interface ISimpleMeasurable { void Run(); }

public static class SimpleMeasureable
{
    public static void Measure<T>(string name, int iterations = 10000)
        where T : struct, ISimpleMeasurable
    {
        var op = new T();

        for (int i = 0; i < 100; ++i)
            op.Run();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long baselineBefore = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < iterations; i++) { /* nothing */ }
        long baselineBytes = GC.GetAllocatedBytesForCurrentThread() - baselineBefore;

        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < iterations; i++) op.Run();
        long after = GC.GetAllocatedBytesForCurrentThread();

        long net = (after - before) - baselineBytes;
        System.Console.WriteLine($"{name,-25} total: {net,8} bytes | per call: {(double)net / iterations,6:F2} bytes");
    }
}
