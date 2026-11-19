
using System;
using System.Collections.Generic;
using System.Linq;

// Метадані файлу
public class FileMetadata
{
    public string FileName { get; set; } = "";
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = "";
}

// Кеш метаданих файлів
public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> cache = new();

    public void Add(string key, FileMetadata metadata)
    {
        cache[key] = metadata;
    }

    public bool Contains(string key)
    {
        return cache.ContainsKey(key);
    }

    public FileMetadata Get(string key)
    {
        if (!cache.TryGetValue(key, out var metadata))
            throw new KeyNotFoundException($"Файл {key} не знайдено.");

        return metadata;
    }
}

// Менеджер тегів
public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> tags = new();

    public void AddTag(string articleId, string tag)
    {
        if (!tags.ContainsKey(articleId))
            tags[articleId] = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        tags[articleId].Add(tag);
    }

    public IEnumerable<string> GetAllUniqueTags()
    {
        return tags.Values
            .SelectMany(t => t)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(t => t);
    }

    public IEnumerable<string> FindCommonTags(
        string firstArticle, string secondArticle)
    {
        if (!tags.ContainsKey(firstArticle) ||
            !tags.ContainsKey(secondArticle))
            return Enumerable.Empty<string>();

        return tags[firstArticle]
            .Intersect(tags[secondArticle],
                StringComparer.OrdinalIgnoreCase);
    }
}

// Пошуковий індекс
public class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> index =
        new(StringComparer.OrdinalIgnoreCase);

    public void AddDocument(int id, string content)
    {
        var words = content.Split(
            new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':' },
            StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            if (!index.ContainsKey(word))
                index[word] = new HashSet<int>();

            index[word].Add(id);
        }
    }

    public IEnumerable<int> Search(string word)
    {
        if (index.TryGetValue(word, out var documents))
            return documents.OrderBy(id => id);

        return Enumerable.Empty<int>();
    }

    public IEnumerable<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0)
            return Enumerable.Empty<int>();

        var results = Search(words[0]).ToHashSet();

        foreach (var word in words.Skip(1))
        {
            results.IntersectWith(Search(word));
        }

        return results.OrderBy(id => id);
    }
}

// Головна програма
class Program
{
    static void Main()
    {
        var metadataCache = new FileMetadataCache();

        metadataCache.Add("report.pdf", new FileMetadata
        {
            FileName = "report.pdf",
            Size = 2048,
            LastModified = DateTime.Now,
            Hash = "a1b2c3"
        });

        Console.WriteLine(
            $"Файл у кеші: {metadataCache.Contains("report.pdf")}");

        var file = metadataCache.Get("report.pdf");

        Console.WriteLine(
            $"Метадані: {file.FileName}, {file.Size} байт");

        var tags = new TagManager();

        tags.AddTag("article-1", "CSharp");
        tags.AddTag("article-1", "Collections");
        tags.AddTag("article-2", "collections");
        tags.AddTag("article-2", "LINQ");

        Console.WriteLine(
            "Унікальні теги: " +
            string.Join(", ", tags.GetAllUniqueTags()));

        Console.WriteLine(
            "Спільні теги: " +
            string.Join(", ",
                tags.FindCommonTags("article-1", "article-2")));

        var search = new SearchIndex();

        search.AddDocument(1, "CSharp Dictionary HashSet collections");
        search.AddDocument(2, "LINQ collections and Dictionary");
        search.AddDocument(3, "HashSet for unique collections");

        Console.WriteLine(
            "Пошук Dictionary: " +
            string.Join(", ", search.Search("dictionary")));

        Console.WriteLine(
            "Пошук Dictionary AND collections: " +
            string.Join(", ",
                search.SearchMultiple("dictionary", "collections")));
    }
}
