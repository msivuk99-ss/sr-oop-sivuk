using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork12
{
    public static class PerformanceBenchmark
    {
        public static List<int> GenerateRandomList(int size, int seed = 42)
        {
            var random = new Random(seed);
            var data = new List<int>(size);

            for (int i = 0; i < size; i++)
            {
                data.Add(random.Next(10_000, 500_000));
            }

            return data;
        }

        public static void RunHeavyComparison(int size)
        {
            var data = GenerateRandomList(size);

            var warmup = data.Take(1000).Where(MathOperations.IsPrime).Select(MathOperations.HeavyTransform).ToList();
            var warmupParallel = data.Take(1000).AsParallel().Where(MathOperations.IsPrime).Select(MathOperations.HeavyTransform).ToList();

            var swLinq = Stopwatch.StartNew();
            var linqResult = data
                .Where(MathOperations.IsPrime)
                .Select(MathOperations.HeavyTransform)
                .ToList();
            swLinq.Stop();

            var swPlinq = Stopwatch.StartNew();
            var plinqResult = data
                .AsParallel()
                .Where(MathOperations.IsPrime)
                .Select(MathOperations.HeavyTransform)
                .ToList();
            swPlinq.Stop();

            double speedup = swPlinq.Elapsed.TotalMilliseconds > 0
                ? swLinq.Elapsed.TotalMilliseconds / swPlinq.Elapsed.TotalMilliseconds
                : 0;

            Console.WriteLine($"Обсяг колекції: {size:N0} елементів (важка операція: IsPrime + HeavyTransform)");
            Console.WriteLine($"  LINQ  : {swLinq.ElapsedMilliseconds,6} ms ({swLinq.ElapsedTicks,10} ticks) | Знайдено: {linqResult.Count:N0}");
            Console.WriteLine($"  PLINQ : {swPlinq.ElapsedMilliseconds,6} ms ({swPlinq.ElapsedTicks,10} ticks) | Знайдено: {plinqResult.Count:N0}");
            Console.WriteLine($"  Прискорення PLINQ: {speedup:F2}x\n");
        }

        public static void RunLightweightComparison(int size)
        {
            var data = GenerateRandomList(size);

            var swLinq = Stopwatch.StartNew();
            var linqResult = data
                .Where(MathOperations.IsEven)
                .Select(x => x + 1)
                .ToList();
            swLinq.Stop();

            var swPlinq = Stopwatch.StartNew();
            var plinqResult = data
                .AsParallel()
                .Where(MathOperations.IsEven)
                .Select(x => x + 1)
                .ToList();
            swPlinq.Stop();

            Console.WriteLine($"Обсяг колекції: {size:N0} елементів (тривіальна операція: x % 2 == 0)");
            Console.WriteLine($"  LINQ  : {swLinq.Elapsed.TotalMilliseconds,8:F4} ms ({swLinq.ElapsedTicks,8} ticks) | Знайдено: {linqResult.Count:N0}");
            Console.WriteLine($"  PLINQ : {swPlinq.Elapsed.TotalMilliseconds,8:F4} ms ({swPlinq.ElapsedTicks,8} ticks) | Знайдено: {plinqResult.Count:N0}\n");
        }
    }
}