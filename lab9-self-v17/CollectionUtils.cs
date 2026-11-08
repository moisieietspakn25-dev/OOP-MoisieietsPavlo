using System;
using System.Collections.Generic;

public static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(mapper);

        foreach (TSource item in collection)
            yield return mapper(item);
    }

    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (T item in collection)
            if (predicate(item))
                yield return item;
    }

    public static TResult Reduce<T, TResult>(
        IEnumerable<T> collection,
        TResult initialValue,
        Func<TResult, T, TResult> reducer)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(reducer);

        TResult result = initialValue;
        foreach (T item in collection)
            result = reducer(result, item);

        return result;
    }

    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (T item in collection)
            action(item);
    }
}
