using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5V17
{
    class Gradebook
    {
        private Dictionary<string, List<int>> _grades;

        public Gradebook()
        {
            _grades = new Dictionary<string, List<int>>();
        }

        // Індексатор
        public List<int> this[string studentName]
        {
            get
            {
                if (_grades.ContainsKey(studentName))
                {
                    return _grades[studentName];
                }

                return new List<int>();
            }
            set
            {
                _grades[studentName] = value;
            }
        }

        // Додавання оцінки
        public void AddGrade(string student, int grade)
        {
            if (grade < 1 || grade > 12)
            {
                Console.WriteLine("Оцінка повинна бути від 1 до 12.");
                return;
            }

            if (!_grades.ContainsKey(student))
            {
                _grades[student] = new List<int>();
            }

            _grades[student].Add(grade);
        }

        // Оператор +
        public static Gradebook operator +(Gradebook first, Gradebook second)
        {
            Gradebook result = new Gradebook();

            foreach (var student in first._grades)
            {
                result._grades[student.Key] =
                    new List<int>(student.Value);
            }

            foreach (var student in second._grades)
            {
                if (!result._grades.ContainsKey(student.Key))
                {
                    result._grades[student.Key] = new List<int>();
                }

                result._grades[student.Key].AddRange(student.Value);
            }

            return result;
        }

        // Оператор ==
        public static bool operator ==(Gradebook first, Gradebook second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (first is null || second is null)
                return false;

            if (first._grades.Count != second._grades.Count)
                return false;

            foreach (var student in first._grades)
            {
                if (!second._grades.ContainsKey(student.Key))
                    return false;

                if (!student.Value.SequenceEqual(
                    second._grades[student.Key]))
                    return false;
            }

            return true;
        }

        // Оператор !=
        public static bool operator !=(Gradebook first, Gradebook second)
        {
            return !(first == second);
        }

        // ToString
        public override string ToString()
        {
            string result = "";

            foreach (var student in _grades)
            {
                result += student.Key + ": ";
                result += string.Join(", ", student.Value);
                result += Environment.NewLine;
            }

            return result;
        }

        // Equals
        public override bool Equals(object obj)
        {
            if (obj is Gradebook)
            {
                return this == (Gradebook)obj;
            }

            return false;
        }

        // GetHashCode
        public override int GetHashCode()
        {
            int hash = 17;

            foreach (var student in _grades)
            {
                hash = hash * 31 + student.Key.GetHashCode();

                foreach (int grade in student.Value)
                {
                    hash = hash * 31 + grade;
                }
            }

            return hash;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Перший журнал
            Gradebook journal1 = new Gradebook();

            journal1.AddGrade("Іваненко", 10);
            journal1.AddGrade("Іваненко", 11);
            journal1.AddGrade("Петренко", 8);

            // Другий журнал
            Gradebook journal2 = new Gradebook();

            journal2.AddGrade("Іваненко", 12);
            journal2.AddGrade("Петренко", 9);
            journal2.AddGrade("Сидоренко", 10);

            Console.WriteLine("=== ЖУРНАЛ 1 ===");
            Console.WriteLine(journal1);

            Console.WriteLine("=== ЖУРНАЛ 2 ===");
            Console.WriteLine(journal2);

            // Читання через індексатор
            Console.WriteLine("Оцінки Іваненка:");

            List<int> grades = journal1["Іваненко"];

            Console.WriteLine(string.Join(", ", grades));

            // Запис через індексатор
            journal1["Новий Студент"] = new List<int> { 7, 8, 9 };

            Console.WriteLine("Оцінки Нового Студента:");
            Console.WriteLine(
                string.Join(", ", journal1["Новий Студент"])
            );

            // Оператор +
            Gradebook journal3 = journal1 + journal2;

            Console.WriteLine();
            Console.WriteLine("=== ОБ'ЄДНАНИЙ ЖУРНАЛ ===");
            Console.WriteLine(journal3);

            // Оператори == та !=
            Console.WriteLine("=== ПОРІВНЯННЯ ===");

            Console.WriteLine(
                "journal1 == journal2: " + (journal1 == journal2)
            );

            Console.WriteLine(
                "journal1 != journal2: " + (journal1 != journal2)
            );

            // Equals
            Console.WriteLine();
            Console.WriteLine("=== EQUALS ===");

            Console.WriteLine(
                "journal1.Equals(journal2): " +
                journal1.Equals(journal2)
            );

            // GetHashCode
            Console.WriteLine();
            Console.WriteLine("=== HASH CODE ===");

            Console.WriteLine(
                "Hash journal1: " + journal1.GetHashCode()
            );

            Console.WriteLine(
                "Hash journal2: " + journal2.GetHashCode()
            );

            Console.WriteLine();
            Console.WriteLine("Програму завершено.");
        }
    }
}