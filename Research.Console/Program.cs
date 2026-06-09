// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var s = new PointStruct { X = 1, Y = 2 };

object boxed = s;

var c = new PointClass { X = 1, Y = 2 };

Console.WriteLine("Hello World x2");

GC.KeepAlive(boxed);
GC.KeepAlive(c);

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

