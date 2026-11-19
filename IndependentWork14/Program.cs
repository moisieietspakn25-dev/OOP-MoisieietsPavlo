using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

var retry = new ResiliencePipelineBuilder()
    .AddRetry(new Polly.Retry.RetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromMilliseconds(200),
        BackoffType = DelayBackoffType.Exponential,
        OnRetry = args => { Console.WriteLine($"Retry: спроба {args.AttemptNumber}"); return default; }
    }).Build();

int attempts = 0;
await retry.ExecuteAsync(_ =>
{
    attempts++;
    Console.WriteLine($"Спроба підключення №{attempts}");
    if (attempts < 3) throw new HttpRequestException("Тимчасова помилка мережі");
    Console.WriteLine("Підключення успішне.");
    return ValueTask.CompletedTask;
});

var timeout = new ResiliencePipelineBuilder()
    .AddTimeout(TimeSpan.FromSeconds(1))
    .Build();
try
{
    await timeout.ExecuteAsync(async token =>
    {
        Console.WriteLine("Запуск операції з обмеженням часу...");
        await Task.Delay(TimeSpan.FromSeconds(2), token);
    });
}
catch (TimeoutRejectedException)
{
    Console.WriteLine("Операцію перервано через Timeout.");
}
