using System.Diagnostics;

namespace Research.Shared.Timing;

public class SimpleTimer : IDisposable
{
    private long _startTime;
    
    public SimpleTimer()
    {
        _startTime = Stopwatch.GetTimestamp();
    }

    public void Dispose()
    {
        var diff = Stopwatch.GetElapsedTime(_startTime);
        Console.WriteLine($"Runtime: {diff}");
    }
}