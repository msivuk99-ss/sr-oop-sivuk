using System;
using System.Threading;

namespace IndependentWork14
{
    public class RetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _initialDelay;
        private readonly double _jitterFactor;
        private readonly Random _random;

        public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
        {
            if (maxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            }

            if (initialDelay < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(initialDelay));
            }

            if (jitterFactor < 0 || jitterFactor > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(jitterFactor));
            }

            _maxRetries = maxRetries;
            _initialDelay = initialDelay;
            _jitterFactor = jitterFactor;
            _random = new Random();
        }

        public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            int retryAttempt = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine($"[Спроба #{retryAttempt + 1}] Виконання операції...");
                    T result = operation();
                    Console.WriteLine($"[Успіх] Операцію виконано на спробі #{retryAttempt + 1}.");
                    return result;
                }
                catch (Exception ex)
                {
                    bool canRetry = retryAttempt < _maxRetries && (shouldRetry == null || shouldRetry(ex));

                    if (!canRetry)
                    {
                        Console.WriteLine(
                            $"[Відмова] Повтор не виконується (спроба #{retryAttempt + 1}). " +
                            $"Причина: {ex.GetType().Name}: {ex.Message}");
                        throw;
                    }

                    TimeSpan delay = _initialDelay * Math.Pow(2, retryAttempt) *
                                     (1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor);

                    retryAttempt++;
                    Console.WriteLine(
                        $"[Помилка] Спроба #{retryAttempt} невдала ({ex.GetType().Name}: {ex.Message}). " +
                        $"Очікування {delay.TotalMilliseconds:F0} мс перед спробою #{retryAttempt + 1}...");

                    Thread.Sleep(delay);
                }
            }
        }
    }
}