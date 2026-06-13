using System.Globalization;
using System.Text;

namespace Research.Shared._1BRC;

public class Naive(string path)
{
    private string _path = path;

    public Dictionary<string, ResultRow> Collect()
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
                        agg => new ResultRow(
                            agg.Min,
                            agg.Sum / agg.Count,
                            agg.Max)));
    }
    
    public record ResultRow(double Min, double Mean, double Max)
    {
        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "{0:F1}/{1:F1}/{2:F1}", Round(Min), Round(Mean), Round(Max));
    
        private static double Round(double value) =>
            Math.Round(value * 10.0, MidpointRounding.ToEven) / 10.0;
    }

    private class MeasurementAggregator
    {
        public double Min = double.PositiveInfinity;
        public double Max = double.NegativeInfinity;
        public double Sum;
        public long Count;
    }
    
    // public Dictionary<string, StationEntry> Perform()
    // {
    //     var stations = new Dictionary<string, StationEntry>();
    //     var lines = File.ReadAllLines(_path, new UTF8Encoding());
    //     foreach (var l in lines)
    //     {
    //         var parts = l.Split(';');
    //         var name = parts[0];
    //         var value = Double.Parse(parts[1], CultureInfo.InvariantCulture);
    //
    //         if (stations.TryGetValue(name, out var entry))
    //         {
    //             entry.Min = entry.Min < value ? entry.Min : value;
    //             entry.Max = entry.Max > value ? entry.Max : value;
    //             entry.Sum += value;
    //             entry.Count = entry.Count + 1;
    //         }
    //         else
    //         {
    //             stations[name] = new StationEntry()
    //             {
    //                 Min = value,
    //                 Max = value,
    //                 Sum = value,
    //                 Count = 1
    //             };
    //         }
    //     }
    //
    //     return stations;
    // }
    
    // public class StationEntry
    // {
    //     public double Min { get; set; } 
    //     public double Max { get; set; }
    //     public double Sum { get; set; }
    //     public int Count { get; set; }
    // }
}