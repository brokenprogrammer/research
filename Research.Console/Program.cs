// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Research.Console.Memory;
using Research.Console.Scratch;
using Research.Shared._1BRC;
using Research.Shared._1BRC.Iteration1;
using Research.Shared.Timing;

Console.WriteLine($"PID: {Environment.ProcessId}");

Console.OutputEncoding = Encoding.UTF8;

BasicPointer.RunBasic();

// var stations = new Dictionary<string, StationResult>();
// var naiveRun = new Naive("C:\\Users\\mende\\Documents\\Github\\research\\Data\\measurements-20.txt");
//
// var brc = new BRC("C:\\Users\\mende\\Documents\\Github\\research\\Data\\measurements-20.txt");
// using (new SimpleTimer())
// {
//     // stations = naiveRun.Collect();
//     brc.Collect();
// }
//
// var measurements = new SortedDictionary<string, StationResult>(stations);
// Console.WriteLine(
//     "{" + string.Join(", ", measurements.Select(kv => $"{kv.Key}={kv.Value}")) + "}");














// var lines = File.ReadAllLines("C:\\Users\\mende\\Documents\\Github\\research\\Data\\measurements-20.txt", new UTF8Encoding());
// foreach (var l in lines)
// {
//     var parts = l.Split(';');
//     var name = parts[0];
//     var value = Double.Parse(parts[1], CultureInfo.InvariantCulture);
//     
//     if (stations.TryGetValue(name, out var entry))
//     {
//         entry.Min = entry.Min < value ?  entry.Min : value; 
//         entry.Max = entry.Max > value ?  entry.Max : value; 
//         entry.Sum += value; 
//         entry.Count = entry.Count + 1; 
//     }
//     else
//     {
//         stations[name] = new StationEntry()
//         {
//             Min = value,
//             Max = value,
//             Sum = value,
//             Count = 1
//         };
//     }
// }

// record ResultRow(double Min, double Mean, double Max)
// {
//     public override string ToString() =>
//         $"{Round(Min)}/{Round(Mean)}/{Round(Max)}";
//     
//     private static double Round(double value) =>
//         Math.Round(value * 10.0, MidpointRounding.AwayFromZero) / 10.0;
// }
//
// class MeasurementAggregator
// {
//     public double Min = double.PositiveInfinity;
//     public double Max = double.NegativeInfinity;
//     public double Sum;
//     public long Count;
// }

// var sortedStations = new SortedDictionary<string, StationEntry>(stations);
//
// var head = "{";
// foreach (var kvp in sortedStations)
// {
//     var e = kvp.Value;
//     var avg = e.Sum / e.Count;
//     Console.Write($"{head}{kvp.Key}={e.Min:F1}/{avg:F1}/{e.Max:F1}");
//     head = ", ";
// }
// Console.WriteLine("}");

// public class StationEntry
// {
//     public double Min { get; set; } 
//     public double Max { get; set; }
//     public double Sum { get; set; }
//     public int Count { get; set; }
// }

// SimpleMeasureable.Measure<MeasureStruct>("Struct");
// SimpleMeasureable.Measure<MeasureBoxedStruct>("Boxed Struct");
// SimpleMeasureable.Measure<MeasureClass>("Class");
// SimpleMeasureable.Measure<MeasureBoxedClass>("Boxed Class");
//
// struct MeasureStruct : ISimpleMeasurable
// {
//     [MethodImpl(MethodImplOptions.NoInlining)]
//     public void Run()
//     {
//         var s = new PointStruct { X = 1, Y = 2 };
//     }
// }
//
// struct MeasureBoxedStruct : ISimpleMeasurable
// {
//     [MethodImpl(MethodImplOptions.NoInlining)]
//     public void Run()
//     {
//         var s = new PointStruct { X = 1, Y = 2 }; 
//         object o = s; 
//         GC.KeepAlive(o);
//     }
// }
//
// struct MeasureClass : ISimpleMeasurable
// {
//     [MethodImpl(MethodImplOptions.NoInlining)]
//     public void Run()
//     {
//         var c = new PointClass { X = 1, Y = 2 };
//     }
// }
//
// struct MeasureBoxedClass : ISimpleMeasurable
// {
//     [MethodImpl(MethodImplOptions.NoInlining)]
//     public void Run()
//     {
//         var c = new PointClass { X = 1, Y = 2 };
//         object o = c;
//         GC.KeepAlive(o);
//     }
// }
//
// struct PointStruct
// {
//     public int X;
//     public int Y;
// }
//
// class PointClass
// {
//     public int X;
//     public int Y;
// }
//
