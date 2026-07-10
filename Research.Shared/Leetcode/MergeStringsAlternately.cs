using System.Text;

namespace Research.Shared.Leetcode;

public static class MergeStringsAlternately
{
    public static string MergeAlternatelyStringBuilder(string word1, string word2)
    {
        var sb = new StringBuilder();
        int i = 0;
        int j = 0;

        while (i < word1.Length && j < word2.Length)
        {
            sb.Append(word1[i++]);
            sb.Append(word2[j++]);
        }
        
        sb.Append(word1.Substring(i));
        sb.Append(word2.Substring(j));
        
        return sb.ToString();
    }
    
    
    public static string MergeAlternately(string word1, string word2)
    {
        var length = word1.Length + word2.Length;
        var minLength = Math.Min(word1.Length, word2.Length);
        
        return string.Create(length, (word1, word2, minLength), (chars, state) =>
        {
            var (w1, w2, min) = state;
            ReadOnlySpan<char> s1 = w1;
            ReadOnlySpan<char> s2 = w2;
            var position = 0;

            for (int i = 0; i < min; ++i)
            {
                chars[position++] = s1[i];
                chars[position++] = s2[i];
            }

            if (s1.Length > min)
            {
                s1.Slice(min).CopyTo(chars.Slice(position));
            }
            else if (s2.Length > min)
            {
                s2.Slice(min).CopyTo(chars.Slice(position));
            }
        });
    }
}