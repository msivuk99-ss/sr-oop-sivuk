using System;

namespace IndependentWork14
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== Сценарій 1: Кеш метаданих (Dictionary) ===");
            var metadataCache = new FileMetadataCache();

            metadataCache.Add("docs/report.pdf", new FileMetadata
            {
                FileName = "report.pdf",
                Size = 204800,
                LastModified = new DateTime(2026, 11, 20, 14, 30, 0),
                Hash = "A1B2C3D4"
            });

            metadataCache.Add("src/Program.cs", new FileMetadata
            {
                FileName = "Program.cs",
                Size = 4096,
                LastModified = new DateTime(2026, 11, 21, 10, 15, 0),
                Hash = "E5F6G7H8"
            });

            Console.WriteLine($"Кількість файлів у кеші: {metadataCache.Count}");
            Console.WriteLine($"Чи є DOCS/REPORT.PDF: {metadataCache.Contains("DOCS/REPORT.PDF")}");

            var file = metadataCache.Get("docs/report.pdf");
            Console.WriteLine($"Отримано: {file.FileName}, розмір: {file.Size} байт, хеш: {file.Hash}");

            metadataCache.Remove("src/Program.cs");
            Console.WriteLine($"Після видалення Program.cs кількість: {metadataCache.Count}");

            Console.WriteLine("\n=== Сценарій 2: Унікальні теги (HashSet) ===");
            var tagManager = new TagManager();

            tagManager.AddTag("article-1", "CSharp");
            tagManager.AddTag("article-1", "DotNet");
            tagManager.AddTag("article-1", "csharp");
            tagManager.AddTag("article-1", "Collections");

            tagManager.AddTag("article-2", "DotNet");
            tagManager.AddTag("article-2", "Collections");
            tagManager.AddTag("article-2", "Performance");

            Console.WriteLine("Усі унікальні теги: " + string.Join(", ", tagManager.GetAllUniqueTags()));
            Console.WriteLine("Спільні теги (article-1 та article-2): " + string.Join(", ", tagManager.FindCommonTags("article-1", "article-2")));
            Console.WriteLine("Унікальні теги лише для article-1: " + string.Join(", ", tagManager.FindUniqueTagsForFirst("article-1", "article-2")));

            tagManager.RemoveTag("article-1", "Collections");
            Console.WriteLine("Спільні теги після видалення 'Collections' з article-1: " + string.Join(", ", tagManager.FindCommonTags("article-1", "article-2")));

            Console.WriteLine("\n=== Сценарій 3: Пошуковий індекс (Dictionary + HashSet) ===");
            var searchIndex = new SearchIndex();

            searchIndex.AddDocument(1, "C# collections Dictionary and HashSet provide fast lookup.");
            searchIndex.AddDocument(2, "Dictionary stores key-value pairs in C#.");
            searchIndex.AddDocument(3, "HashSet stores unique elements and supports set operations.");

            Console.WriteLine("Документи зі словом 'Dictionary': " + string.Join(", ", searchIndex.Search("Dictionary")));
            Console.WriteLine("Документи зі словами 'C#' та 'Dictionary': " + string.Join(", ", searchIndex.SearchMultiple("C#", "Dictionary")));
            Console.WriteLine("Документи зі словами 'Dictionary' та 'HashSet': " + string.Join(", ", searchIndex.SearchMultiple("Dictionary", "HashSet")));
        }
    }
}