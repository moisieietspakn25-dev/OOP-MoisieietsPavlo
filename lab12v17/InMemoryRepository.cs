
namespace lab12v17;

public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new();

    public T GetById(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        return item ?? throw new KeyNotFoundException(
            $"Елемент з ID {id} не знайдено.");
    }

    public IEnumerable<T> GetAll()
    {
        return _items.ToList();
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Any(x => x.Id == item.Id))
        {
            throw new ArgumentException(
                $"Елемент з ID {item.Id} вже існує.");
        }

        _items.Add(item);
    }

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        int index = _items.FindIndex(x => x.Id == item.Id);

        if (index == -1)
        {
            throw new KeyNotFoundException(
                $"Елемент з ID {item.Id} не знайдено.");
        }

        _items[index] = item;
    }

    public void Delete(int id)
    {
        int index = _items.FindIndex(x => x.Id == id);

        if (index == -1)
        {
            throw new KeyNotFoundException(
                $"Елемент з ID {id} не знайдено.");
        }

        _items.RemoveAt(index);
    }
}
