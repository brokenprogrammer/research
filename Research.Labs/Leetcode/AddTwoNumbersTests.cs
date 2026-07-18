using Research.Shared.Leetcode;

namespace Research.Labs.Leetcode;

public class AddTwoNumbersTests
{
    [Theory]
    [InlineData(new[] { 2, 4, 3 }, new[] { 5,6,4 }, new[] { 7, 0, 8 })]
    [InlineData(new[] { 0 }, new[] { 0 }, new[] { 0 })]
    [InlineData(new[] { 9,9,9,9,9,9,9 }, new[] { 9,9,9,9 }, new[] { 8,9,9,9,0,0,0,1 })]
    [InlineData(new[] { 9 }, new[] { 1,9,9,9,9,9,9,9,9,9 }, new[] { 0,0,0,0,0,0,0,0,0,0,1 })]
    [InlineData(new[] { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 }, new[] { 5,6,4 }, new[] { 6,6,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 })]
    public void AddTwoNumbersTest(int[] l1, int[] l2,  int[] res)
    {

        var l1List = AddTwoNumbers.ListNode.FromArray(l1);
        var l2List = AddTwoNumbers.ListNode.FromArray(l2);
        
        var result = AddTwoNumbers.Add(l1List, l2List);

        Assert.Equal(result.ToArray(), res);
    }
}