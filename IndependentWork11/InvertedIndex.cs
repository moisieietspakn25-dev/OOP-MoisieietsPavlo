public sealed class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _map = new(StringComparer.OrdinalIgnoreCase);
    public void Add(Document document)
    {
        foreach (var token in TextProcessor.Tokenize(document.Title + " " + document.Content))
        {
            if (!_map.TryGetValue(token, out var ids)) _map[token] = ids = new();
            ids.Add(document.Id);
        }
    }
    public IReadOnlySet<int> SearchOr(params string[] terms)
    {
        var result = new HashSet<int>();
        foreach (var term in terms)
            if (_map.TryGetValue(term, out var ids)) result.UnionWith(ids);
        return result;
    }
    public IReadOnlySet<int> SearchAnd(params string[] terms)
    {
        if (terms.Length == 0) return new HashSet<int>();
        if (!_map.TryGetValue(terms[0], out var first)) return new HashSet<int>();
        var result = first.ToHashSet();
        foreach (var term in terms.Skip(1))
        {
            if (!_map.TryGetValue(term, out var ids)) return new HashSet<int>();
            result.IntersectWith(ids);
        }
        return result;
    }
}
