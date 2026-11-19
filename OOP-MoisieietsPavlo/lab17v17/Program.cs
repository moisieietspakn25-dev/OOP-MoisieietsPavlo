
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;

public class FileProcessor
{
    private int _attempts = 0;

    public List<string> LoadOrderIds(string path)
    {
        _attempts++;
        Console.WriteLine($"[FileProcessor] Спроба {_attempts}: читання {path}");

        if (_attempts <= 2)
        {
            throw new IOException("Тимчасова помилка читання файлу.");
        }

        Console.WriteLine("[FileProcessor] Файл успішно прочитано.");

        return new List<string>
        {
            "ORD-001",
            "ORD-002",
            "ORD-003"
        };
    }
}

public class NetworkClient
{
    private int _attempts = 0;

    public List<string> GetOrdersFromApi(string url)
    {
        _attempts++;
        Console.WriteLine($"[NetworkClient] Спроба {_attempts}: запит до {url}");

        if (_attempts <= 3)
        {
            throw new HttpRequestException("Тимчасова мережева помилка.");
        }

        Console.WriteLine("[NetworkClient] Дані успішно отримано.");

        return new List<string>
        {
            "ORD-004",
            "ORD-005",
            "ORD-006"
        };
    }
}

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        if (retryCount < 0)
            throw new ArgumentOutOfRangeException(nameof(retryCount));

        if (initialDelay == default)
            initialDelay = TimeSpan.FromMilliseconds(500);

        int attempt = 0;

        while (true)
        {
            try
            {
                attempt++;
                Console.WriteLine($"\n[RetryHelper] Виконання спроби {attempt}...");
                return operation();
            }
            catch (Exception ex)
            {
                bool canRetry = shouldRetry?.Invoke(ex) ?? true;

                if (!canRetry || attempt > retryCount)
                {
                    Console.WriteLine(
                        $"[RetryHelper] Повторення неможливе. Помилка: {ex.Message}");
                    throw;
                }

                TimeSpan delay = TimeSpan.FromMilliseconds(
                    initialDelay.TotalMilliseconds *
                    Math.Pow(2, attempt - 1));

                Console.WriteLine(
                    $"[RetryHelper] Помилка: {ex.Message}");

                Console.WriteLine(
                    $"[RetryHelper] Наступна спроба через {delay.TotalMilliseconds} мс.");

                Thread.Sleep(delay);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==================================");
        Console.WriteLine(" ЛАБОРАТОРНА РОБОТА №17");
        Console.WriteLine(" Варіант 17 — Отримання замовлень");
        Console.WriteLine("==================================");

        var fileProcessor = new FileProcessor();
        var networkClient = new NetworkClient();

        Func<Exception, bool> shouldRetry = ex =>
            ex is IOException || ex is HttpRequestException;

        Console.WriteLine("\n--- Завантаження замовлень із файлу ---");

        try
        {
            List<string> fileOrders =
                RetryHelper.ExecuteWithRetry(
                    () => fileProcessor.LoadOrderIds("orders.txt"),
                    retryCount: 3,
                    initialDelay: TimeSpan.FromMilliseconds(500),
                    shouldRetry: shouldRetry);

            Console.WriteLine("Замовлення з файлу:");
            foreach (string order in fileOrders)
                Console.WriteLine($"  {order}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine("\n--- Отримання замовлень через API ---");

        try
        {
            List<string> apiOrders =
                RetryHelper.ExecuteWithRetry(
                    () => networkClient.GetOrdersFromApi(
                        "https://example.com/api/orders"),
                    retryCount: 4,
                    initialDelay: TimeSpan.FromMilliseconds(500),
                    shouldRetry: shouldRetry);

            Console.WriteLine("Замовлення з API:");
            foreach (string order in apiOrders)
                Console.WriteLine($"  {order}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine("\n--- Перевірка shouldRetry ---");

        try
        {
            RetryHelper.ExecuteWithRetry(
                () => throw new UnauthorizedAccessException(
                    "Доступ заборонено."),
                retryCount: 3,
                initialDelay: TimeSpan.FromMilliseconds(500),
                shouldRetry: shouldRetry);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine(
                $"Вибіркова обробка: {ex.Message}");
            Console.WriteLine(
                "Цей виняток не повторюється.");
        }

        Console.WriteLine("\n==================================");
        Console.WriteLine(" Роботу завершено.");
        Console.WriteLine("==================================");
    }
}
