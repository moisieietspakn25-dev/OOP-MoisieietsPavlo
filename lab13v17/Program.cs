
using System;
using System.Collections.Generic;

namespace Lab13v17
{
    public static class FunctionalUtils
    {
        // 1. Map — перетворення елементів списку
        public static List<TResult> Map<T, TResult>(
            IEnumerable<T> items,
            Func<T, TResult> mapper)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            List<TResult> result = new List<TResult>();

            foreach (T item in items)
            {
                result.Add(mapper(item));
            }

            return result;
        }

        // 2. ForEach — виконання дії для кожного елемента
        public static void ForEach<T>(
            IEnumerable<T> items,
            Action<T> action)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in items)
            {
                action(item);
            }
        }

        // 3. All — перевірка всіх елементів за умовою
        public static bool All<T>(
            IEnumerable<T> items,
            Predicate<T> predicate)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (T item in items)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        // 4. OrderBy — сортування за вибраним ключем
        public static List<T> OrderBy<T, TKey>(
            IEnumerable<T> items,
            Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            List<T> result = new List<T>(items);

            // Сортування вставками без LINQ
            for (int i = 1; i < result.Count; i++)
            {
                T current = result[i];
                int j = i - 1;

                while (j >= 0 &&
                    keySelector(result[j]).CompareTo(
                        keySelector(current)) > 0)
                {
                    result[j + 1] = result[j];
                    j--;
                }

                result[j + 1] = current;
            }

            return result;
        }

        // 5. Distinct — отримання унікальних елементів
        public static List<T> Distinct<T>(
            IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            List<T> result = new List<T>();
            HashSet<T> unique = new HashSet<T>();

            foreach (T item in items)
            {
                if (unique.Add(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }

    // Власний клас для тестування
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double AverageGrade { get; set; }

        public Student(string name, int age, double averageGrade)
        {
            Name = name;
            Age = age;
            AverageGrade = averageGrade;
        }

        public override string ToString()
        {
            return $"{Name}, вік: {Age}, середній бал: {AverageGrade:F1}";
        }
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;

            Console.WriteLine("Лабораторна робота №13");
            Console.WriteLine("Варіант 17");
            Console.WriteLine();

            // 1. MAP — робота з int
            List<int> numbers = new List<int>
            {
                1, 2, 3, 4, 5
            };

            List<int> squares = FunctionalUtils.Map(
                numbers, n => n * n);

            Console.WriteLine("1. Map — квадрати чисел:");
            FunctionalUtils.ForEach(
                squares, n => Console.Write(n + " "));
            Console.WriteLine("\n");

            // 2. FOREACH — робота з string
            List<string> names = new List<string>
            {
                "Олена", "Павло", "Ірина"
            };

            Console.WriteLine("2. ForEach — список імен:");
            FunctionalUtils.ForEach(
                names, name => Console.WriteLine(name));
            Console.WriteLine();

            // 3. ALL — перевірка чисел
            bool allPositive = FunctionalUtils.All(
                numbers, n => n > 0);

            Console.WriteLine(
                $"3. All — усі числа додатні: {allPositive}");
            Console.WriteLine();

            // Створення об'єктів Student
            List<Student> students = new List<Student>
            {
                new Student("Павло", 17, 10.2),
                new Student("Олена", 18, 11.4),
                new Student("Ірина", 17, 9.8)
            };

            // All для власного класу
            bool allPassed = FunctionalUtils.All(
                students, s => s.AverageGrade >= 7.0);

            Console.WriteLine(
                $"Усі студенти мають бал не менше 7: {allPassed}");
            Console.WriteLine();

            // 4. ORDERBY — сортування студентів за балом
            List<Student> orderedStudents =
                FunctionalUtils.OrderBy(
                    students, s => s.AverageGrade);

            Console.WriteLine(
                "4. OrderBy — студенти за середнім балом:");

            FunctionalUtils.ForEach(
                orderedStudents,
                student => Console.WriteLine(student));

            Console.WriteLine();

            // Map для власного класу
            List<string> studentNames =
                FunctionalUtils.Map(
                    students, s => s.Name);

            Console.WriteLine("Map — імена студентів:");
            FunctionalUtils.ForEach(
                studentNames,
                name => Console.WriteLine(name));

            Console.WriteLine();

            // 5. DISTINCT — унікальні рядки
            List<string> repeatedNames = new List<string>
            {
                "Павло", "Олена", "Павло",
                "Ірина", "Олена"
            };

            List<string> uniqueNames =
                FunctionalUtils.Distinct(repeatedNames);

            Console.WriteLine("5. Distinct — унікальні імена:");
            FunctionalUtils.ForEach(
                uniqueNames,
                name => Console.WriteLine(name));

            Console.WriteLine();

            // Distinct для чисел
            List<int> repeatedNumbers = new List<int>
            {
                2, 1, 2, 3, 1, 4, 4
            };

            List<int> uniqueNumbers =
                FunctionalUtils.Distinct(repeatedNumbers);

            Console.WriteLine("Distinct — унікальні числа:");
            FunctionalUtils.ForEach(
                uniqueNumbers,
                n => Console.Write(n + " "));

            Console.WriteLine("\n");
            Console.WriteLine("Лабораторну роботу завершено.");
        }
    }
}

