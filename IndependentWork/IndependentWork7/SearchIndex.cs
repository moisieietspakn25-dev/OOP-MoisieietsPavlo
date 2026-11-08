using System.Text.RegularExpressions;

namespace IndependentWork9
{
    public class SearchIndex
    {
        private Dictionary<string, HashSet<int>> _invertedIndex =
            new Dictionary<string, HashSet<int>>(
                StringComparer.OrdinalIgnoreCase);

        public void AddDocument(int docId, string content)
        {
            string[] words = Regex.Split(
                content.ToLower(),
                @"[^\p{L}\p{N}]+");

            foreach (string word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                if (!_invertedIndex.ContainsKey(word))
                {
                    _invertedIndex[word] = new HashSet<int>();
                }

                _invertedIndex[word].Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            if (_invertedIndex.ContainsKey(word))
            {
                return new HashSet<int>(_invertedIndex[word]);
            }

            return new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            HashSet<int> result = new HashSet<int>();

            if (words.Length == 0)
                return result;

            result = Search(words[0]);

            for (int i = 1; i < words.Length; i++)
            {
                result.IntersectWith(Search(words[i]));
            }

            return result;
        }
    }
}