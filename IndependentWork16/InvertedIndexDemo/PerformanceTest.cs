using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InvertedIndexDemo
{
    public static class PerformanceTest
    {
        private static readonly string[] SampleTopics =
        {
            "C# is a modern object-oriented programming language developed by Microsoft.",
            "Java is a popular programming language known for its portability and performance.",
            "Python is a high-level programming language with simple syntax.",
            "C# supports LINQ, async/await, and many modern programming paradigms.",
            "C#, Java, and Python are among the most popular programming languages today."
        };

        private static readonly string[] ExtraWords =
        {
            "algorithms", "collections", "dictionary", "hashset", "indexing",
            "database", "architecture", "compiler", "runtime", "memory",
            "optimization", "benchmark", "framework", "microservices", "cloud"
        };

        public static List<Document> GenerateDocuments(int count)
        {
            var random = new Random(42);
            var documents = new List<Document>(count);

            for (int i = 1; i <= count; i++)
            {
                string baseText = SampleTopics[(i - 1) % SampleTopics.Length];
                string extra1 = ExtraWords[random.Next(ExtraWords.Length)];
                string extra2 = ExtraWords[random.Next(ExtraWords.Length)];
                string extra3 = ExtraWords[random.Next(ExtraWords.Length)];

                documents.Add(new Document
                {
                    Id = i,
                    Title = $"Document #{i}",
                    Content = $"{baseText} Keywords: {extra1}, {extra2}, {extra3}."
                });
            }

            return documents;
        }

        public static void CompareSearchMethods(List<Document> documents, string searchWord)
        {
            var index = new InvertedIndex();
            foreach (var doc in documents)
            {
                index.AddDocument(doc);
            }

            var normalizedWarmup = searchWord.ToLowerInvariant();
            _ = documents.Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWarmup)).ToList();
            _ = index.SearchSingleWord(searchWord);

            var sw = new Stopwatch();

            sw.Start();
            var normalizedWord = searchWord.ToLowerInvariant();
            var linearResults = documents
                .Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord))
                .ToList();
            sw.Stop();
            long linearTicks = sw.ElapsedTicks;
            double linearMs = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            var indexResults = index.SearchSingleWord(searchWord);
            sw.Stop();
            long indexTicks = sw.ElapsedTicks;
            double indexMs = sw.Elapsed.TotalMilliseconds;

            double speedup = indexTicks > 0 ? (double)linearTicks / indexTicks : 0;

            Console.WriteLine($"Документів: {documents.Count} | Слово: '{searchWord}'");
            Console.WriteLine($"  Linear search : {linearTicks,8} ticks ({linearMs:F4} ms) | Знайдено: {linearResults.Count}");
            Console.WriteLine($"  Inverted Index: {indexTicks,8} ticks ({indexMs:F4} ms) | Знайдено: {indexResults.Count}");
            Console.WriteLine($"  Прискорення   : {speedup:F1}x\n");
        }
    }
}