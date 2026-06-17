using System.Runtime.InteropServices;

namespace Research.Console.Scratch;

public static class BasicPointer
{
    public static unsafe void RunBasic()
    {
        // Using IntPtr
        {
            IntPtr p1 = Marshal.AllocHGlobal(sizeof(int));
            if (p1 == IntPtr.Zero)
            {
                // Failed to alloc
            }
            Marshal.WriteInt32(p1, 25);
            
            PrintIntPtr(p1);
            
            Marshal.FreeHGlobal(p1);
            p1 = IntPtr.Zero;
        }

        // Using int*
        {
            int* p2 = (int*)Marshal.AllocHGlobal(sizeof(int));
            if (p2 == null)
            {
                // Failed to alloc
            }
            *p2 = 35;
            
            PrintPointer(p2);
            PrintRef(ref *p2);
            
            Marshal.FreeHGlobal((IntPtr)p2);
            p2 = null;
        }

        // Using stack value
        {
            int stackValue = 55;
            int* p3 = &stackValue;
            
            PrintPointer(p3);
            PrintRef(ref *p3);
        }
    }

    public static unsafe void RunStructAndClass()
    {
        // TODO 1: Alloc struct with Marshal
        // TODO 2: Alloc Class with GCHandle
        // TODO 3: Fixed keyword for stack allocated struct
        // TODO 4: Alloc struct with GCHandle
    }

    private static void PrintIntPtr(IntPtr p)
    {
        int value = Marshal.ReadInt32(p);
        System.Console.WriteLine("Printing IntPtr value: " + value);
    }

    private static unsafe void PrintPointer(int* value)
    {
        System.Console.WriteLine("Printing int* value: " + *value);
    }
    
    private static void PrintRef(ref int value)
    {
        System.Console.WriteLine("Printing ref int value: " + value);
    }

    private struct PointStruct
    {
        public int x;
        public int y;
    }

    private class PointClass
    {
        public int x;
        public int y;
    }
}