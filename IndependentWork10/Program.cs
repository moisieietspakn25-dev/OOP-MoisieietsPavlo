
using System;

class Program
{
    static void Main()
    {
        var numbers = new[] { 2, 5, 8, 11, 14 };

        var doubled = CollectionUtils.Map(numbers, x => x * 2);
        var even = CollectionUtils.Filter(numbers, x => x % 2 == 0);
        var largest = CollectionUtils.MaxBy(numbers, x => x);

        Console.WriteLine("Map: " + string.Join(", ", doubled));
        Console.WriteLine("Filter: " + string.Join(", ", even));
        Console.WriteLine("MaxBy: " + largest);

        Console.Write("ForEach: ");
        CollectionUtils.ForEach(numbers, x => Console.Write($"{x} "));
        Console.WriteLine();
    }
}
