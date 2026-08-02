using System.Diagnostics;
using System.Runtime.InteropServices;
using Xunit.Abstractions;

namespace Research.Labs.Memory;

/*
 * The classic model of a virtual address space splits it into three
 * regions: Stack, Heap, and Code. On a modern operating system and
 * a managed programming language this still exist as a *contract*,
 * but its not something you can verify from an address alone.
 * What lives where or what memory address belongs in what category
 * is not something you can query.
 * To us, it's just a memory address.
 * There is nothing that coordinates the heap and stack placement in
 * relation to each other at all. Each allocator independently asks
 * Windows for a chunk of unmapped virtual space and Windows will pick a spot.
 */
public class AddressSpaceTests
{
    private readonly ITestOutputHelper _out;
    public AddressSpaceTests(ITestOutputHelper output) => _out = output;
    
    [Fact]
    public unsafe void ComparingRegions()
    {
        void PrintRegion(string label, IntPtr addr)
        {
            VirtualQuery(addr, out var info, (uint)Marshal.SizeOf<MemoryBasicInformation>());
            _out.WriteLine($"{label} -> State=0x{info.State:X}  Protect=0x{info.Protect:X}  Type=0x{info.Type:X}  Size={(long)info.RegionSize}");
        }
        
        int stackVar = 42;
        var heapObj = new byte[16];
        var codePtr = typeof(AddressSpaceTests).GetMethod(nameof(ComparingRegions))!
            .MethodHandle.GetFunctionPointer();

        fixed (byte* heapPtr = heapObj)
        {
            PrintRegion("stack", (IntPtr)(&stackVar));
            PrintRegion("heap ", (IntPtr)heapPtr);
            PrintRegion("code ", codePtr);
        }
    }
    
    [Fact]
    public void WalkingAddressSpace()
    {
        IntPtr address = IntPtr.Zero;
        while (true)
        {
            IntPtr result = VirtualQuery(
                address, 
                out var info,
                (uint)Marshal.SizeOf<MemoryBasicInformation>());
            if (result == IntPtr.Zero)
            {
                break;
            }
            
            _out.WriteLine(
                $"{address.ToInt64():X16} " +
                $"{info.RegionSize / 1024} KB " +
                $"{info.TypeToString()} " +
                $"{info.StateToString()} " +
                $"{info.Protect}"
            );
            
            ulong next = (ulong)address.ToInt64() + info.RegionSize;
            if (next <= (ulong)address.ToInt64())
            {
                break;
            }

            address = (IntPtr)next;
        }
    }
    
    [DllImport("kernel32.dll")]
    public static extern IntPtr VirtualQuery(IntPtr lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public ushort PartitionId;
        private ushort __alignment;
        public nuint RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;

        public string TypeToString()
        {
            switch (Type)
            {
                case 0x1000000:
                {
                    return "MEM_IMAGE";
                }
                case 0x40000:
                {
                    return "MEM_MAPPED";
                }
                case 0x20000:
                {
                    return "MEM_PRIVATE";
                }
                default:
                {
                    return "UNKNOWN_TYPE";
                }
            }
        }

        public string StateToString()
        {
            switch (State)
            {
                case 0x1000:
                {
                    return "MEM_COMMIT";
                }
                case 0x10000:
                {
                    return "MEM_FREE";
                }
                case 0x2000:
                {
                    return "MEM_RESERVE";
                }
                default:
                {
                    return "UNKNOWN_STATE";
                }
            }
        }
    }
}