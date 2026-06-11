using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Research.Shared;
using ObjectLayoutInspector;

namespace Research.Labs;

public class SimpleSizeTests
{
    [Fact]
    public void SizeOfUnpackedIncludesPadding()
    {
        // Struct is unmanaged hence we have to use Marshal
        Assert.Equal(12, Marshal.SizeOf<Unpacked>());
    }

    [Fact]
    public void SizeOfUnpackedClassHasDifferentSizeBecauseReorderingOfLayoutAndClassOverhead()
    {
        ObjectLayoutInspector.TypeLayout layout = ObjectLayoutInspector.TypeLayout.GetLayout<UnpackedClass>();
        
        Assert.Equal(24, layout.FullSize);
        Assert.Equal(16, layout.Overhead);
        Assert.Equal(8, layout.Size);
        Assert.Equal(2, layout.Paddings);
    }
    
    [Fact]
    public void SizeOfPackedRemovesPadding()
    {
        Assert.Equal(6, Marshal.SizeOf<Packed>());
    }
    
    [Fact]
    public void SizeOfPackedClassIsSameAsUnpackedBecausePadding()
    {
        ObjectLayoutInspector.TypeLayout layout = ObjectLayoutInspector.TypeLayout.GetLayout<PackedClass>();
        
        Assert.Equal(24, layout.FullSize);
        Assert.Equal(16, layout.Overhead);
        Assert.Equal(8, layout.Size);
        Assert.Equal(2, layout.Paddings);
    }
    
    [Fact]
    public void SizeOfExplicitWithOverlappingFields()
    {
        Assert.Equal(4, Marshal.SizeOf<Explicit>());
    }
    
    [Fact]
    public void SizeOfParentStructIncludesNestedSize()
    {
        Assert.Equal(12, Marshal.SizeOf<OuterSequential>());
    }

    [Theory]
    [InlineData(typeof(OuterSequential),        12)] // Inner(8) + byte(1) + 3 padding
    [InlineData(typeof(OuterPacked),             9)] // Inner(8) + byte(1). No padding
    [InlineData(typeof(OuterExplicit),          12)] // Inner(8), Byte(1) + 3 padding
    [InlineData(typeof(OuterExplicitByteOnly),   3)] // Inner(2), Byte(1)
    public void SizeOfNestedStructReflectsParentLayout(Type t, int expected)
    {
        Assert.Equal(expected, Marshal.SizeOf(t));
    }
    
    [Fact]
    public void Inner_Padding_PropagatesIntoParent_WhenSequential()
    {
        // The parent inherits the inner struct's alignment requirements
        // Inner has int (4-byte aligned), so parent must also be 4-byte aligned
        Assert.Equal(8, Marshal.SizeOf<Inner>());
        Assert.True(Marshal.SizeOf<OuterSequential>() % 4 == 0);
    }

    [Fact]
    public void WrapperSizeDependsOnTypeArgument()
    {
        var x = new Wrapper<int>();
        Assert.Equal(8,  Marshal.SizeOf(x));    // int(4) + Tag(4)
        
        var y = new Wrapper<double>();
        Assert.Equal(16, Marshal.SizeOf(y)); // double(8) + Tag(4), padding 4
        
        var z = new Wrapper<byte>();
        Assert.Equal(8,  Marshal.SizeOf(z));   // byte(1) + 3 pad + Tag(4)
    }
    
    [Fact]
    public void WrapperWithReferenceTypeSizeIsPointerPlusTag()
    {
        var wrapper = new Wrapper<string>();
        Assert.Equal(16, Marshal.SizeOf(wrapper)); // ptr(8) + Tag(4) + 4 pad
    }
    
    [Fact]
    public void PairMixedTypesSizeReflectsLargestAlignmentRequirement()
    {
        var x = new Pair<byte, double>(); // double forces 8-byte alignment
        Assert.Equal(16, Marshal.SizeOf(x));
        
        var xx = new Pair<double, byte>();
        Assert.Equal(16, Marshal.SizeOf(xx));
        
        var y = new Pair<int, int>();
        Assert.Equal(8,  Marshal.SizeOf(y));
        
        var z = new Pair<byte, byte>();
        Assert.Equal(2,  Marshal.SizeOf(z));
    }
    
}