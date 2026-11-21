using System;

namespace IndependentWork19
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Демонстрація власного типу винятку OutOfStockException ===");
            var warehouse = new WarehouseService();

            try
            {
                warehouse.ReserveProduct("LAPTOP-PRO-15", 2, 10);
                warehouse.ReserveProduct("MONITOR-4K-27", 5, 2);
            }
            catch (OutOfStockException ex)
            {
                Console.WriteLine($"Помилка складу: {ex.Message}");
                Console.WriteLine($"  Артикул (SKU)      : {ex.ProductSku}");
                Console.WriteLine($"  Запитано одиниць   : {ex.RequestedQuantity}");
                Console.WriteLine($"  Доступно на складі : {ex.AvailableQuantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непередбачена помилка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Перевірку складських залишків завершено.\n");
            }

            PollyScenarios.RunRetryScenario();
            PollyScenarios.RunCircuitBreakerScenario();
            PollyScenarios.RunTimeoutAndFallbackScenario();
        }
    }
}