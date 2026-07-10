using System.Globalization;
using Research.Shared.Leetcode;

namespace Research.Labs.Leetcode;

public class MergeStringsAlternatelyTests
{
    [Theory]
    [InlineData("abc", "pqr", "apbqcr")]
    [InlineData("ab", "pqrs", "apbqrs")]
    [InlineData("abcd", "pq", "apbqcd")]
    public void MergeAlternatelyStringBuilder(string word1, string word2, string result)
    {
        var merged = MergeStringsAlternately.MergeAlternatelyStringBuilder(word1, word2);
        
        Assert.Equal(result, merged);
    }
    
    [Theory]
    [InlineData("abc", "pqr", "apbqcr")]
    [InlineData("ab", "pqrs", "apbqrs")]
    [InlineData("abcd", "pq", "apbqcd")]
    public void MergeAlternately(string word1, string word2, string result)
    {
        var merged = MergeStringsAlternately.MergeAlternately(word1, word2);
        
        Assert.Equal(result, merged);
    }
}