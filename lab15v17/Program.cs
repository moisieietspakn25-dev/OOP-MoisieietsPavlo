
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// Клас викладача
public class Instructor
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Specialization { get; set; } = "";

    public override string ToString()
    {
        return $"{Name} ({Specialization})";
    }
}

// Клас курсу
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int InstructorId { get; set; }
    public int MaxStudents { get; set; }

    public override string ToString()
    {
        return $"{Title} | Максимум студентів: {MaxStudents}";
    }
}

public class Program
{
    // Вимірювання часу виконання запиту
    static List<T> Measure<T>(string queryName, Func<List<T>> query)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        List<T> results = query();

        stopwatch.Stop();

        Console.WriteLine($"\n{queryName}");
        Console.WriteLine($"Кількість результатів: {results.Count}");
        Console.WriteLine($"Час виконання: {stopwatch.Elapsed.TotalMilliseconds:F6} мс");

        return results;
    }

    public static void Main()
    {
        // Колекція викладачів
        List<Instructor> instructors = new List<Instructor>
        {
            new Instructor { Id = 1, Name = "Dr. Smith", Specialization = "Programming" },
            new Instructor { Id = 2, Name = "John Brown", Specialization = "Databases" },
            new Instructor { Id = 3, Name = "Anna Wilson", Specialization = "Mathematics" },
            new Instructor { Id = 4, Name = "Michael Davis", Specialization = "Networks" },
            new Instructor { Id = 5, Name = "Sarah Miller", Specialization = "Web Development" },
            new Instructor { Id = 6, Name = "David Taylor", Specialization = "Cybersecurity" },
            new Instructor { Id = 7, Name = "Emily Anderson", Specialization = "Design" },
            new Instructor { Id = 8, Name = "Robert Thomas", Specialization = "Algorithms" },
            new Instructor { Id = 9, Name = "Lisa Jackson", Specialization = "AI" },
            new Instructor { Id = 10, Name = "James White", Specialization = "Physics" },
            new Instructor { Id = 11, Name = "Olivia Harris", Specialization = "Economics" },
            new Instructor { Id = 12, Name = "William Martin", Specialization = "Management" },
            new Instructor { Id = 13, Name = "Sophia Thompson", Specialization = "Statistics" },
            new Instructor { Id = 14, Name = "Benjamin Garcia", Specialization = "Robotics" },
            new Instructor { Id = 15, Name = "Mia Martinez", Specialization = "English" },
            new Instructor { Id = 16, Name = "Daniel Robinson", Specialization = "Electronics" },
            new Instructor { Id = 17, Name = "Isabella Clark", Specialization = "Biology" },
            new Instructor { Id = 18, Name = "Henry Lewis", Specialization = "Chemistry" },
            new Instructor { Id = 19, Name = "Charlotte Lee", Specialization = "History" },
            new Instructor { Id = 20, Name = "Alexander Walker", Specialization = "Geography" }
        };

        // Колекція курсів
        List<Course> courses = new List<Course>
        {
            new Course { Id = 1, Title = "C# Programming", InstructorId = 1, MaxStudents = 40 },
            new Course { Id = 2, Title = "Advanced C#", InstructorId = 1, MaxStudents = 35 },
            new Course { Id = 3, Title = "Python Basics", InstructorId = 1, MaxStudents = 25 },
            new Course { Id = 4, Title = "SQL Fundamentals", InstructorId = 2, MaxStudents = 45 },
            new Course { Id = 5, Title = "Database Design", InstructorId = 2, MaxStudents = 30 },
            new Course { Id = 6, Title = "Calculus", InstructorId = 3, MaxStudents = 50 },
            new Course { Id = 7, Title = "Linear Algebra", InstructorId = 3, MaxStudents = 35 },
            new Course { Id = 8, Title = "Computer Networks", InstructorId = 4, MaxStudents = 28 },
            new Course { Id = 9, Title = "HTML and CSS", InstructorId = 5, MaxStudents = 40 },
            new Course { Id = 10, Title = "JavaScript", InstructorId = 5, MaxStudents = 32 },
            new Course { Id = 11, Title = "Network Security", InstructorId = 6, MaxStudents = 45 },
            new Course { Id = 12, Title = "Graphic Design", InstructorId = 7, MaxStudents = 20 },
            new Course { Id = 13, Title = "Algorithms", InstructorId = 8, MaxStudents = 55 },
            new Course { Id = 14, Title = "Artificial Intelligence", InstructorId = 9, MaxStudents = 60 },
            new Course { Id = 15, Title = "Physics Basics", InstructorId = 10, MaxStudents = 25 },
            new Course { Id = 16, Title = "Economics", InstructorId = 11, MaxStudents = 35 },
            new Course { Id = 17, Title = "Project Management", InstructorId = 12, MaxStudents = 42 },
            new Course { Id = 18, Title = "Statistics", InstructorId = 13, MaxStudents = 38 },
            new Course { Id = 19, Title = "Robotics", InstructorId = 14, MaxStudents = 48 },
            new Course { Id = 20, Title = "English Language", InstructorId = 15, MaxStudents = 30 }
        };

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("======================================");
        Console.WriteLine("       УПРАВЛІННЯ КУРСАМИ");
        Console.WriteLine("       Лабораторна робота №15");
        Console.WriteLine("       Варіант 17");
        Console.WriteLine("======================================");

        Console.WriteLine($"\nКількість викладачів: {instructors.Count}");
        Console.WriteLine($"Кількість курсів: {courses.Count}");

        // ==========================================
        // ЗАПИТ 1
        // Знайти курси Dr. Smith з MaxStudents > 30
        // ==========================================

        Console.WriteLine("\n========== ЗАПИТ 1 ==========");

        var q1 = Measure(
            "1.1 Query Syntax",
            () => (
                from c in courses
                join i in instructors on c.InstructorId equals i.Id
                where i.Name == "Dr. Smith" && c.MaxStudents > 30
                select c
            ).ToList()
        );

        foreach (var course in q1)
            Console.WriteLine(course);

        var q1Methods = Measure(
            "1.2 Method Syntax",
            () => courses
                .Join(instructors,
                    c => c.InstructorId,
                    i => i.Id,
                    (c, i) => new { Course = c, Instructor = i })
                .Where(x => x.Instructor.Name == "Dr. Smith"
                         && x.Course.MaxStudents > 30)
                .Select(x => x.Course)
                .ToList()
        );

        foreach (var course in q1Methods)
            Console.WriteLine(course);

        // ==========================================
        // ЗАПИТ 2
        // Об'єднати курси з викладачами
        // ==========================================

        Console.WriteLine("\n========== ЗАПИТ 2 ==========");

        var q2 = Measure(
            "2.1 Query Syntax",
            () => (
                from c in courses
                join i in instructors on c.InstructorId equals i.Id
                select new
                {
                    CourseTitle = c.Title,
                    InstructorName = i.Name
                }
            ).ToList()
        );

        foreach (var item in q2)
            Console.WriteLine($"{item.CourseTitle} — {item.InstructorName}");

        var q2Methods = Measure(
            "2.2 Method Syntax",
            () => courses
                .Join(instructors,
                    c => c.InstructorId,
                    i => i.Id,
                    (c, i) => new
                    {
                        CourseTitle = c.Title,
                        InstructorName = i.Name
                    })
                .ToList()
        );

        foreach (var item in q2Methods)
            Console.WriteLine($"{item.CourseTitle} — {item.InstructorName}");

        // ==========================================
        // ЗАПИТ 3
        // Кількість курсів кожного викладача
        // ==========================================

        Console.WriteLine("\n========== ЗАПИТ 3 ==========");

        var q3 = Measure(
            "3.1 Query Syntax",
            () => (
                from i in instructors
                join c in courses on i.Id equals c.InstructorId into instructorCourses
                select new
                {
                    InstructorName = i.Name,
                    CourseCount = instructorCourses.Count()
                }
            ).ToList()
        );

        foreach (var item in q3)
            Console.WriteLine($"{item.InstructorName}: {item.CourseCount} курсів");

        var q3Methods = Measure(
            "3.2 Method Syntax",
            () => instructors
                .GroupJoin(courses,
                    i => i.Id,
                    c => c.InstructorId,
                    (i, instructorCourses) => new
                    {
                        InstructorName = i.Name,
                        CourseCount = instructorCourses.Count()
                    })
                .ToList()
        );

        foreach (var item in q3Methods)
            Console.WriteLine($"{item.InstructorName}: {item.CourseCount} курсів");

        // ==========================================
        // ЗАПИТ 4
        // Курс із найбільшою кількістю студентів
        // ==========================================

        Console.WriteLine("\n========== ЗАПИТ 4 ==========");

        var q4 = Measure(
            "4.1 Query Syntax",
            () => (
                from c in courses
                orderby c.MaxStudents descending
                select c
            ).Take(1).ToList()
        );

        foreach (var course in q4)
            Console.WriteLine(course);

        var q4Methods = Measure(
            "4.2 Method Syntax",
            () => courses
                .OrderByDescending(c => c.MaxStudents)
                .Take(1)
                .ToList()
        );

        foreach (var course in q4Methods)
            Console.WriteLine(course);

        Console.WriteLine("\n======================================");
        Console.WriteLine("Аналіз продуктивності завершено.");
        Console.WriteLine("Усі 4 LINQ-запити виконано.");
        Console.WriteLine("======================================");
    }
}

