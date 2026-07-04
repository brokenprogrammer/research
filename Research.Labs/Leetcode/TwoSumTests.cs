using Research.Shared.Leetcode;

namespace Research.Labs.Leetcode;

public class TwoSumTests
{
    [Theory]
    [InlineData(new int[] {2,7,11,15}, 9, new int[] {0, 1})]
    [InlineData(new int[] {-1,-2,-3,-4,-5}, -8, new int[] {2, 4})]
    [InlineData(new int[] {3,2,4}, 6, new int[] {1, 2})]
    public void TwoSumTest(int[] nums, int target, int[] expected)
    {
        var result = TwoSum.TwoSumDictionary(nums, target);
        Assert.Contains(expected[0], result);
        Assert.Contains(expected[1], result);
    }
    
    [Theory]
    [InlineData(new int[] {2,7,11,15}, 9, new int[] {0, 1})]
    [InlineData(new int[] {-1,-2,-3,-4,-5}, -8, new int[] {2, 4})]
    [InlineData(new int[] {3,2,4}, 6, new int[] {1, 2})]
    public void TwoSumCustomHashTest(int[] nums, int target, int[] expected)
    {
        var result = TwoSum.TwoSumCustomHash(nums, target);
        Assert.Contains(expected[0], result);
        Assert.Contains(expected[1], result);
    }
}