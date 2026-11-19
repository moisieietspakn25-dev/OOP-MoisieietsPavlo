var policy = new RetryPolicy(3, TimeSpan.FromMilliseconds(150), 0.15);
int attempts = 0;
var result = policy.Execute(() =>
{
    attempts++;
    if (attempts < 3) throw new IOException("Тимчасова помилка читання");
    return "Операцію виконано успішно";
}, ex => ex is IOException);
Console.WriteLine(result);

try
{
    policy.Execute<int>(() => throw new InvalidOperationException("Некоректний стан"), ex => ex is IOException);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Без повтору: {ex.Message}");
}
