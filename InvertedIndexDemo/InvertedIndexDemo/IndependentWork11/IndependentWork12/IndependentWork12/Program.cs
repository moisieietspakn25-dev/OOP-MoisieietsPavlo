
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace IndependentWork12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("==============================================");
            Console.WriteLine(" САМОСТІЙНА РОБОТА №13");
            Console.WriteLine(" PLINQ: продуктивність та безпека");
            Console.WriteLine("==============================================\n");

            // Розміри колекцій для експериментів
            int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };

            foreach (int size in sizes)
            {
                Console.WriteLine($"\nКІЛЬКІСТЬ ЕЛЕМЕНТІВ: {size:N0}");
                Console.WriteLine("----------------------------------------------");

                List<int> numbers = GenerateNumbers(size);

                // LINQ: послідовна обробка
                Stopwatch stopwatch = Stopwatch.StartNew();

                double[] linqResults = numbers
                    .Where(x => x % 3 == 0)
                    .Select(HeavyCalculation)
                    .ToArray();

                stopwatch.Stop();
                long linqTime = stopwatch.ElapsedMilliseconds;

                Console.WriteLine($"LINQ:  {linqTime} мс");
                Console.WriteLine($"Кількість результатів: {linqResults.Length}");

                // PLINQ: паралельна обробка
                stopwatch.Restart();

                double[] plinqResults = numbers
                    .AsParallel()
                    .Where(x => x % 3 == 0)
                    .Select(HeavyCalculation)
                    .ToArray();

                stopwatch.Stop();
                long plinqTime = stopwatch.ElapsedMilliseconds;

                Console.WriteLine($"PLINQ: {plinqTime} мс");
                Console.WriteLine($"Кількість результатів: {plinqResults.Length}");

                // Перевірка прискорення
                if (plinqTime > 0)
                {
                    double speedup = (double)linqTime / plinqTime;
                    Console.WriteLine($"Прискорення: {speedup:F2}x");
                }

                Console.WriteLine(
                    $"Контрольна сума LINQ: {linqResults.Sum():F2}");

                Console.WriteLine(
                    $"Контрольна сума PLINQ: {plinqResults.Sum():F2}");

                // Звільнення пам'яті після експерименту
                numbers.Clear();
            }

            // Демонстрація проблеми потокобезпечності
            DemonstrateThreadSafety();

            Console.WriteLine("\n==============================================");
            Console.WriteLine(" Дослідження завершено!");
            Console.WriteLine("==============================================");
        }

        // Генерація випадкових чисел
        static List<int> GenerateNumbers(int size)
        {
            Random random = new Random(42);
            List<int> numbers = new List<int>(size);

            for (int i = 0; i < size; i++)
            {
                numbers.Add(random.Next(1, 1_000_001));
            }

            return numbers;
        }

        // Обчислювально інтенсивна операція
        static double HeavyCalculation(int number)
        {
            double result = number;

            for (int i = 0; i < 12; i++)
            {
                result = Math.Sqrt(Math.Abs(result) + 1);
                result = Math.Sin(result) * Math.Cos(result);
                result = Math.Abs(result) + 0.5;
            }

            return result;
        }

        // Дослідження потокобезпечності
        static void DemonstrateThreadSafety()
        {
            Console.WriteLine("\n==============================================");
            Console.WriteLine(" ДОСЛІДЖЕННЯ ПОТОКОБЕЗПЕЧНОСТІ");
            Console.WriteLine("==============================================");

            const int elementCount = 1_000_000;

            List<int> numbers = Enumerable
                .Range(0, elementCount)
                .ToList();

            int expected = numbers.Count(x => x % 2 == 0);

            Console.WriteLine($"\nОчікувана кількість: {expected:N0}");

            // НЕБЕЗПЕЧНИЙ варіант
            Console.WriteLine("\n1. PLINQ без синхронізації:");

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                int unsafeCounter = 0;

                numbers
                    .AsParallel()
                    .ForAll(x =>
                    {
                        if (x % 2 == 0)
                        {
                            // Помилка: одночасна зміна спільної змінної
                            unsafeCounter++;
                        }
                    });

                Console.WriteLine(
                    $"Спроба {attempt}: {unsafeCounter:N0}");
            }

            // БЕЗПЕЧНИЙ варіант
            Console.WriteLine("\n2. PLINQ з Interlocked:");

            int safeCounter = 0;

            numbers
                .AsParallel()
                .ForAll(x =>
                {
                    if (x % 2 == 0)
                    {
                        Interlocked.Increment(ref safeCounter);
                    }
                });

            Console.WriteLine($"Правильний результат: {safeCounter:N0}");

            Console.WriteLine(
                $"Перевірка: {(safeCounter == expected ? "УСПІШНО" : "ПОМИЛКА")}");

            Console.WriteLine("\nПояснення:");
            Console.WriteLine("counter++ не є атомарною операцією.");
            Console.WriteLine("Кілька потоків можуть одночасно змінювати");
            Console.WriteLine("одну змінну, через що частина змін губиться.");
            Console.WriteLine("Interlocked.Increment виконує атомарне");
            Console.WriteLine("збільшення лічильника.");
        }
    }
}

