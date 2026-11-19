# Самостійна робота №8 — узагальнений FIFO-кеш

Робота відповідає методичці: створено два проєкти `IndependentWork7` та `IndependentWork7.Tests`. Клас `Cache<TKey,TValue>` використовує `Dictionary<TKey,TValue>` для зберігання пар і `Queue<TKey>` для порядку додавання.

Реалізовано:
- `Cache(int capacity)` — перевірка додатної ємності;
- `Add(key, value)` — додавання та FIFO-витіснення найстарішого ключа;
- `Get(key)` — отримання значення або `KeyNotFoundException`;
- `ContainsKey(key)` та `Count`.

## Команди запуску
```powershell
dotnet run --project IndependentWork7
dotnet test IndependentWork7.Tests
```

Тестами перевіряються додавання, отримання, переповнення FIFO, відсутній ключ, ContainsKey, Count, оновлення наявного ключа та некоректна ємність.
