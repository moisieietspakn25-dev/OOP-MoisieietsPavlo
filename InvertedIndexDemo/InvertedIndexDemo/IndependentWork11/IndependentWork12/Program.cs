
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace IndependentWork12
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("==========================================");
            Console.WriteLine(" САМОСТІЙНА РОБОТА №12");
            Console.WriteLine(" LINQ vs PLINQ: ПРОДУКТИВНІСТЬ І БЕЗПЕКА");
            Console.WriteLine("==========================================\n");

            int[] sizes =
            {
                1_000_000,
                5_000_000,
                10_000_000
            };

            foreach (int size in sizes)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine($"КІЛЬКІСТЬ ЕЛЕМЕНТІВ: {size:N0}");
                Console.WriteLine("==========================================");

                List<int> numbers = GenerateNumbers(size);

                // Прогрів JIT перед вимірюваннями.
                // Використовуємо невелику частину колекції.
                var warmup = numbers.Take(1000);

                warmup.Where(IsPrime).Select(x => (long)x * x).Sum();
                warmup.AsParallel()
                    .Where(IsPrime)
                    .Select(x => (long)x * x)
                    .Sum();

                // Звичайний LINQ
                Stopwatch linqWatch = Stopwatch.StartNew();

                long linqResult = numbers
                    .Where(IsPrime)
                    .Select(x => (long)x * x)
                    .Sum();

                linqWatch.Stop();

                // Паралельний PLINQ
                Stopwatch plinqWatch = Stopwatch.StartNew();

                long plinqResult = numbers
                    .AsParallel()
                    .Where(IsPrime)
                    .Select(x => (long)x * x)
                    .Sum();

                plinqWatch.Stop();

                Console.WriteLine("\n--- Результати LINQ ---");
                Console.WriteLine($"Сума квадратів простих чисел: {linqResult}");
                Console.WriteLine(
                    $"Час: {linqWatch.Elapsed.TotalMilliseconds:F2} мс");

                Console.WriteLine("\n--- Результати PLINQ ---");
                Console.WriteLine($"Сума квадратів простих чисел: {plinqResult}");
                Console.WriteLine(
                    $"Час: {plinqWatch.Elapsed.TotalMilliseconds:F2} мс");

                Console.WriteLine("\n--- Порівняння ---");

                if (plinqWatch.Elapsed < linqWatch.Elapsed)
                {
                    double speedup =
                        linqWatch.Elapsed.TotalMilliseconds /
                        plinqWatch.Elapsed.TotalMilliseconds;

                    Console.WriteLine(
                        $"PLINQ швидший у {speedup:F2} разів.");
                }
                else
                {
                    double ratio =
                        plinqWatch.Elapsed.TotalMilliseconds /
                        Math.Max(linqWatch.Elapsed.TotalMilliseconds, 0.001);

                    Console.WriteLine(
                        $"LINQ швидший у {ratio:F2} разів.");
                }

                Console.WriteLine(
                    $"Результати однакові: {linqResult == plinqResult}");
            }

            // Демонстрація проблем потокобезпечності
            DemonstrateThreadSafety();

            Console.WriteLine("\n==========================================");
            Console.WriteLine("ДОСЛІДЖЕННЯ ЗАВЕРШЕНО");
            Console.WriteLine("==========================================");
        }

        // Генерація випадкових чисел
        static List<int> GenerateNumbers(int count)
        {
            Random random = new Random(12345);

            List<int> numbers = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                numbers.Add(random.Next(1, 2_000_001));
            }

            return numbers;
        }

        // Перевірка числа на простоту
        static bool IsPrime(int number)
        {
            if (number < 2)
                return false;

            if (number == 2)
                return true;

            if (number % 2 == 0)
                return false;

            for (int i = 3; i <= number / i; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        // Демонстрація побічних ефектів
        static void DemonstrateThreadSafety()
        {
            Console.WriteLine("\n==========================================");
            Console.WriteLine("ПЕРЕВІРКА ПОТОКОБЕЗПЕЧНОСТІ");
            Console.WriteLine("==========================================");

            List<int> numbers = Enumerable
                .Range(1, 1_000_000)
                .ToList();

            int expected = numbers.Count;

            Console.WriteLine($"Очікувана кількість: {expected:N0}");

            // Небезпечний варіант:
            // Кілька потоків одночасно змінюють одну змінну.
            Console.WriteLine("\n--- Небезпечний PLINQ ---");

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                int unsafeCounter = 0;

                numbers
                    .AsParallel()
                    .ForAll(x =>
                    {
                        unsafeCounter++;
                    });

                Console.WriteLine(
                    $"Спроба {attempt}: {unsafeCounter:N0}");

                if (unsafeCounter != expected)
                {
                    Console.WriteLine(
                        "Виявлено помилку потокобезпечності!");
                }
            }

            // Безпечний варіант:
            // Interlocked гарантує атомарне збільшення лічильника.
            Console.WriteLine("\n--- Безпечний PLINQ ---");

            int safeCounter = 0;

            numbers
                .AsParallel()
                .ForAll(x =>
                {
                    Interlocked.Increment(ref safeCounter);
                });

            Console.WriteLine(
                $"Результат: {safeCounter:N0}");

            Console.WriteLine(
                $"Очікуваний результат: {expected:N0}");

            Console.WriteLine(
                $"Коректність: {safeCounter == expected}");
        }
    }
}

