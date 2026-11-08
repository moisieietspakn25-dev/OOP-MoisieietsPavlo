
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace IndependentWork11
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal UnitPrice { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string City { get; set; } = "";
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;

            var (sales, products, customers) =
                GenerateTestData();

            Console.WriteLine("=== LINQ ETL АНАЛІТИКА ===");
            Console.WriteLine($"Продажів: {sales.Count}");
            Console.WriteLine($"Товарів: {products.Count}");
            Console.WriteLine($"Клієнтів: {customers.Count}");
            Console.WriteLine();

            RunAndMeasure(
                "1. Найпопулярніші продукти за категоріями",
                () => TopProductsByCategory(sales, products));

            RunAndMeasure(
                "2. Дохід за містами",
                () => RevenueByCity(sales, products, customers));

            RunAndMeasure(
                "3. Продажі за місяцями та роками",
                () => SalesByMonth(sales, products));

            RunAndMeasure(
                "4. ТОП-10 клієнтів",
                () => TopCustomersByRevenue(sales, products, customers));
        }

        // Генерація тестових даних
        static (List<Sale>, List<Product>, List<Customer>)
            GenerateTestData()
        {
            Random random = new Random(12345);

            string[] categories =
            {
                "Електроніка",
                "Одяг",
                "Побутова техніка",
                "Книги",
                "Спорт"
            };

            string[] cities =
            {
                "Рівне", "Київ", "Львів",
                "Одеса", "Харків", "Луцьк",
                "Житомир", "Тернопіль",
                "Дніпро", "Чернівці"
            };

            var products = Enumerable.Range(1, 100)
                .Select(id => new Product
                {
                    Id = id,
                    Name = $"Товар {id:000}",
                    Category = categories[(id - 1) % categories.Length],
                    UnitPrice = random.Next(50, 5001) / 10m
                })
                .ToList();

            var customers = Enumerable.Range(1, 100)
                .Select(id => new Customer
                {
                    Id = id,
                    Name = $"Клієнт {id:000}",
                    City = cities[(id - 1) % cities.Length]
                })
                .ToList();

            var sales = Enumerable.Range(1, 5000)
                .Select(id => new Sale
                {
                    Id = id,
                    ProductId = random.Next(1, 101),
                    Quantity = random.Next(1, 11),
                    SaleDate = new DateTime(2024, 1, 1)
                        .AddDays(random.Next(0, 730)),
                    CustomerId = random.Next(1, 101)
                })
                .ToList();

            return (sales, products, customers);
        }

        // ПАЙПЛАЙН 1
        // Топ-3 товарів у кожній категорії
        static List<string> TopProductsByCategory(
            List<Sale> sales,
            List<Product> products)
        {
            var productTotals = sales
                .GroupBy(s => s.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(s => s.Quantity)
                });

            var result = productTotals
                .Join(products,
                    total => total.ProductId,
                    product => product.Id,
                    (total, product) => new
                    {
                        product.Category,
                        product.Name,
                        total.TotalQuantity
                    })
                .GroupBy(x => x.Category)
                .OrderBy(g => g.Key)
                .SelectMany(g => g
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(3)
                    .Select(x =>
                        $"{g.Key}: {x.Name} — {x.TotalQuantity} од."));

            return result.ToList();
        }

        // ПАЙПЛАЙН 2
        // Загальний дохід за містами
        static List<string> RevenueByCity(
            List<Sale> sales,
            List<Product> products,
            List<Customer> customers)
        {
            var result = sales
                .Join(products,
                    sale => sale.ProductId,
                    product => product.Id,
                    (sale, product) => new { sale, product })
                .Join(customers,
                    item => item.sale.CustomerId,
                    customer => customer.Id,
                    (item, customer) => new
                    {
                        customer.City,
                        Revenue = item.sale.Quantity *
                                  item.product.UnitPrice
                    })
                .GroupBy(x => x.City)
                .Select(g => new
                {
                    City = g.Key,
                    TotalRevenue = g.Sum(x => x.Revenue)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ThenBy(x => x.City)
                .Select(x =>
                    $"{x.City}: {x.TotalRevenue:N2} грн");

            return result.ToList();
        }

        // ПАЙПЛАЙН 3
        // Продажі за місяцями та роками
        static List<string> SalesByMonth(
            List<Sale> sales,
            List<Product> products)
        {
            var result = sales
                .Join(products,
                    sale => sale.ProductId,
                    product => product.Id,
                    (sale, product) => new
                    {
                        sale.SaleDate,
                        sale.Quantity,
                        Revenue = sale.Quantity *
                                  product.UnitPrice
                    })
                .GroupBy(x => new
                {
                    Year = x.SaleDate.Year,
                    Month = x.SaleDate.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Revenue)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .Select(x =>
                    $"{x.Year}-{x.Month:00}: " +
                    $"{x.TotalQuantity} од.; " +
                    $"{x.TotalRevenue:N2} грн");

            return result.ToList();
        }

        // ПАЙПЛАЙН 4
        // ТОП-10 клієнтів за сумою великих покупок
        static List<string> TopCustomersByRevenue(
            List<Sale> sales,
            List<Product> products,
            List<Customer> customers)
        {
            var result = sales
                .Where(s => s.Quantity >= 5)
                .Join(products,
                    sale => sale.ProductId,
                    product => product.Id,
                    (sale, product) => new
                    {
                        sale.CustomerId,
                        sale.Quantity,
                        product.UnitPrice
                    })
                .Join(customers,
                    item => item.CustomerId,
                    customer => customer.Id,
                    (item, customer) => new
                    {
                        customer.Name,
                        customer.City,
                        Revenue = item.Quantity *
                                  item.UnitPrice
                    })
                .GroupBy(x => new
                {
                    x.Name,
                    x.City
                })
                .Select(g => new
                {
                    g.Key.Name,
                    g.Key.City,
                    Total = g.Sum(x => x.Revenue)
                })
                .OrderByDescending(x => x.Total)
                .Take(10)
                .Select((x, index) =>
                    $"{index + 1}. {x.Name} ({x.City}) — {x.Total:N2} грн");

            return result.ToList();
        }

        // Вимірювання часу кожного пайплайну
        static void RunAndMeasure(
            string title,
            Func<List<string>> pipeline)
        {
            Console.WriteLine("\n--- " + title + " ---");

            Stopwatch stopwatch = Stopwatch.StartNew();

            List<string> results = pipeline();

            stopwatch.Stop();

            foreach (string item in results)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(
                $"Час виконання: {stopwatch.Elapsed.TotalMilliseconds:F3} мс");
        }
    }
}

