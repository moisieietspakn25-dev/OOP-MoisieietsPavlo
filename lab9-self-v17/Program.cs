using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main()
    {
        List<Product> products = new()
        {
            new Product(1, "Ноутбук", "Електроніка", 25000m, 5),
            new Product(2, "Миша", "Аксесуари", 500m, 20),
            new Product(3, "Клавіатура", "Аксесуари", 1200m, 15),
            new Product(4, "Монітор", "Електроніка", 8000m, 3),
            new Product(5, "Навушники", "Аксесуари", 1800m, 10),
            new Product(6, "Принтер", "Офісна техніка", 6500m, 2),
            new Product(7, "Флешка", "Носії", 300m, 50),
            new Product(8, "Роутер", "Мережеве обладнання", 2200m, 7),
            new Product(9, "Вебкамера", "Аксесуари", 1500m, 8),
            new Product(10, "Колонки", "Аудіотехніка", 3200m, 4)
        };

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== MAP: Назви товарів ===");
        var names = CollectionUtils.Map(products, p => p.Name);
        CollectionUtils.ForEach(names, Console.WriteLine);

        Console.WriteLine("\n=== FILTER: Товари дорожчі за 2000 грн ===");
        var expensive = CollectionUtils.Filter(products, p => p.Price > 2000m);
        CollectionUtils.ForEach(expensive, p => Console.WriteLine(p));

        Console.WriteLine("\n=== REDUCE: Загальна вартість складу ===");
        decimal totalValue = CollectionUtils.Reduce(
            products, 0m, (total, p) => total + p.Price * p.Quantity);
        Console.WriteLine($"Загальна вартість: {totalValue} грн");

        Console.WriteLine("\n=== FOREACH: Повний список товарів ===");
        CollectionUtils.ForEach(products, Console.WriteLine);

        Console.WriteLine("\n=== ТОВАРИ З КІЛЬКІСТЮ МЕНШЕ 5 ===");
        var lowStock = CollectionUtils.Filter(products, p => p.Quantity < 5);
        CollectionUtils.ForEach(lowStock,
            p => Console.WriteLine($"{p.Name} — {p.Quantity} шт."));

        Console.WriteLine("\nПрограму завершено!");
    }
}
