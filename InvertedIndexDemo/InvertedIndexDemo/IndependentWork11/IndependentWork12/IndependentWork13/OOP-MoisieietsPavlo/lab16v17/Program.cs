
using System;
using System.Collections.Generic;
using System.Linq;

public class Museum
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Exhibit> Exhibits { get; set; } = new();
}

public class Exhibit
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public List<Artist> Artists { get; set; } = new();
}

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public static class MuseumExtensions
{
    public static bool HasExhibit(
        this Museum museum, string exhibitTitle)
    {
        if (museum == null)
            throw new ArgumentNullException(nameof(museum));

        if (string.IsNullOrWhiteSpace(exhibitTitle))
            return false;

        return museum.Exhibits.Any(e =>
            string.Equals(e.Title, exhibitTitle,
                StringComparison.OrdinalIgnoreCase));
    }
}

public class Program
{
    public static void Main()
    {
        var artists = new List<Artist>
        {
            new() { Id = 1, Name = "Тарас Шевченко" },
            new() { Id = 2, Name = "Іван Айвазовський" },
            new() { Id = 3, Name = "Марія Примаченко" },
            new() { Id = 4, Name = "Казимир Малевич" },
            new() { Id = 5, Name = "Олександр Архипенко" },
            new() { Id = 6, Name = "Катерина Білокур" }
        };

        var museums = new List<Museum>
        {
            new()
            {
                Id = 1,
                Name = "Національний художній музей",
                Exhibits = new()
                {
                    new() { Id = 1, Title = "Катерина",
                        Artists = new() { artists[0] } },
                    new() { Id = 2, Title = "Морський краєвид",
                        Artists = new() { artists[1] } },
                    new() { Id = 3, Title = "Квіти України",
                        Artists = new() { artists[5], artists[2] } }
                }
            },
            new()
            {
                Id = 2,
                Name = "Музей сучасного мистецтва",
                Exhibits = new()
                {
                    new() { Id = 4, Title = "Чорний квадрат",
                        Artists = new() { artists[3] } },
                    new() { Id = 5, Title = "Селяни",
                        Artists = new() { artists[2] } },
                    new() { Id = 6, Title = "Скульптурна композиція",
                        Artists = new() { artists[4] } }
                }
            },
            new()
            {
                Id = 3,
                Name = "Музей української культури",
                Exhibits = new()
                {
                    new() { Id = 7, Title = "Автопортрет",
                        Artists = new() { artists[0] } },
                    new() { Id = 8, Title = "Морська далечінь",
                        Artists = new() { artists[1] } },
                    new() { Id = 9, Title = "Декоративний розпис",
                        Artists = new() { artists[2], artists[5] } }
                }
            },
            new()
            {
                Id = 4,
                Name = "Музей скульптури",
                Exhibits = new()
                {
                    new() { Id = 10, Title = "Жіноча фігура",
                        Artists = new() { artists[4] } },
                    new() { Id = 11, Title = "Композиція № 2",
                        Artists = new() { artists[3] } },
                    new() { Id = 12, Title = "Пейзаж",
                        Artists = new() { artists[5] } }
                }
            }
        };

        Console.OutputEncoding =
            System.Text.Encoding.UTF8;

        Console.WriteLine("ЛАБОРАТОРНА РОБОТА №16");
        Console.WriteLine("Варіант 17");

        // 1. SelectMany
        var uniqueArtists = museums
            .SelectMany(m => m.Exhibits)
            .SelectMany(e => e.Artists)
            .DistinctBy(a => a.Id)
            .OrderBy(a => a.Name)
            .ToList();

        Console.WriteLine("\n1. Унікальні автори:");

        foreach (var artist in uniqueArtists)
            Console.WriteLine(artist.Name);

        // 2. Пагінація
        var allExhibits = museums
            .SelectMany(m => m.Exhibits)
            .OrderBy(e => e.Id)
            .ToList();

        const int pageSize = 3;

        int totalPages = (int)Math.Ceiling(
            allExhibits.Count / (double)pageSize);

        Console.WriteLine("\n2. Пагінація експонатів:");

        for (int page = 1; page <= totalPages; page++)
        {
            var pageItems = allExhibits
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            Console.WriteLine($"\nСторінка {page}:");

            foreach (var exhibit in pageItems)
                Console.WriteLine(
                    $"{exhibit.Id}. {exhibit.Title}");
        }

        // 3. Custom Extension
        Console.WriteLine("\n3. Перевірка HasExhibit:");

        foreach (var museum in museums)
        {
            Console.WriteLine(
                $"{museum.Name}: " +
                (museum.HasExhibit("Чорний квадрат")
                    ? "Так"
                    : "Ні"));
        }

        Console.WriteLine("\nПеревірка без урахування регістру:");

        Console.WriteLine(
            museums[0].HasExhibit("катерина"));

        Console.WriteLine("\nРоботу завершено.");
    }
}

