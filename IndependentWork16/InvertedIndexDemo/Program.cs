using System;
using System.Collections.Generic;

namespace InvertedIndexDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var index = new InvertedIndex();
            LoadDocuments(index);

            Console.WriteLine($"Loaded {index.DocumentCount} documents");
            Console.WriteLine($"Index contains {index.UniqueWords} unique words\n");

            Console.WriteLine("--- Single Word Search: 'c#' ---");
            foreach (var doc in index.SearchSingleWord("c#"))
            {
                Console.WriteLine(doc);
            }

            Console.WriteLine("\n--- Multiple Words Search (AND): 'programming language' ---");
            foreach (var doc in index.SearchMultipleWords("programming", "language"))
            {
                Console.WriteLine(doc);
            }

            Console.WriteLine("\n--- Any Word Search (OR): 'python linq' ---");
            foreach (var doc in index.SearchAnyWord("python", "linq"))
            {
                Console.WriteLine(doc);
            }

            var sampleTokens = TextProcessor.Tokenize("C# is a modern language and Java is popular");
            var filteredTokens = TextProcessor.RemoveStopWords(sampleTokens);
            Console.WriteLine("\n--- Stop-Words Removal Demo ---");
            Console.WriteLine("Без стоп-слів: " + string.Join(", ", filteredTokens));

            Console.WriteLine("\n=== Performance Comparison (Linear vs Inverted Index) ===\n");
            int[] sizes = { 10, 100, 1000, 10000 };
            foreach (int size in sizes)
            {
                var generatedDocs = PerformanceTest.GenerateDocuments(size);
                PerformanceTest.CompareSearchMethods(generatedDocs, "python");
            }

            while (true)
            {
                Console.Write("Enter search query (or 'exit'): ");
                var query = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(query) || query.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("\n--- Search Results (AND) ---");
                var results = index.SearchMultipleWords(words);
                foreach (var doc in results)
                {
                    Console.WriteLine(doc);
                    Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
                }

                Console.WriteLine($"\nFound {results.Count} documents\n");
            }
        }

        private static void LoadDocuments(InvertedIndex index)
        {
            var documents = new List<Document>
            {
                new Document
                {
                    Id = 1,
                    Title = "Introduction to C#",
                    Content = "C# is a modern object-oriented programming language developed by Microsoft."
                },
                new Document
                {
                    Id = 2,
                    Title = "Java Basics",
                    Content = "Java is a popular programming language known for its portability and performance."
                },
                new Document
                {
                    Id = 3,
                    Title = "Python Overview",
                    Content = "Python is a high-level programming language with simple syntax."
                },
                new Document
                {
                    Id = 4,
                    Title = "C# Advanced Features",
                    Content = "C# supports LINQ, async/await, and many modern programming paradigms."
                },
                new Document
                {
                    Id = 5,
                    Title = "Comparing Languages",
                    Content = "C#, Java, and Python are among the most popular programming languages today."
                }
            };

            foreach (var doc in documents)
            {
                index.AddDocument(doc);
            }
        }
    }
}