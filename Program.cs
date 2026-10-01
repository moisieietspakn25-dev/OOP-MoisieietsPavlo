
using System;

namespace OOP_lab5v17
{
    class RectangleF
    {
        // Приватні поля
        private float _x;
        private float _y;
        private float _width;
        private float _height;

        // Властивість X
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        // Властивість Y
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        // Властивість Width з перевіркою
        public float Width
        {
            get { return _width; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Ширина не може бути від'ємною.");

                _width = value;
            }
        }

        // Властивість Height з перевіркою
        public float Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Висота не може бути від'ємною.");

                _height = value;
            }
        }

        // Конструктор
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        // Статична властивість Empty
        public static RectangleF Empty
        {
            get { return new RectangleF(0, 0, 0, 0); }
        }

        // Перевантаження оператора ==
        public static bool operator ==(RectangleF a, RectangleF b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.X == b.X &&
                   a.Y == b.Y &&
                   a.Width == b.Width &&
                   a.Height == b.Height;
        }

        // Перевантаження оператора !=
        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return !(a == b);
        }

        // Перевизначення Equals
        public override bool Equals(object obj)
        {
            if (obj is RectangleF rectangle)
                return this == rectangle;

            return false;
        }

        // Перевизначення GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height);
        }

        // Перевизначення ToString
        public override string ToString()
        {
            return $"RectangleF [X={X}, Y={Y}, Width={Width}, Height={Height}]";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Створення об'єктів
            RectangleF rect1 = new RectangleF(10, 20, 100, 50);
            RectangleF rect2 = new RectangleF(10, 20, 100, 50);
            RectangleF rect3 = new RectangleF(5, 15, 80, 40);

            // Виведення інформації
            Console.WriteLine("Прямокутники:");
            Console.WriteLine(rect1);
            Console.WriteLine(rect2);
            Console.WriteLine(rect3);

            // Перевірка властивостей
            Console.WriteLine("\nПеревірка властивостей:");
            Console.WriteLine($"Ширина rect1: {rect1.Width}");
            Console.WriteLine($"Висота rect1: {rect1.Height}");

            // Перевірка валідації
            Console.WriteLine("\nПеревірка валідації:");
            try
            {
                rect1.Width = -10;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            // Використання статичної властивості
            RectangleF empty = RectangleF.Empty;
            Console.WriteLine("\nПорожній прямокутник:");
            Console.WriteLine(empty);

            // Перевірка операторів
            Console.WriteLine("\nПеревірка операторів:");
            Console.WriteLine($"rect1 == rect2: {rect1 == rect2}");
            Console.WriteLine($"rect1 != rect3: {rect1 != rect3}");

            // Перевірка Equals
            Console.WriteLine("\nПеревірка Equals:");
            Console.WriteLine($"rect1.Equals(rect2): {rect1.Equals(rect2)}");
            Console.WriteLine($"rect1.Equals(rect3): {rect1.Equals(rect3)}");

            // Перевірка GetHashCode
            Console.WriteLine("\nХеш-коди:");
            Console.WriteLine($"rect1: {rect1.GetHashCode()}");
            Console.WriteLine($"rect2: {rect2.GetHashCode()}");
            Console.WriteLine($"rect3: {rect3.GetHashCode()}");

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення.");
            Console.ReadKey();
        }
    }
}