using System;
using System.Threading;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;

    public RetryPolicy(
        int maxRetries,
        TimeSpan initialDelay,
        double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
            throw new ArgumentOutOfRangeException(nameof(maxRetries));

        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay));

        if (jitterFactor < 0 || jitterFactor > 1)
            throw new ArgumentOutOfRangeException(nameof(jitterFactor));

        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(
        Func<T> operation,
        Func<Exception, bool> shouldRetry = null)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        for (int attempt = 0; ; attempt++)
        {
            Console.WriteLine($"Спроба №{attempt + 1}");

            try
            {
                T result = operation();

                Console.WriteLine("Операція виконана успішно!");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");

                if (attempt >= _maxRetries)
                {
                    Console.WriteLine("Вичерпано всі повторні спроби.");
                    throw;
                }

                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.WriteLine("Повторення заборонено shouldRetry.");
                    throw;
                }

                double multiplier =
                    1 + Random.Shared.NextDouble() *
                    _jitterFactor * 2 - _jitterFactor;

                double delayMilliseconds =
                    _initialDelay.TotalMilliseconds *
                    Math.Pow(2, attempt) * multiplier;

                TimeSpan delay =
                    TimeSpan.FromMilliseconds(delayMilliseconds);

                Console.WriteLine(
                    $"Повторна спроба через {delay.TotalMilliseconds:F0} мс");

                Thread.Sleep(delay);
            }
        }
    }
}