
using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;

public class Program
{
    private static int dbAttempts = 0;
    private static int queueAttempts = 0;
    private static int operationAttempts = 0;

    // ==========================================
    // SCENARIO 1: Retry - підключення до бази даних
    // ==========================================

    private static async Task<string> ConnectToDatabase(
        CancellationToken cancellationToken)
    {
        dbAttempts++;

        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] DB: спроба №{dbAttempts}");

        await Task.Delay(200, cancellationToken);

        if (dbAttempts <= 2)
        {
            throw new TimeoutException(
                "База даних тимчасово недоступна.");
        }

        return "Успішне підключення до бази даних!";
    }

    private static async Task Scenario1()
    {
        Console.WriteLine("\n========== SCENARIO 1 ==========");
        Console.WriteLine("Retry: підключення до бази даних");

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<TimeoutException>(),

                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                BackoffType = DelayBackoffType.Constant,

                OnRetry = args =>
                {
                    Console.WriteLine(
                        $"[RETRY] Повтор №{args.AttemptNumber + 1}. " +
                        $"Причина: {args.Outcome.Exception?.Message}");

                    return default;
                }
            })
            .Build();

        try
        {
            string result = await pipeline.ExecuteAsync(
                ConnectToDatabase,
                CancellationToken.None);

            Console.WriteLine($"[SUCCESS] {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
        }
    }

    // ==========================================
    // SCENARIO 2: Circuit Breaker - черга повідомлень
    // ==========================================

    private static string SendMessage()
    {
        queueAttempts++;

        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] Queue: спроба №{queueAttempts}");

        if (queueAttempts <= 2)
        {
            throw new InvalidOperationException(
                "Черга повідомлень перевантажена.");
        }

        return "Повідомлення успішно відправлено!";
    }

    private static void Scenario2()
    {
        Console.WriteLine("\n========== SCENARIO 2 ==========");
        Console.WriteLine("Circuit Breaker: черга повідомлень");

        var pipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<InvalidOperationException>(),

                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 2,
                BreakDuration = TimeSpan.FromSeconds(5),

                OnOpened = args =>
                {
                    Console.WriteLine(
                        "[CIRCUIT BREAKER] Ланцюг відкрито! " +
                        "Запити тимчасово блокуються.");

                    return default;
                },

                OnClosed = args =>
                {
                    Console.WriteLine(
                        "[CIRCUIT BREAKER] Ланцюг закрито.");

                    return default;
                }
            })
            .Build();

        // Виконуємо декілька запитів до черги.
        for (int i = 0; i < 4; i++)
        {
            try
            {
                string result = pipeline.Execute(SendMessage);
                Console.WriteLine($"[SUCCESS] {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[ERROR] {ex.GetType().Name}: {ex.Message}");

                if (ex.GetType().Name == "BrokenCircuitException")
                {
                    Console.WriteLine(
                        "[FALLBACK] Повідомлення збережено " +
                        "для повторної відправки.");
                }
            }

            Thread.Sleep(100);
        }
    }

    // ==========================================
    // SCENARIO 3: Timeout + Retry - довга операція
    // ==========================================

    private static async Task<string> LongOperation(
        CancellationToken cancellationToken)
    {
        operationAttempts++;

        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] Operation: " +
            $"спроба №{operationAttempts}");

        if (operationAttempts == 1)
        {
            // Перша спроба триває довше встановленого Timeout.
            await Task.Delay(3000, cancellationToken);
        }

        await Task.Delay(300, cancellationToken);

        return "Операцію успішно завершено!";
    }

    private static async Task Scenario3()
    {
        Console.WriteLine("\n========== SCENARIO 3 ==========");
        Console.WriteLine("Timeout + Retry: довга операція");

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<TimeoutRejectedException>(),

                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromMilliseconds(300),

                OnRetry = args =>
                {
                    Console.WriteLine(
                        "[RETRY] Повтор після перевищення часу очікування.");

                    return default;
                }
            })
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(1),

                OnTimeout = args =>
                {
                    Console.WriteLine(
                        "[TIMEOUT] Час виконання перевищено!");

                    return default;
                }
            })
            .Build();

        try
        {
            string result = await pipeline.ExecuteAsync(
                LongOperation,
                CancellationToken.None);

            Console.WriteLine($"[SUCCESS] {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.GetType().Name}: {ex.Message}");
        }
    }

    // ==========================================
    // MAIN
    // ==========================================

    public static async Task Main(string[] args)
    {
        Console.WriteLine("====================================");
        Console.WriteLine("     SAMOSTIINA ROBOTA №14");
        Console.WriteLine("          POLLY / RETRY");
        Console.WriteLine("====================================");

        await Scenario1();

        Scenario2();

        await Scenario3();

        Console.WriteLine("\n====================================");
        Console.WriteLine("       ВСІ СЦЕНАРІЇ ЗАВЕРШЕНО");
        Console.WriteLine("====================================");
    }
}

