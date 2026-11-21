using System;
using System.Collections.Generic;
using System.Linq;

namespace InvertedIndexDemo
{
    public class InvertedIndex
    {
        private readonly Dictionary<string, HashSet<int>> _index;
        private readonly Dictionary<int, Document> _documents;

        public InvertedIndex()
        {
            _index = new Dictionary<string, HashSet<int>>();
            _documents = new Dictionary<int, Document>();
        }

        public int DocumentCount => _documents.Count;
        public int UniqueWords => _index.Keys.Count;

        public void AddDocument(Document doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            _documents[doc.Id] = doc;

            foreach (var word in TextProcessor.Tokenize(doc.Content))
            {
                if (!_index.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _index[word] = docIds;
                }

                docIds.Add(doc.Id);
            }
        }

        public List<Document> SearchSingleWord(string word)
        {
            var normalizedWord = TextProcessor.Tokenize(word).FirstOrDefault();
            if (string.IsNullOrEmpty(normalizedWord))
            {
                return new List<Document>();
            }

            if (_index.TryGetValue(normalizedWord, out var docIds))
            {
                return docIds.Select(id => _documents[id]).ToList();
            }

            return new List<Document>();
        }

        public List<Document> SearchMultipleWords(params string[] words)
        {
            if (words == null || words.Length == 0)
            {
                return new List<Document>();
            }

            var normalizedWords = words
                .SelectMany(TextProcessor.Tokenize)
                .Distinct()
                .ToList();

            if (normalizedWords.Count == 0)
            {
                return new List<Document>();
            }

            if (!_index.TryGetValue(normalizedWords[0], out var firstSet))
            {
                return new List<Document>();
            }

            var resultIds = new HashSet<int>(firstSet);

            for (int i = 1; i < normalizedWords.Count; i++)
            {
                if (!_index.TryGetValue(normalizedWords[i], out var nextSet))
                {
                    return new List<Document>();
                }

                resultIds.IntersectWith(nextSet);

                if (resultIds.Count == 0)
                {
                    break;
                }
            }

            return resultIds.Select(id => _documents[id]).ToList();
        }

        public List<Document> SearchAnyWord(params string[] words)
        {
            if (words == null || words.Length == 0)
            {
                return new List<Document>();
            }

            var normalizedWords = words
                .SelectMany(TextProcessor.Tokenize)
                .Distinct();

            var resultIds = new HashSet<int>();

            foreach (var word in normalizedWords)
            {
                if (_index.TryGetValue(word, out var docIds))
                {
                    resultIds.UnionWith(docIds);
                }
            }

            return resultIds.Select(id => _documents[id]).ToList();
        }
    }
}