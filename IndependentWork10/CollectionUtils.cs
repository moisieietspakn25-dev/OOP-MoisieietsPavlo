public static class CollectionUtils
{
    public static List<TResult> Map<T, TResult>(IEnumerable<T> source, Func<T, TResult> selector)
        => source.Select(selector).ToList();

    public static T MaxBy<T, TKey>(IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey>
    {
        using var e = source.GetEnumerator();
        if (!e.MoveNext()) throw new InvalidOperationException("Колекція порожня.");
        var max = e.Current; var maxKey = selector(max);
        while (e.MoveNext())
        {
            var key = selector(e.Current);
            if (key.CompareTo(maxKey) > 0) { max = e.Current; maxKey = key; }
        }
        return max;
    }

    public static List<T> Filter<T>(IEnumerable<T> source, Func<T, bool> predicate)
        => source.Where(predicate).ToList();

    public static void ForEach<T>(IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source) action(item);
    }
}
