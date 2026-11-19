# Самостійна робота №9 — Dictionary<TKey,TValue> та HashSet<T>

## Запуск
```powershell
dotnet run --project IndependentWork9
```

Програма демонструє три сценарії:
1. `FileMetadataCache` — кеш метаданих файлів через `Dictionary<string, FileMetadata>`; реалізовані Add, Get, Remove, Contains.
2. `TagManager` — теги статей через `Dictionary<string, HashSet<string>>`; реалізовані AddTag, RemoveTag, GetAllUniqueTags, FindCommonTags з множинними операціями.
3. `SearchIndex` — інвертований індекс `Dictionary<string, HashSet<int>>`; реалізовані AddDocument, Search і SearchMultiple (перетин множин).

Порівняльна таблиця, аналіз open-source прикладів і висновки наведені у `REPORT.md`.
