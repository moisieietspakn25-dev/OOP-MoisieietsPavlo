using IndependentWork7;

var cache = new Cache<string, string>(2);
cache.Add("UA", "Україна");
cache.Add("PL", "Польща");
Console.WriteLine($"UA: {cache.Get("UA")}");
cache.Add("DE", "Німеччина");
Console.WriteLine($"Після переповнення: UA існує — {cache.ContainsKey("UA")}");
Console.WriteLine($"PL існує — {cache.ContainsKey("PL")}");
Console.WriteLine($"DE: {cache.Get("DE")}");
Console.WriteLine($"Кількість елементів: {cache.Count}");
