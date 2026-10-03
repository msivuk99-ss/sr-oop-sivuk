using System;
using System.Collections.Generic;

namespace IndependentWork2
{
    public static class ProceduralDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== 1. ПРОЦЕДУРНА ВЕРСІЯ ===");

            List<string> names = new List<string> { "Ноутбук", "Мишка", "Монітор" };
            List<double> prices = new List<double> { 25000, 400, 6500 };
            List<int> quantities = new List<int> { 1, 2, 1 };

            double subtotal = CalculateSubtotal(prices, quantities);
            double totalDiscount = CalculateDiscount(prices, quantities);
            double finalTotal = subtotal - totalDiscount;

            for (int i = 0; i < names.Count; i++)
            {
                double lineTotal = prices[i] * quantities[i];
                double lineDiscount = (prices[i] > 500) ? lineTotal * 0.10 : 0;
                Console.WriteLine($"- {names[i]}: {quantities[i]} шт. х {prices[i]} грн = {lineTotal} грн (Знижка: {lineDiscount} грн)");
            }

            Console.WriteLine($"Загальна сума без знижки: {subtotal} грн");
            Console.WriteLine($"Загальна знижка: {totalDiscount} грн");
            Console.WriteLine($"РАЗОМ ДО СПЛАТИ: {finalTotal} грн\n");
        }

        public static double CalculateSubtotal(List<double> prices, List<int> quantities)
        {
            double sum = 0;
            for (int i = 0; i < prices.Count; i++)
            {
                sum += prices[i] * quantities[i];
            }
            return sum;
        }

        public static double CalculateDiscount(List<double> prices, List<int> quantities)
        {
            double totalDiscount = 0;
            for (int i = 0; i < prices.Count; i++)
            {
                if (prices[i] > 500)
                {
                    totalDiscount += (prices[i] * quantities[i]) * 0.10;
                }
            }
            return totalDiscount;
        }
    }
}