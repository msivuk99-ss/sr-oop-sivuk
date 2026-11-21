using System;

namespace IndependentWork7
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cache = new Cache<string, string>(3);

            cache.Add("user_1", "Олена");
            cache.Add("user_2", "Андрій");
            cache.Add("user_3", "Марія");

            Console.WriteLine($"Кількість елементів у кеші: {cache.Count}");
            Console.WriteLine($"Отримано user_1: {cache.Get("user_1")}");
            Console.WriteLine($"Чи містить user_2: {cache.ContainsKey("user_2")}");

            cache.Add("user_4", "Тарас");

            Console.WriteLine($"Після додавання user_4 кількість: {cache.Count}");
            Console.WriteLine($"Чи містить найстаріший ключ user_1: {cache.ContainsKey("user_1")}");
            Console.WriteLine($"Отримано user_4: {cache.Get("user_4")}");

            try
            {
                cache.Get("user_1");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Очікуваний виняток: {ex.Message}");
            }
        }
    }
}