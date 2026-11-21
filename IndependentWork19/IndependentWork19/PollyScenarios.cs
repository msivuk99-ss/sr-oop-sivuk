using System;
using System.Net.Http;
using System.Threading;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

namespace IndependentWork19
{
    public static class PollyScenarios
    {
        public static void RunRetryScenario()
        {
            Console.WriteLine("=== Сценарій 1: Retry з експоненційною затримкою та Jitter (API служби доставки) ===");

            int attemptCounter = 0;

            var pipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(300),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    OnRetry = args =>
                    {
                        Console.WriteLine(
                            $"  [Retry] Спроба #{args.AttemptNumber + 1} невдала ({args.Outcome.Exception?.Message}). " +
                            $"Повтор через {args.RetryDelay.TotalMilliseconds:F0} мс...");
                        return default;
                    }
                })
                .Build();

            try
            {
                string trackingNumber = pipeline.Execute(() =>
                {
                    attemptCounter++;
                    Console.WriteLine($"  [Виклик API] Спроба виконання #{attemptCounter}...");

                    if (attemptCounter < 3)
                    {
                        throw new HttpRequestException("503 Service Unavailable: Сервер доставки тимчасово перевантажений.");
                    }

                    return "UA-2026-987654";
                });

                Console.WriteLine($"  [Успіх] Створено накладну: {trackingNumber}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [Помилка] Усі спроби вичерпано: {ex.Message}\n");
            }
        }

        public static void RunCircuitBreakerScenario()
        {
            Console.WriteLine("=== Сценарій 2: Circuit Breaker (Захист від каскадних збоїв платіжного шлюзу) ===");

            bool isGatewayHealthy = false;

            var pipeline = new ResiliencePipelineBuilder()
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<InvalidOperationException>(),
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(10),
                    MinimumThroughput = 3,
                    BreakDuration = TimeSpan.FromSeconds(2),
                    OnOpened = args =>
                    {
                        Console.WriteLine(
                            $"  [Circuit Breaker -> OPEN] Ланцюг розімкнено на {args.BreakDuration.TotalSeconds} с через серію збоїв!");
                        return default;
                    },
                    OnHalfOpened = args =>
                    {
                        Console.WriteLine("  [Circuit Breaker -> HALF-OPEN] Перевірка відновлення сервісу...");
                        return default;
                    },
                    OnClosed = args =>
                    {
                        Console.WriteLine("  [Circuit Breaker -> CLOSED] Роботу сервісу повністю відновлено.");
                        return default;
                    }
                })
                .Build();

            for (int i = 1; i <= 6; i++)
            {
                if (i == 6)
                {
                    Console.WriteLine("  [Очікування] Пауза 2.2 с для переходу в стан Half-Open...");
                    Thread.Sleep(2200);
                    isGatewayHealthy = true;
                }

                try
                {
                    pipeline.Execute(() =>
                    {
                        Console.WriteLine($"  [Запит #{i}] Звернення до платіжного шлюзу...");
                        if (!isGatewayHealthy)
                        {
                            throw new InvalidOperationException("Помилка з'єднання з платіжним шлюзом.");
                        }

                        Console.WriteLine($"  [Запит #{i}] Платіж успішно оброблено!");
                    });
                }
                catch (BrokenCircuitException)
                {
                    Console.WriteLine($"  [Запит #{i}] Відхилено миттєво (BrokenCircuitException): виклик сервісу заблоковано.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"  [Запит #{i}] Збій сервісу: {ex.Message}");
                }
            }

            Console.WriteLine();
        }

        public static void RunTimeoutAndFallbackScenario()
        {
            Console.WriteLine("=== Сценарій 3: Комбінація Timeout + Fallback (Отримання курсу валют) ===");

            int simulatedDelayMs = 100;

            var pipeline = new ResiliencePipelineBuilder<string>()
                .AddFallback(new FallbackStrategyOptions<string>
                {
                    ShouldHandle = new PredicateBuilder<string>()
                        .Handle<TimeoutRejectedException>()
                        .Handle<HttpRequestException>(),
                    FallbackAction = args =>
                    {
                        Console.WriteLine(
                            $"  [Fallback] Активовано резервний сценарій через: {args.Outcome.Exception?.GetType().Name}");
                        return Outcome.FromResultAsValueTask("1 USD = 41.50 UAH (резервне кешоване значення)");
                    }
                })
                .AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromMilliseconds(350),
                    OnTimeout = args =>
                    {
                        Console.WriteLine($"  [Timeout] Перевищено ліміт очікування ({args.Timeout.TotalMilliseconds} мс)!");
                        return default;
                    }
                })
                .Build();

            for (int request = 1; request <= 2; request++)
            {
                simulatedDelayMs = request == 1 ? 100 : 1000;
                Console.WriteLine($"  [Запит #{request}] Очікувана затримка зовнішнього API: {simulatedDelayMs} мс");

                string rate = pipeline.Execute(ct =>
                {
                    ct.WaitHandle.WaitOne(simulatedDelayMs);
                    ct.ThrowIfCancellationRequested();
                    return "1 USD = 41.85 UAH (актуальний курс API)";
                });

                Console.WriteLine($"  [Результат #{request}] {rate}\n");
            }
        }
    }
}