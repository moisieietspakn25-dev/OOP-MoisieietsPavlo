using System;

class Program
{
    static void Main()
    {
        RetryPolicy retryPolicy = new RetryPolicy(
            maxRetries: 3,
            initialDelay: TimeSpan.FromMilliseconds(500),
            jitterFactor: 0.2
        );

        Console.WriteLine("================================");
        Console.WriteLine("СЦЕНАРІЙ 1: УСПІШНЕ ВИКОНАННЯ");
        Console.WriteLine("================================");

        int attempts = 0;

        try
        {
            string result = retryPolicy.Execute(() =>
            {
                attempts++;

                if (attempts < 3)
                {
                    throw new Exception("Сервер тимчасово недоступний.");
                }

                return "Дані успішно отримані із сервера!";
            });

            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Остаточна помилка: {ex.Message}");
        }

        Console.WriteLine();

        Console.WriteLine("================================");
        Console.WriteLine("СЦЕНАРІЙ 2: ОСТАТОЧНА НЕВДАЧА");
        Console.WriteLine("================================");

        try
        {
            string result = retryPolicy.Execute<string>(() =>
            {
                throw new Exception("Постійна помилка підключення.");
            });

            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Операція завершилася помилкою: {ex.Message}");
        }

        Console.WriteLine();

        Console.WriteLine("================================");
        Console.WriteLine("СЦЕНАРІЙ 3: SHOULDRETRY");
        Console.WriteLine("================================");

        try
        {
            retryPolicy.Execute<int>(
                () => throw new InvalidOperationException(
                    "Критична помилка."),
                ex => ex is TimeoutException
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Повторення зупинено: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Програму завершено.");
    }
}
