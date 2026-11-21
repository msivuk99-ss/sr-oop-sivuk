using System;

namespace IndependentWork12
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine($"Кількість логічних ядер процесора: {Environment.ProcessorCount}\n");
            Console.WriteLine("=== Порівняння продуктивності LINQ та PLINQ ===\n");

            PerformanceBenchmark.RunLightweightComparison(50_000);
            PerformanceBenchmark.RunHeavyComparison(1_000_000);
            PerformanceBenchmark.RunHeavyComparison(5_000_000);
            PerformanceBenchmark.RunHeavyComparison(10_000_000);

            SideEffectsDemo.Run();
        }
    }
}