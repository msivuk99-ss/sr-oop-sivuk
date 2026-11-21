using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork11
{
    public static class SalesAnalytics
    {
        public static void TopProductsByCategory(List<Sale> sales, List<Product> products)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .GroupBy(s => s.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(s => s.Quantity)
                })
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
                    {
                        p.Name,
                        p.Category,
                        s.TotalQuantity
                    })
                .GroupBy(x => x.Category)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Category = g.Key,
                    TopProducts = g
                        .OrderByDescending(x => x.TotalQuantity)
                        .Take(3)
                        .ToList()
                })
                .ToList();

            sw.Stop();

            Console.WriteLine("=== Пайплайн 1: Найпопулярніші продукти за категоріями (Топ-3) ===");
            foreach (var group in result)
            {
                Console.WriteLine($"Категорія: {group.Category}");
                foreach (var item in group.TopProducts)
                {
                    Console.WriteLine($"  - {item.Name}: {item.TotalQuantity} од.");
                }
            }
            Console.WriteLine($"Час виконання: {sw.ElapsedTicks} ticks ({sw.Elapsed.TotalMilliseconds:F4} ms)\n");
        }

        public static void TotalRevenueByCustomerCity(
            List<Sale> sales,
            List<Product> products,
            List<Customer> customers)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
                    {
                        s.CustomerId,
                        Revenue = s.Quantity * p.UnitPrice
                    })
                .Join(
                    customers,
                    sp => sp.CustomerId,
                    c => c.Id,
                    (sp, c) => new
                    {
                        c.City,
                        sp.Revenue
                    })
                .GroupBy(x => x.City)
                .Select(g => new
                {
                    City = g.Key,
                    TotalRevenue = g.Sum(x => x.Revenue),
                    SalesCount = g.Count()
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();

            sw.Stop();

            Console.WriteLine("=== Пайплайн 2: Загальний дохід за містами клієнтів ===");
            foreach (var item in result)
            {
                Console.WriteLine($"  - {item.City,-10} | Замовлень: {item.SalesCount,3} | Дохід: {item.TotalRevenue,12:N2} грн");
            }
            Console.WriteLine($"Час виконання: {sw.ElapsedTicks} ticks ({sw.Elapsed.TotalMilliseconds:F4} ms)\n");
        }

        public static void SalesByYearAndMonth(List<Sale> sales, List<Product> products)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
                    {
                        s.SaleDate.Year,
                        s.SaleDate.Month,
                        s.Quantity,
                        Revenue = s.Quantity * p.UnitPrice
                    })
                .GroupBy(x => new { x.Year, x.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalUnits = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Revenue)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            sw.Stop();

            Console.WriteLine("=== Пайплайн 3: Продажі за місяцями та роками ===");
            foreach (var item in result)
            {
                Console.WriteLine($"  - {item.Year}-{item.Month:D2} | Продано одиниць: {item.TotalUnits,3} | Дохід: {item.TotalRevenue,12:N2} грн");
            }
            Console.WriteLine($"Час виконання: {sw.ElapsedTicks} ticks ({sw.Elapsed.TotalMilliseconds:F4} ms)\n");
        }

        public static void TopWholesaleCustomers(
            List<Sale> sales,
            List<Product> products,
            List<Customer> customers)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Where(s => s.Quantity >= 3)
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
                    {
                        s.CustomerId,
                        Revenue = s.Quantity * p.UnitPrice
                    })
                .Join(
                    customers,
                    sp => sp.CustomerId,
                    c => c.Id,
                    (sp, c) => new
                    {
                        c.Id,
                        c.Name,
                        c.City,
                        sp.Revenue
                    })
                .GroupBy(x => new { x.Id, x.Name, x.City })
                .Select(g => new
                {
                    g.Key.Name,
                    g.Key.City,
                    TotalSpent = g.Sum(x => x.Revenue),
                    BulkOrders = g.Count()
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(5)
                .ToList();

            sw.Stop();

            Console.WriteLine("=== Пайплайн 4 (додатковий): Топ-5 клієнтів за оптовими покупками (від 3 од.) ===");
            foreach (var item in result)
            {
                Console.WriteLine($"  - {item.Name} ({item.City}) | Оптових замовлень: {item.BulkOrders} | Сума: {item.TotalSpent:N2} грн");
            }
            Console.WriteLine($"Час виконання: {sw.ElapsedTicks} ticks ({sw.Elapsed.TotalMilliseconds:F4} ms)\n");
        }
    }
}