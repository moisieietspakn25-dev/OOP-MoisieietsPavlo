using System.Text.RegularExpressions;

namespace InvertedIndexDemo;

public static class TextProcessor
{
    private static readonly HashSet<string> StopWords = new()
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    public static IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Enumerable.Empty<string>();

        text = text.ToLowerInvariant();

        return Regex.Matches(
            text,
            @"[\p{L}\p{N}]+(?:[+#][\p{L}\p{N}]*)?"
        ).Select(match => match.Value);
    }

    public static IEnumerable<string> RemoveStopWords(
        IEnumerable<string> words)
    {
        return words.Where(word => !StopWords.Contains(word));
    }
}