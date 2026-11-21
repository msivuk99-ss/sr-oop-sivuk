using System;
using System.Collections.Generic;

namespace IndependentWork11
{
    public static class DataGenerator
    {
        private static readonly string[] Categories =
        {
            "Електроніка",
            "Периферія",
            "Комплектуючі",
            "Мережеве обладнання",
            "Аудіо"
        };

        private static readonly string[] ProductPrefixes =
        {
            "Монітор",
            "Клавіатура",
            "Миша",
            "Накопичувач SSD",
            "Роутер",
            "Навушники",
            "Вебкамера",
            "Мікрофон",
            "Блок живлення",
            "Комутатор"
        };

        private static readonly string[] Cities =
        {
            "Рівне",
            "Київ",
            "Львів",
            "Одеса",
            "Харків",
            "Дніпро"
        };

        private static readonly string[] FirstNames =
        {
            "Олександр",
            "Марія",
            "Андрій",
            "Олена",
            "Дмитро",
            "Ірина",
            "Максим",
            "Наталія",
            "Тарас",
            "Софія"
        };

        private static readonly string[] LastNames =
        {
            "Коваленко",
            "Бондаренко",
            "Ткаченко",
            "Шевченко",
            "Кравчук",
            "Мельник",
            "Савченко",
            "Поліщук",
            "Мороз",
            "Лисенко"
        };

        public static List<Product> GenerateProducts(int count, int seed = 42)
        {
            var random = new Random(seed);
            var products = new List<Product>(count);

            for (int i = 1; i <= count; i++)
            {
                string prefix = ProductPrefixes[random.Next(ProductPrefixes.Length)];
                string category = Categories[random.Next(Categories.Length)];
                decimal price = random.Next(5, 150) * 100m;

                products.Add(new Product
                {
                    Id = i,
                    Name = $"{prefix} Pro-{100 + i}",
                    Category = category,
                    UnitPrice = price
                });
            }

            return products;
        }

        public static List<Customer> GenerateCustomers(int count, int seed = 42)
        {
            var random = new Random(seed + 1);
            var customers = new List<Customer>(count);

            for (int i = 1; i <= count; i++)
            {
                string firstName = FirstNames[random.Next(FirstNames.Length)];
                string lastName = LastNames[random.Next(LastNames.Length)];
                string city = Cities[random.Next(Cities.Length)];

                customers.Add(new Customer
                {
                    Id = i,
                    Name = $"{firstName} {lastName}",
                    City = city
                });
            }

            return customers;
        }

        public static List<Sale> GenerateSales(int count, int productCount, int customerCount, int seed = 42)
        {
            var random = new Random(seed + 2);
            var sales = new List<Sale>(count);
            var startDate = new DateTime(2025, 1, 1);
            int totalDays = 670;

            for (int i = 1; i <= count; i++)
            {
                sales.Add(new Sale
                {
                    Id = i,
                    ProductId = random.Next(1, productCount + 1),
                    CustomerId = random.Next(1, customerCount + 1),
                    Quantity = random.Next(1, 11),
                    SaleDate = startDate.AddDays(random.Next(0, totalDays))
                });
            }

            return sales;
        }
    }
}