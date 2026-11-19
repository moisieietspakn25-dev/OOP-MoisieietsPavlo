using IndependentWork7;
using Xunit;

namespace IndependentWork7.Tests;

public class CacheTests
{
    [Fact]
    public void Add_IncreasesCountAndStoresValue()
    {
        var cache = new Cache<string, int>(3);
        cache.Add("one", 1);
        Assert.Equal(1, cache.Count);
        Assert.Equal(1, cache.Get("one"));
    }

    [Fact]
    public void Get_ReturnsStoredValue()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(7, "seven");
        Assert.Equal("seven", cache.Get(7));
    }

    [Fact]
    public void Add_WhenFull_RemovesOldestKey()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");
        cache.Add(3, "three");
        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<string, int>(2);
        Assert.Throws<KeyNotFoundException>(() => cache.Get("missing"));
    }

    [Fact]
    public void ContainsKey_ReportsPresence()
    {
        var cache = new Cache<int, int>(2);
        cache.Add(10, 100);
        Assert.True(cache.ContainsKey(10));
        Assert.False(cache.ContainsKey(20));
    }

    [Fact]
    public void Count_ReportsCurrentNumberOfItems()
    {
        var cache = new Cache<int, int>(3);
        Assert.Equal(0, cache.Count);
        cache.Add(1, 10);
        cache.Add(2, 20);
        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValueWithoutDuplicatingQueueEntry()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "old");
        cache.Add(1, "new");
        cache.Add(2, "two");
        cache.Add(3, "three");
        Assert.False(cache.ContainsKey(1));
        Assert.Equal("two", cache.Get(2));
        Assert.Equal("three", cache.Get(3));
    }

    [Fact]
    public void Constructor_RejectsNonPositiveCapacity()
        => Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, int>(0));
}
