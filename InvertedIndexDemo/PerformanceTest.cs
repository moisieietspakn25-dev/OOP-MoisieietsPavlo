using System.Diagnostics;

namespace InvertedIndexDemo;

public static class PerformanceTest
{
    public static void CompareSearchMethods(
        List<Document> documents,
        string searchWord)
    {
        const int repetitions = 100;

        // Створення індексу
        var index = new InvertedIndex();

        foreach (var doc in documents)
        {
            index.AddDocument(doc);
        }

        string normalizedWord =
            TextProcessor.Tokenize(searchWord).FirstOrDefault() ?? "";

        // Прогрівання
        LinearSearch(documents, normalizedWord);
        index.SearchSingleWord(searchWord);

        Stopwatch sw = Stopwatch.StartNew();

        List<Document> linearResults = new();

        for (int i = 0; i < repetitions; i++)
        {
            linearResults = LinearSearch(documents, normalizedWord);
        }

        sw.Stop();

        long linearTicks = sw.ElapsedTicks;

        // Пошук через індекс
        sw.Restart();

        List<Document> indexResults = new();

        for (int i = 0; i < repetitions; i++)
        {
            indexResults = index.SearchSingleWord(searchWord);
        }

        sw.Stop();

        long indexTicks = sw.ElapsedTicks;

        bool correct = linearResults
            .Select(d => d.Id)
            .OrderBy(id => id)
            .SequenceEqual(
                indexResults.Select(d => d.Id).OrderBy(id => id)
            );

        Console.WriteLine($"Documents: {documents.Count}");
        Console.WriteLine($"Linear search: {linearTicks} ticks");
        Console.WriteLine($"Inverted Index: {indexTicks} ticks");

        Console.WriteLine(
            $"Linear found: {linearResults.Count}, " +
            $"Index found: {indexResults.Count}");

        Console.WriteLine($"Results correct: {correct}");

        Console.WriteLine(new string('-', 50));
    }

    private static List<Document> LinearSearch(
        List<Document> documents,
        string searchWord)
    {
        return documents
            .Where(d => TextProcessor.Tokenize(d.Content)
                .Contains(searchWord))
            .ToList();
    }
}