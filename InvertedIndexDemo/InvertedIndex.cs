namespace InvertedIndexDemo;

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index;
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    // Додавання документа
    public void AddDocument(Document doc)
    {
        _documents[doc.Id] = doc;

        var words = TextProcessor.Tokenize(doc.Content).Distinct();

        foreach (string word in words)
        {
            if (!_index.ContainsKey(word))
            {
                _index[word] = new HashSet<int>();
            }

            _index[word].Add(doc.Id);
        }
    }

    // Пошук одного слова
    public List<Document> SearchSingleWord(string word)
    {
        var tokens = TextProcessor.Tokenize(word).ToList();

        if (tokens.Count != 1)
            return new List<Document>();

        if (!_index.TryGetValue(tokens[0], out var ids))
            return new List<Document>();

        return ids
            .Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    // AND — пошук усіх слів
    public List<Document> SearchMultipleWords(params string[] words)
    {
        var tokens = words
            .SelectMany(TextProcessor.Tokenize)
            .Distinct()
            .ToList();

        if (tokens.Count == 0)
            return new List<Document>();

        HashSet<int>? result = null;

        foreach (string word in tokens)
        {
            if (!_index.TryGetValue(word, out var ids))
                return new List<Document>();

            if (result == null)
                result = new HashSet<int>(ids);
            else
                result.IntersectWith(ids);

            if (result.Count == 0)
                return new List<Document>();
        }

        return result!
            .Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    // OR — пошук будь-якого слова
    public List<Document> SearchAnyWord(params string[] words)
    {
        var tokens = words
            .SelectMany(TextProcessor.Tokenize)
            .Distinct();

        HashSet<int> result = new HashSet<int>();

        foreach (string word in tokens)
        {
            if (_index.TryGetValue(word, out var ids))
            {
                result.UnionWith(ids);
            }
        }

        return result
            .Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    public int DocumentCount => _documents.Count;

    public int UniqueWords => _index.Keys.Count;
}