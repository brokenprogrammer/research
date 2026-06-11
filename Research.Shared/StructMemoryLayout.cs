using System.Runtime.InteropServices;

namespace Research.Shared;

public struct Unpacked
{
    // 6 byte sum and 12 byte actual
    public byte A; // 1 byte
    public int  B; // 4 byte - 3 bytes padding should be added after A
    public byte C; // 1 byte - 3 bytes padding added to align struct
}

public class UnpackedClass
{
    // Object header 4/8 bytes
    // Method table pointer 4/8 bytes
    
    // 6 byte sum and 12 byte actual
    public byte A; // 1 byte
    public int  B; // 4 byte - 3 bytes padding should be added after A
    public byte C; // 1 byte - 3 bytes padding added to align struct
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Packed
{
    // 6 bytes
    public byte A; // 1 byte
    public int  B; // 4 bytes - No padding
    public byte C; // 1 byte
}

// Class will add padding in the end so this packed version will have the same size as unpacked version.
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PackedClass
{
    public byte A;
    public int  B;
    public byte C;
}

[StructLayout(LayoutKind.Explicit)]
public struct Explicit
{
    // 4 bytes, should create C union style struct.
    [FieldOffset(0)]
    public int A;

    [FieldOffset(0)]
    public float B;
}

public struct Inner
{
    // 8 bytes, padding after Byte A.
    public byte A;
    public int B;
}

public struct InnerByteOnly
{
    public byte A;
    public byte B; // no int — max alignment is 1 byte
}

[StructLayout(LayoutKind.Sequential)]
public struct OuterSequential { public Inner Nested; public byte C; }

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OuterPacked { public Inner Nested; public byte C; }

[StructLayout(LayoutKind.Explicit)]
public struct OuterExplicit
{
    [FieldOffset(0)] public Inner Nested;
    [FieldOffset(8)] public byte C;
}

[StructLayout(LayoutKind.Explicit)]
public struct OuterExplicitByteOnly
{
    // Total 3 bytes, no padding needed.
    [FieldOffset(0)] public InnerByteOnly Nested; // 2 bytes, alignment 1
    [FieldOffset(2)] public byte C;               // 1 byte
}

public struct Wrapper<T>
{
    public T Value;
    public int Tag;
}

public struct Pair<T1, T2>
{
    public T1 First;
    public T2 Second;
}