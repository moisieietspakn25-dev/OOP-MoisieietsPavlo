
using System;

namespace IndependentWork7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cache<string, string> cache =
                new Cache<string, string>(3);

            Console.WriteLine("=== FIFO CACHE ===");

            cache.Add("1", "Ноутбук");
            cache.Add("2", "Клавіатура");
            cache.Add("3", "Миша");

            Console.WriteLine("Кількість елементів: " + cache.Count);

            Console.WriteLine("Елемент 1: " + cache.Get("1"));
            Console.WriteLine("Елемент 2: " + cache.Get("2"));

            Console.WriteLine("\nДодаємо новий елемент...");

            cache.Add("4", "Монітор");

            Console.WriteLine("Кількість елементів: " + cache.Count);

            Console.WriteLine("Чи є ключ 1: " +
                cache.ContainsKey("1"));

            Console.WriteLine("Чи є ключ 4: " +
                cache.ContainsKey("4"));

            Console.WriteLine("\nПеревірка FIFO:");

            try
            {
                Console.WriteLine(cache.Get("1"));
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine(
                    "Перший елемент видалено з кешу!");
            }

            Console.WriteLine("Новий елемент: " + cache.Get("4"));

            Console.WriteLine("\nПрограма завершена.");
        }
    }
}

