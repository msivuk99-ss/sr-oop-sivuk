using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IndependentWork12
{
    public static class SideEffectsDemo
    {
        public static void Run()
        {
            var numbers = Enumerable.Range(1, 500_000).ToList();
            int expectedCount = numbers.Count(n => n % 2 == 0);

            Console.WriteLine("=== Дослідження безпеки та побічних ефектів у PLINQ ===");
            Console.WriteLine($"Очікувана кількість парних чисел: {expectedCount:N0}\n");

            Console.WriteLine("1. Небезпечна модифікація спільної змінної без синхронізації (Race Condition):");
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                int unsafeCounter = 0;

                var query = numbers
                    .AsParallel()
                    .Where(n =>
                    {
                        if (n % 2 == 0)
                        {
                            unsafeCounter++;
                            return true;
                        }
                        return false;
                    })
                    .ToList();

                Console.WriteLine($"  Спроба {attempt}: unsafeCounter = {unsafeCounter:N0} (втрачено: {expectedCount - unsafeCounter:N0})");
            }

            Console.WriteLine("\n2. Виправлення через Interlocked.Increment (атомарна операція):");
            int interlockedCounter = 0;
            _ = numbers
                .AsParallel()
                .Where(n =>
                {
                    if (n % 2 == 0)
                    {
                        Interlocked.Increment(ref interlockedCounter);
                        return true;
                    }
                    return false;
                })
                .ToList();
            Console.WriteLine($"  Результат з Interlocked: {interlockedCounter:N0} (коректно: {interlockedCounter == expectedCount})");

            Console.WriteLine("\n3. Виправлення через lock та потокобезпечну колекцію ConcurrentBag<T>:");
            object syncLock = new object();
            int lockedCounter = 0;
            var safeBag = new ConcurrentBag<int>();

            numbers
                .AsParallel()
                .ForAll(n =>
                {
                    if (n % 2 == 0)
                    {
                        lock (syncLock)
                        {
                            lockedCounter++;
                        }
                        safeBag.Add(n);
                    }
                });

            Console.WriteLine($"  Результат з lock         : {lockedCounter:N0} (коректно: {lockedCounter == expectedCount})");
            Console.WriteLine($"  Елементів у ConcurrentBag: {safeBag.Count:N0} (коректно: {safeBag.Count == expectedCount})");

            Console.WriteLine("\n4. Ідіоматичний підхід без побічних ефектів (чистий PLINQ):");
            int purePlinqCount = numbers
                .AsParallel()
                .Count(n => n % 2 == 0);
            Console.WriteLine($"  Результат чистого PLINQ  : {purePlinqCount:N0} (коректно: {purePlinqCount == expectedCount})\n");
        }
    }
}