public sealed class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new();

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        if (maxRetries < 0) throw new ArgumentOutOfRangeException(nameof(maxRetries));
        if (initialDelay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(initialDelay));
        if (jitterFactor < 0 || jitterFactor > 1) throw new ArgumentOutOfRangeException(nameof(jitterFactor));
        _maxRetries = maxRetries; _initialDelay = initialDelay; _jitterFactor = jitterFactor;
    }

    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        shouldRetry ??= _ => true;
        for (int attempt = 0; ; attempt++)
        {
            try { return operation(); }
            catch (Exception ex) when (attempt < _maxRetries && shouldRetry(ex))
            {
                var baseMs = _initialDelay.TotalMilliseconds * Math.Pow(2, attempt);
                var jitter = 1 + ((_random.NextDouble() * 2 - 1) * _jitterFactor);
                var delay = TimeSpan.FromMilliseconds(Math.Max(0, baseMs * jitter));
                Console.WriteLine($"Помилка: {ex.Message}. Повтор {attempt + 1}/{_maxRetries} через {delay.TotalMilliseconds:F0} мс.");
                Thread.Sleep(delay);
            }
        }
    }
}
