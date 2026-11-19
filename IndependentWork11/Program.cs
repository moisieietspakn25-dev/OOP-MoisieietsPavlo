
using System;

class Program
{
    static void Main()
    {
        var docs = new[]
        {
            new Document(1, "C Sharp collections",
                "Dictionary and HashSet in dotnet"),

            new Document(2, "LINQ guide",
                "Query collections with LINQ"),

            new Document(3, "Search systems",
                "An inverted index supports fast text search")
        };

        var index = new InvertedIndex();

        foreach (var doc in docs)
        {
            index.Add(doc);
        }

        Console.WriteLine(
            "OR (collections, linq): " +
            string.Join(", ", index.SearchOr("collections", "linq")));

        Console.WriteLine(
            "AND (dictionary, hashset): " +
            string.Join(", ", index.SearchAnd("dictionary", "hashset")));
    }
}
