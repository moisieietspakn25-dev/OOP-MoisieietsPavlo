using System.Text.RegularExpressions;
public static class TextProcessor
{
    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase) { "і", "й", "та", "the", "and", "or", "a", "an", "of", "to" };
    public static IEnumerable<string> Tokenize(string text)
        => Regex.Matches(text.ToLowerInvariant(), @"[\p{L}\p{N}]+").Select(m => m.Value).Where(w => !StopWords.Contains(w));
}
