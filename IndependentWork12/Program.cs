
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

record Sale(
    int Id,
    string Product,
    string Region,
    decimal Amount,
    DateTime Date
);

class Program
{
    static void Main()
    {
        // Extract: отримання початкових даних
        var sales = Enumerable.Range(1, 80)
            .Select(i => new Sale(
                i,
                $"Product-{(i % 8) + 1}",
                $"Region-{(i % 4) + 1}",
                decimal.Round((i % 17 + 5) * 12.5m, 2),
                new DateTime(2026, (i % 12) + 1, (i % 27) + 1)
            ))
            .ToList();

        // Transform: нормалізація та фільтрація
        var sw = Stopwatch.StartNew();

        var transformed = sales
            .Where(s => s.Amount > 100)
            .Select(s => s with
            {
                Product = s.Product.Trim().ToUpperInvariant(),
                Region = s.Region.Trim()
            })
            .ToList();

        // Load: агрегація даних
        var byProduct = transformed
            .GroupBy(s => s.Product)
            .Select(g => new
            {
                Product = g.Key,
                Revenue = g.Sum(x => x.Amount),
                Count = g.Count()
            })
            .OrderByDescending(x => x.Revenue)
            .ToList();

        sw.Stop();

        // Виведення результатів
        Console.WriteLine($"Початкових продажів: {sales.Count}");
        Console.WriteLine($"Після трансформації: {transformed.Count}");
        Console.WriteLine($"Час ETL: {sw.ElapsedMilliseconds} мс");

        Console.WriteLine("\nВиторг за товарами:");

        foreach (var row in byProduct)
        {
            Console.WriteLine(
                $"{row.Product}: {row.Revenue:C} ({row.Count} продажів)"
            );
        }
    }
}
