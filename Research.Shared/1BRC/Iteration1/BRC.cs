using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace Research.Shared._1BRC.Iteration1;

public class BRC : IBRC
{
    private string _path;
    private int _numberOfCores = 0;

    public BRC(string path)
    {
        _path = path;
        _numberOfCores = Environment.ProcessorCount;
    }

    public Dictionary<string, StationResult> Collect()
    {
        var allText = File.ReadAllText(_path, new UTF8Encoding());
        var results =
            SplitFileIntoChunks(allText, _numberOfCores)
                .AsParallel()
                .Select(i =>
                {
                    var substring = allText.Substring((int)i.Start, (int)i.End - (int)i.Start);
                    var local = new Dictionary<string, MeasurementAggregator>();

                    foreach (var line in substring.Split('\n'))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = line.Split(';');
                        if (parts.Length < 2) continue;

                        var name = parts[0];
                        var value = double.Parse(parts[1], CultureInfo.InvariantCulture);

                        if (!local.TryGetValue(name, out var agg))
                        {
                            agg = new MeasurementAggregator();
                            local[name] = agg;
                        }

                        agg.Min = Math.Min(agg.Min, value);
                        agg.Max = Math.Max(agg.Max, value);
                        agg.Sum += value;
                        agg.Count++;
                    }

                    return local;
                })
                .Aggregate(
                    new Dictionary<string, MeasurementAggregator>(),
                    (global, local) =>
                    {
                        foreach (var (name, localAgg) in local)
                        {
                            if (!global.TryGetValue(name, out var globalAgg))
                            {
                                globalAgg = new MeasurementAggregator();
                                global[name] = globalAgg;
                            }

                            globalAgg.Min = Math.Min(globalAgg.Min, localAgg.Min);
                            globalAgg.Max = Math.Max(globalAgg.Max, localAgg.Max);
                            globalAgg.Sum += localAgg.Sum;
                            globalAgg.Count += localAgg.Count;
                        }

                        return global;
                    });

        return results.ToDictionary(
            kvp => kvp.Key,
            kvp => new StationResult(
                kvp.Value.Min,
                kvp.Value.Sum / kvp.Value.Count,
                kvp.Value.Max));
    }

    private List<FileChunk> SplitFileIntoChunks(string text, int numberOfChunks)
    {
        var chunks = new List<FileChunk>();
        long fileSize = text.Length;
        long chunkSize = fileSize / numberOfChunks;
        long current = 0;

        for (int i = 0; i < numberOfChunks; i++)
        {
            long start = current;
            long end = i == numberOfChunks - 1 ? text.Length : current + chunkSize;

            if (current >= fileSize)
            {
                break;
            }

            if (i < numberOfChunks - 1)
            {
                end = findEndOfLine(text, end, fileSize);
            }

            chunks.Add(new FileChunk(start, end));
            current = end;
        }

        return chunks;
    }

    private long findEndOfLine(string text, long end, long fileSize)
    {
        int newlineIndex = text.IndexOf('\n', (int)end);

        if (newlineIndex == -1)
        {
            return fileSize;
        }

        return newlineIndex + 1;
    }

    struct FileChunk
    {
        public FileChunk(long start, long end)
        {
            Start = start;
            End = end;
        }

        public long Start;
        public long End;
    }

    private class MeasurementAggregator
    {
        public double Min = double.PositiveInfinity;
        public double Max = double.NegativeInfinity;
        public double Sum;
        public long Count;
    }
}