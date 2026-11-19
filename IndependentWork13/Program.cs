
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static long HeavyCalculation(int value)
    {
        long total = 0;

        for (int i = 1; i <= 250; i++)
        {
            total += (long)(value + i) * (value + i);
        }

        return total;
    }

    static void Main()
    {
        foreach (var size in new[] { 1_000_000, 5_000_000, 10_000_000 })
        {
            Console.WriteLine($"\nКількість елементів: {size:N0}");

            var data = Enumerable.Range(1, size).ToArray();

            // Звичайний LINQ
            var sw = Stopwatch.StartNew();

            var linqResult = data
                .Where(x => HeavyCalculation(x) % 2 == 0)
                .LongCount();

            sw.Stop();
            var linqMs = sw.ElapsedMilliseconds;

            // Паралельний PLINQ
            sw.Restart();

            var plinqResult = data
                .AsParallel()
                .Where(x => HeavyCalculation(x) % 2 == 0)
                .LongCount();

            sw.Stop();

            Console.WriteLine(
                $"LINQ: {linqMs} мс");

            Console.WriteLine(
                $"PLINQ: {sw.ElapsedMilliseconds} мс");

            Console.WriteLine(
                $"Кількість збігається: {linqResult == plinqResult}");
        }

        // Демонстрація гонки даних
        int unsafeCounter = 0;

        Parallel.For(0, 100_000, _ =>
        {
            unsafeCounter++;
        });

        // Виправлення через Interlocked
        int safeCounter = 0;

        Parallel.For(0, 100_000, _ =>
        {
            Interlocked.Increment(ref safeCounter);
        });

        Console.WriteLine("\nРезультати лічильників:");
        Console.WriteLine($"Без синхронізації: {unsafeCounter}");
        Console.WriteLine($"Interlocked: {safeCounter}");
    }
}
