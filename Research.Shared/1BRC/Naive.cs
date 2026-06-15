using System.Globalization;
using System.Text;

namespace Research.Shared._1BRC;

public class Naive(string path) : IBRC
{
    private string _path = path;

    public Dictionary<string, StationResult> Collect()
    {
        return
            File.ReadLines(path, new UTF8Encoding())
                .Select(l => l.Split(';'))
                .GroupBy(parts => parts[0])
                .ToDictionary(
                    g => g.Key,
                    g => g.Aggregate(
                        new MeasurementAggregator(),
                        (agg, parts) =>
                        {
                            var value = double.Parse(parts[1], CultureInfo.InvariantCulture);
                            agg.Min = Math.Min(agg.Min, value);
                            agg.Max = Math.Max(agg.Max, value);
                            agg.Sum += value;
                            agg.Count++;
                            return agg;
                        },
                        agg => new StationResult(
                            agg.Min,
                            agg.Sum / agg.Count,
                            agg.Max)));
    }

    private class MeasurementAggregator
    {
        public double Min = double.PositiveInfinity;
        public double Max = double.NegativeInfinity;
        public double Sum;
        public long Count;
    }
}