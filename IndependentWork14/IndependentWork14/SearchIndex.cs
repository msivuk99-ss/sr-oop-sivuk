using System;
using System.Collections.Generic;

namespace IndependentWork14
{
    public class SearchIndex
    {
        private readonly Dictionary<string, HashSet<int>> _invertedIndex;
        private static readonly char[] Separators =
        {
            ' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t', '(', ')', '"'
        };

        public SearchIndex()
        {
            _invertedIndex = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddDocument(int docId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            string[] words = content.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (!_invertedIndex.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _invertedIndex[word] = docIds;
                }

                docIds.Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return new HashSet<int>();
            }

            if (_invertedIndex.TryGetValue(word, out var docIds))
            {
                return new HashSet<int>(docIds);
            }

            return new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words == null || words.Length == 0)
            {
                return new HashSet<int>();
            }

            if (!_invertedIndex.TryGetValue(words[0], out var firstSet))
            {
                return new HashSet<int>();
            }

            var result = new HashSet<int>(firstSet);

            for (int i = 1; i < words.Length; i++)
            {
                if (!_invertedIndex.TryGetValue(words[i], out var nextSet))
                {
                    return new HashSet<int>();
                }

                result.IntersectWith(nextSet);

                if (result.Count == 0)
                {
                    break;
                }
            }

            return result;
        }
    }
}