namespace IndependentWork9
{
    public class FileMetadataCache
    {
        private Dictionary<string, FileMetadata> _cache =
            new Dictionary<string, FileMetadata>(
                StringComparer.OrdinalIgnoreCase);

        public void Add(FileMetadata metadata)
        {
            _cache[metadata.FileName] = metadata;
        }

        public FileMetadata Get(string fileName)
        {
            if (_cache.TryGetValue(fileName, out FileMetadata metadata))
                return metadata;

            throw new KeyNotFoundException("Файл не знайдено!");
        }

        public bool Remove(string fileName)
        {
            return _cache.Remove(fileName);
        }

        public bool Contains(string fileName)
        {
            return _cache.ContainsKey(fileName);
        }
    }
}