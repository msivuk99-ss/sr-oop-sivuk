using System;
using System.Net.Http;

namespace IndependentWork14
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var retryPolicy = new RetryPolicy(
                maxRetries: 3,
                initialDelay: TimeSpan.FromMilliseconds(200),
                jitterFactor: 0.2);

            Console.WriteLine("=== Сценарій 1: Тимчасовий збій з подальшим успіхом ===");
            int transientAttempts = 0;
            try
            {
                string response = retryPolicy.Execute(() =>
                {
                    transientAttempts++;
                    if (transientAttempts < 3)
                    {
                        throw new HttpRequestException("Тимчасова втрата з'єднання із сервером (503).");
                    }

                    return "Дані успішно отримано (HTTP 200 OK)";
                });

                Console.WriteLine($"Результат: {response}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неочікувана помилка: {ex.Message}\n");
            }

            Console.WriteLine("=== Сценарій 2: Постійний збій і вичерпання всіх спроб ===");
            try
            {
                retryPolicy.Execute<string>(() =>
                {
                    throw new TimeoutException("Перевищено час очікування відповіді від бази даних.");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Перехоплено фінальний виняток у Main: {ex.GetType().Name} — {ex.Message}\n");
            }

            Console.WriteLine("=== Сценарій 3: Фільтрація винятків через shouldRetry ===");
            try
            {
                retryPolicy.Execute<string>(
                    () => throw new UnauthorizedAccessException("Недійсний токен доступу (401)."),
                    shouldRetry: ex => ex is HttpRequestException || ex is TimeoutException);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Миттєве завершення без повторів: {ex.GetType().Name} — {ex.Message}\n");
            }
        }
    }
}