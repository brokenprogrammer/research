using System.Runtime.InteropServices;

namespace Research.Shared;

struct Unpacked
{
    // 6 byte sum and 12 byte actual
    public byte A; // 1 byte
    public int  B; // 4 byte - 3 bytes padding should be added after A
    public byte C; // 1 byte - 3 bytes padding added to align struct
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct Packed
{
    // 6 bytes
    public byte A; // 1 byte
    public int  B; // 4 bytes - No padding
    public byte C; // 1 byte
}

[StructLayout(LayoutKind.Explicit)]
struct ExplicitLayout
{
    // 4 bytes, should create C union style struct.
    [FieldOffset(0)]
    public int A;

    [FieldOffset(0)]
    public float B;
}

public struct ParentStruct
{
    public byte A; // 1
    public NestedStruct B; // 6
    public byte C; // 7
}

public struct NestedStruct
{
    public byte A;
    public int  B;
    public byte C;
}