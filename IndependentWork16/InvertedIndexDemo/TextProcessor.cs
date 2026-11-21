using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedIndexDemo
{
    public static class TextProcessor
    {
        private static readonly HashSet<string> StopWords = new HashSet<string>
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        public static IEnumerable<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Enumerable.Empty<string>();
            }

            return Regex.Matches(text.ToLowerInvariant(), @"\w+#*")
                        .Select(m => m.Value);
        }

        public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
        {
            return words.Where(w => !StopWords.Contains(w));
        }
    }
}