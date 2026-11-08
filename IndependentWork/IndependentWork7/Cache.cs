
using System;
using System.Collections.Generic;

namespace IndependentWork7
{
    public class Cache<TKey, TValue> where TKey : notnull
    {
        private readonly int _capacity;
        private readonly Dictionary<TKey, TValue> _cache;
        private readonly Queue<TKey> _keysOrder;

        public Cache(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException(
                    "Ємність кешу повинна бути більшою за нуль.");

            _capacity = capacity;
            _cache = new Dictionary<TKey, TValue>();
            _keysOrder = new Queue<TKey>();
        }

        public void Add(TKey key, TValue value)
        {
            // Якщо ключ уже існує, оновлюємо значення
            // без зміни його позиції у FIFO.
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
                return;
            }

            // Якщо кеш заповнений, видаляємо найстаріший елемент.
            if (_cache.Count >= _capacity)
            {
                TKey oldestKey = _keysOrder.Dequeue();
                _cache.Remove(oldestKey);
            }

            _cache.Add(key, value);
            _keysOrder.Enqueue(key);
        }

        public TValue Get(TKey key)
        {
            if (!_cache.TryGetValue(key, out TValue? value))
                throw new KeyNotFoundException(
                    $"Ключ '{key}' не знайдено в кеші.");

            return value;
        }

        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key);
        }

        public int Count
        {
            get { return _cache.Count; }
        }
    }
}
