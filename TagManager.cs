namespace IndependentWork9
{
    public class TagManager
    {
        private Dictionary<string, HashSet<string>> _articleTags =
            new Dictionary<string, HashSet<string>>();

        public void AddTag(string articleId, string tag)
        {
            if (!_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId] =
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);
            }

            _articleTags[articleId].Add(tag);
        }

        public void RemoveTag(string articleId, string tag)
        {
            if (_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId].Remove(tag);
            }
        }

        public HashSet<string> GetAllUniqueTags()
        {
            HashSet<string> result =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (var tags in _articleTags.Values)
            {
                result.UnionWith(tags);
            }

            return result;
        }

        public HashSet<string> FindCommonTags(
            string articleId1, string articleId2)
        {
            HashSet<string> result =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            if (_articleTags.ContainsKey(articleId1))
                result.UnionWith(_articleTags[articleId1]);

            if (_articleTags.ContainsKey(articleId2))
                result.IntersectWith(_articleTags[articleId2]);
            else
                result.Clear();

            return result;
        }

        public HashSet<string> FindExclusiveTags(
            string articleId1, string articleId2)
        {
            HashSet<string> result =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            if (_articleTags.ContainsKey(articleId1))
                result.UnionWith(_articleTags[articleId1]);

            if (_articleTags.ContainsKey(articleId2))
                result.ExceptWith(_articleTags[articleId2]);

            return result;
        }
    }
}