using System;
using System.Collections.Generic;

namespace lab9_self_vN
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== 1. Демонстрація з List<int> ===");
            var numbers = new List<int> { 14, 7, 29, 42, 3, 18, 35 };

            var evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
            Console.WriteLine("Парні числа (Filter): " + string.Join(", ", evenNumbers));

            var squares = CollectionUtils.Map(numbers, n => n * n);
            Console.WriteLine("Квадрати чисел (Map): " + string.Join(", ", squares));

            int maxNumber = CollectionUtils.MaxBy(numbers, n => n);
            int maxLastDigit = CollectionUtils.MaxBy(numbers, n => n % 10);
            Console.WriteLine($"Найбільше число (MaxBy): {maxNumber}");
            Console.WriteLine($"Число з найбільшою останньою цифрою (MaxBy): {maxLastDigit}");

            Console.Write("Числа, більші за 20 (ForEach): ");
            var largeNumbers = CollectionUtils.Filter(numbers, n => n > 20);
            CollectionUtils.ForEach(largeNumbers, n => Console.Write($"{n} "));
            Console.WriteLine("\n");

            Console.WriteLine("=== 2. Демонстрація з List<string> ===");
            var cities = new List<string> { "Рівне", "Київ", "Львів", "Одеса", "Харків", "Тернопіль" };

            var cityLengths = CollectionUtils.Map(cities, c => $"{c} ({c.Length} симв.)");
            Console.WriteLine("Довжина назв (Map): " + string.Join(", ", cityLengths));

            var longCities = CollectionUtils.Filter(cities, c => c.Length > 5);
            Console.WriteLine("Міста з назвою довше 5 символів (Filter): " + string.Join(", ", longCities));

            string longestCity = CollectionUtils.MaxBy(cities, c => c.Length);
            Console.WriteLine($"Найдовша назва міста (MaxBy): {longestCity}");

            Console.Write("Міста у верхньому регістрі (ForEach): ");
            CollectionUtils.ForEach(cities, c => Console.Write(c.ToUpper() + " "));
            Console.WriteLine("\n");

            Console.WriteLine("=== 3. Демонстрація з List<Product> ===");
            var products = new List<Product>
            {
                new Product { Name = "Монітор 27\"", Category = "Дисплеї", Price = 11500m },
                new Product { Name = "Вебкамера FullHD", Category = "Периферія", Price = 2200m },
                new Product { Name = "SSD 1TB NVMe", Category = "Накопичувачі", Price = 3400m },
                new Product { Name = "Навушники студійні", Category = "Аудіо", Price = 4800m },
                new Product { Name = "Мікрофон USB", Category = "Аудіо", Price = 2900m }
            };

            var productLabels = CollectionUtils.Map(products, p => $"{p.Name} [{p.Category}]");
            Console.WriteLine("Список товарів (Map): " + string.Join(", ", productLabels));

            var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
            Console.WriteLine($"Найдорожчий товар (MaxBy): {mostExpensive.Name} — {mostExpensive.Price} грн");

            var midRangeProducts = CollectionUtils.Filter(products, p => p.Price >= 3000m && p.Price <= 5000m);
            Console.WriteLine("Товари в діапазоні 3000–5000 грн (Filter + ForEach):");
            CollectionUtils.ForEach(midRangeProducts, p => Console.WriteLine($"  - {p.Name}: {p.Price} грн"));
        }
    }
}