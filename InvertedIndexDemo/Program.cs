namespace InvertedIndexDemo;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var documents = LoadTestDocuments();

        var index = new InvertedIndex();

        foreach (var doc in documents)
        {
            index.AddDocument(doc);
        }

        Console.WriteLine("=== INVERTED INDEX DEMO ===");

        Console.WriteLine($"Loaded {index.DocumentCount} documents");
        Console.WriteLine($"Index contains {index.UniqueWords} unique words");

        // Тестування продуктивності
        Console.WriteLine("\n=== PERFORMANCE TEST ===");

        int[] sizes = { 10, 100, 1000, 10000 };

        foreach (int size in sizes)
        {
            var testDocuments = GenerateDocuments(size);

            PerformanceTest.CompareSearchMethods(
                testDocuments,
                "programming"
            );
        }

        // Інтерактивний пошук
        Console.WriteLine("\n=== INTERACTIVE SEARCH ===");

        while (true)
        {
            Console.Write("\nEnter search query (or 'exit'): ");

            string? query = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(query))
                continue;

            if (query.Trim().Equals(
                "exit",
                StringComparison.OrdinalIgnoreCase))
                break;

            string[] words = query.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            );

            // AND пошук
            var results = index.SearchMultipleWords(words);

            Console.WriteLine("\n--- Search Results (AND) ---");

            foreach (var doc in results)
            {
                Console.WriteLine(doc);

                string preview = doc.Content.Substring(
                    0,
                    Math.Min(100, doc.Content.Length)
                );

                Console.WriteLine($"  {preview}...");
            }

            Console.WriteLine($"\nFound {results.Count} documents");

            // OR пошук
            var anyResults = index.SearchAnyWord(words);

            Console.WriteLine("\n--- Search Results (OR) ---");

            foreach (var doc in anyResults)
            {
                Console.WriteLine(doc);
            }

            Console.WriteLine($"Found {anyResults.Count} documents");
        }

        Console.WriteLine("\nProgram finished.");
    }

    // Тестові документи
    static List<Document> LoadTestDocuments()
    {
        return new List<Document>
        {
            new Document(
                1,
                "Introduction to C#",
                "C# is a modern object-oriented programming language developed by Microsoft."
            ),

            new Document(
                2,
                "Java Basics",
                "Java is a popular programming language known for its portability and performance."
            ),

            new Document(
                3,
                "Python Overview",
                "Python is a high-level programming language with simple syntax."
            ),

            new Document(
                4,
                "C# Advanced Features",
                "C# supports LINQ, async/await, and many modern programming paradigms."
            ),

            new Document(
                5,
                "Comparing Languages",
                "C#, Java, and Python are among the most popular programming languages today."
            )
        };
    }

    // Генерація документів
    static List<Document> GenerateDocuments(int count)
    {
        var templates = LoadTestDocuments();

        var documents = new List<Document>();

        for (int i = 0; i < count; i++)
        {
            Document template = templates[i % templates.Count];

            documents.Add(new Document(
                i + 1,
                $"{template.Title} - Document {i + 1}",
                template.Content
            ));
        }

        return documents;
    }
}
