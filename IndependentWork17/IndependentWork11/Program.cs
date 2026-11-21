using System;

namespace IndependentWork11
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var products = DataGenerator.GenerateProducts(60);
            var customers = DataGenerator.GenerateCustomers(60);
            var sales = DataGenerator.GenerateSales(100, products.Count, customers.Count);

            Console.WriteLine($"Згенеровано тестові дані: {products.Count} продуктів, {customers.Count} клієнтів, {sales.Count} продажів.\n");

            SalesAnalytics.TopProductsByCategory(sales, products);
            SalesAnalytics.TotalRevenueByCustomerCity(sales, products, customers);
            SalesAnalytics.SalesByYearAndMonth(sales, products);
            SalesAnalytics.TopWholesaleCustomers(sales, products, customers);
        }
    }
}