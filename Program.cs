namespace IndependentWork9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;

            // СЦЕНАРІЙ 1
            Console.WriteLine("=== КЕШ МЕТАДАНИХ ФАЙЛІВ ===");

            FileMetadataCache cache =
                new FileMetadataCache();

            cache.Add(new FileMetadata(
                "report.pdf",
                2500,
                DateTime.Now,
                "ABC123"));

            cache.Add(new FileMetadata(
                "photo.jpg",
                5000,
                DateTime.Now,
                "XYZ789"));

            Console.WriteLine(cache.Get("report.pdf"));

            Console.WriteLine(
                "Чи існує photo.jpg: " +
                cache.Contains("photo.jpg"));

            cache.Remove("photo.jpg");

            Console.WriteLine(
                "Після видалення photo.jpg: " +
                cache.Contains("photo.jpg"));

            // СЦЕНАРІЙ 2
            Console.WriteLine("\n=== УНІКАЛЬНІ ТЕГИ ===");

            TagManager manager = new TagManager();

            manager.AddTag("1", "CSharp");
            manager.AddTag("1", "Programming");
            manager.AddTag("1", "DotNet");

            manager.AddTag("2", "CSharp");
            manager.AddTag("2", "Games");
            manager.AddTag("2", "DotNet");

            Console.WriteLine("Усі теги:");

            foreach (string tag in manager.GetAllUniqueTags())
            {
                Console.WriteLine(tag);
            }

            Console.WriteLine("\nСпільні теги:");

            foreach (string tag in
                manager.FindCommonTags("1", "2"))
            {
                Console.WriteLine(tag);
            }

            Console.WriteLine("\nТільки в першій статті:");

            foreach (string tag in
                manager.FindExclusiveTags("1", "2"))
            {
                Console.WriteLine(tag);
            }

            // СЦЕНАРІЙ 3
            Console.WriteLine("\n=== ПОШУКОВИЙ ІНДЕКС ===");

            SearchIndex index = new SearchIndex();

            index.AddDocument(1,
                "CSharp is a programming language");

            index.AddDocument(2,
                "CSharp and DotNet are popular");

            index.AddDocument(3,
                "Programming with DotNet");

            Console.WriteLine("\nПошук слова CSharp:");

            foreach (int id in index.Search("CSharp"))
            {
                Console.WriteLine("Документ: " + id);
            }

            Console.WriteLine(
                "\nПошук документів зі словами CSharp і DotNet:");

            foreach (int id in
                index.SearchMultiple("CSharp", "DotNet"))
            {
                Console.WriteLine("Документ: " + id);
            }

            Console.WriteLine("\nПрограму завершено!");
        }
    }
}