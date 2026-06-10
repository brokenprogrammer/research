// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using Research.Console.Memory;

Console.WriteLine("Hello, World!");

Console.WriteLine($"PID: {Environment.ProcessId}");
Console.WriteLine("Press Enter to start...");
Console.ReadLine();

SimpleMeasureable.Measure<MeasureStruct>("Struct");
SimpleMeasureable.Measure<MeasureBoxedStruct>("Boxed Struct");
SimpleMeasureable.Measure<MeasureClass>("Class");
SimpleMeasureable.Measure<MeasureBoxedClass>("Boxed Class");

struct MeasureStruct : ISimpleMeasurable
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run()
    {
        var s = new PointStruct { X = 1, Y = 2 };
    }
}

struct MeasureBoxedStruct : ISimpleMeasurable
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run()
    {
        var s = new PointStruct { X = 1, Y = 2 }; 
        object o = s; 
        GC.KeepAlive(o);
    }
}

struct MeasureClass : ISimpleMeasurable
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run()
    {
        var c = new PointClass { X = 1, Y = 2 };
    }
}

struct MeasureBoxedClass : ISimpleMeasurable
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run()
    {
        var c = new PointClass { X = 1, Y = 2 };
        object o = c;
        GC.KeepAlive(o);
    }
}

struct PointStruct
{
    public int X;
    public int Y;
}

class PointClass
{
    public int X;
    public int Y;
}

