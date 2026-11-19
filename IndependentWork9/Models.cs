public sealed class FileMetadata
{
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = string.Empty;
}

public sealed class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache = new(StringComparer.OrdinalIgnoreCase);

    public void Add(string path, FileMetadata metadata) => _cache[path] = metadata;
    public FileMetadata Get(string path) => _cache.TryGetValue(path, out var value)
        ? value : throw new KeyNotFoundException($"Метадані для '{path}' не знайдено.");
    public bool Remove(string path) => _cache.Remove(path);
    public bool Contains(string path) => _cache.ContainsKey(path);
}

public sealed class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags = new();

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.TryGetValue(articleId, out var tags))
            _articleTags[articleId] = tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        tags.Add(tag);
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (_articleTags.TryGetValue(articleId, out var tags))
        {
            tags.Remove(tag);
            if (tags.Count == 0) _articleTags.Remove(articleId);
        }
    }

    public HashSet<string> GetAllUniqueTags()
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var tags in _articleTags.Values) result.UnionWith(tags);
        return result;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        var first = _articleTags.TryGetValue(articleId1, out var a) ? a : new HashSet<string>();
        var second = _articleTags.TryGetValue(articleId2, out var b) ? b : new HashSet<string>();
        var result = new HashSet<string>(first, StringComparer.OrdinalIgnoreCase);
        result.IntersectWith(second);
        return result;
    }
}

public sealed class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> _invertedIndex = new(StringComparer.OrdinalIgnoreCase);

    public void AddDocument(int docId, string content)
    {
        var words = content.Split(new[] { ' ', '\t', '\r', '\n', '.', ',', ';', ':', '!', '?', '(', ')', '[', ']' },
            StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            var normalized = word.ToLowerInvariant();
            if (!_invertedIndex.TryGetValue(normalized, out var ids))
                _invertedIndex[normalized] = ids = new HashSet<int>();
            ids.Add(docId);
        }
    }

    public HashSet<int> Search(string word) =>
        _invertedIndex.TryGetValue(word.ToLowerInvariant(), out var ids)
            ? new HashSet<int>(ids) : new HashSet<int>();

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0) return new HashSet<int>();
        var result = Search(words[0]);
        foreach (var word in words.Skip(1))
            result.IntersectWith(Search(word));
        return result;
    }
}
