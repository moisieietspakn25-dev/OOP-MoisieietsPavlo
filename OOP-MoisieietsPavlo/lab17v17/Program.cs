
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace Lab17v17
{
    // Клас для імітації роботи з файлами
    public class FileProcessor
    {
        private int attempts = 0;

        // Завантаження ідентифікаторів замовлень
        public List<string> LoadOrderIds(string path)
        {
            attempts++;

            Console.WriteLine($"[FileProcessor] Спроба №{attempts}");

            // Перші 2 спроби завершуються помилкою
            if (attempts <= 2)
            {
                throw new IOException(
                    "Тимчасова помилка читання файлу."
                );
            }

            Console.WriteLine("[FileProcessor] Файл успішно прочитано!");

            return new List<string>
            {
                "ORD-001",
                "ORD-002",
                "ORD-003",
                "ORD-004",
                "ORD-005"
            };
        }
    }

    // Клас для імітації мережевих запитів
    public class NetworkClient
    {
        private int attempts = 0;

        // Отримання замовлень через API
        public List<string> GetOrdersFromApi(string url)
        {
            attempts++;

            Console.WriteLine($"[NetworkClient] Спроба №{attempts}");

            // Перші 3 спроби завершуються помилкою
            if (attempts <= 3)
            {
                throw new HttpRequestException(
                    "Тимчасова помилка мережевого з'єднання."
                );
            }

            Console.WriteLine("[NetworkClient] Дані успішно отримано!");

            return new List<string>
            {
                "ORD-101",
                "ORD-102",
                "ORD-103",
                "ORD-104"
            };
        }
    }

    // Узагальнений допоміжний клас повторних спроб
    public static class RetryHelper
    {
        public static T ExecuteWithRetry<T>(
            Func<T> operation,
            int retryCount = 3,
            TimeSpan initialDelay = default,
            Func<Exception, bool> shouldRetry = null)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount));

            if (initialDelay == default)
                initialDelay = TimeSpan.FromMilliseconds(500);

            for (int attempt = 0; ; attempt++)
            {
                Console.WriteLine(
                    $"\n[RetryHelper] Виконання операції. Спроба {attempt + 1}"
                );

                try
                {
                    T result = operation();

                    Console.WriteLine(
                        "[RetryHelper] Операція виконана успішно!"
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"[RetryHelper] Помилка: {ex.GetType().Name}"
                    );

                    Console.WriteLine(
                        $"[RetryHelper] Причина: {ex.Message}"
                    );

                    // Перевірка, чи дозволено повторювати операцію
                    bool canRetry = shouldRetry == null || shouldRetry(ex);

                    if (!canRetry)
                    {
                        Console.WriteLine(
                            "[RetryHelper] Цей виняток не підлягає повторенню."
                        );

                        throw;
                    }

                    if (attempt >= retryCount)
                    {
                        Console.WriteLine(
                            "[RetryHelper] Кількість повторних спроб вичерпана."
                        );

                        throw;
                    }

                    // Експоненційна затримка: initialDelay * 2^attempt
                    double delayMultiplier = Math.Pow(2, attempt);

                    TimeSpan delay = TimeSpan.FromMilliseconds(
                        initialDelay.TotalMilliseconds * delayMultiplier
                    );

                    Console.WriteLine(
                        $"[RetryHelper] Повтор через {delay.TotalSeconds} сек."
                    );

                    Thread.Sleep(delay);
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("====================================");
            Console.WriteLine(" ЛАБОРАТОРНА РОБОТА №17");
            Console.WriteLine(" Об'єктно-орієнтоване програмування");
            Console.WriteLine(" Варіант 17");
            Console.WriteLine(" Отримання списку замовлень");
            Console.WriteLine("====================================");

            FileProcessor fileProcessor = new FileProcessor();
            NetworkClient networkClient = new NetworkClient();

            // Визначення винятків, для яких дозволено повторення
            Func<Exception, bool> shouldRetry = ex =>
                ex is IOException ||
                ex is HttpRequestException;

            // Затримка між спробами
            TimeSpan initialDelay = TimeSpan.FromMilliseconds(500);

            // ==================================
            // 1. Завантаження замовлень із файлу
            // ==================================

            Console.WriteLine("\n--- ЗАВАНТАЖЕННЯ З ФАЙЛУ ---");

            try
            {
                List<string> fileOrders =
                    RetryHelper.ExecuteWithRetry(
                        () => fileProcessor.LoadOrderIds("orders.txt"),
                        retryCount: 3,
                        initialDelay: initialDelay,
                        shouldRetry: shouldRetry
                    );

                Console.WriteLine("\nСписок замовлень із файлу:");

                foreach (string order in fileOrders)
                {
                    Console.WriteLine(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            // ==================================
            // 2. Отримання замовлень через API
            // ==================================

            Console.WriteLine("\n--- ОТРИМАННЯ ЗАМОВЛЕНЬ ЧЕРЕЗ API ---");

            try
            {
                List<string> apiOrders =
                    RetryHelper.ExecuteWithRetry(
                        () => networkClient.GetOrdersFromApi(
                            "https://example.com/api/orders"
                        ),
                        retryCount: 3,
                        initialDelay: initialDelay,
                        shouldRetry: shouldRetry
                    );

                Console.WriteLine("\nСписок замовлень із API:");

                foreach (string order in apiOrders)
                {
                    Console.WriteLine(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            // ==================================
            // 3. Демонстрація shouldRetry
            // ==================================

            Console.WriteLine("\n--- ПЕРЕВІРКА SHOULDRETRY ---");

            try
            {
                RetryHelper.ExecuteWithRetry(
                    () =>
                    {
                        Console.WriteLine(
                            "Виконання тестової операції..."
                        );

                        throw new InvalidOperationException(
                            "Непідтримуваний тип помилки."
                        );

                        return "Успіх";
                    },
                    retryCount: 3,
                    initialDelay: initialDelay,
                    shouldRetry: shouldRetry
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Операцію зупинено: {ex.GetType().Name}"
                );
            }

            Console.WriteLine("\n====================================");
            Console.WriteLine(" Роботу програми завершено!");
            Console.WriteLine("====================================");
        }
    }
}

