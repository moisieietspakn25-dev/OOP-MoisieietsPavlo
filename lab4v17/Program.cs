using System;

namespace Lab4V17
{
    public class RectangleF
    {
        private float _x;
        private float _y;
        private float _width;
        private float _height;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

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

        public RectangleF()
        {
            _x = 0;
            _y = 0;
            _width = 0;
            _height = 0;
        }

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static RectangleF Empty
        {
            get
            {
                return new RectangleF(0, 0, 0, 0);
            }
        }

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

        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return "RectangleF(X=" + X +
                   ", Y=" + Y +
                   ", Width=" + Width +
                   ", Height=" + Height + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj is RectangleF)
            {
                RectangleF other = (RectangleF)obj;
                return this == other;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            RectangleF rectangle1 = new RectangleF(10, 20, 100, 50);
            RectangleF rectangle2 = new RectangleF(10, 20, 100, 50);
            RectangleF rectangle3 = new RectangleF(5, 10, 80, 40);

            Console.WriteLine("Лабораторна робота №4");
            Console.WriteLine("Варіант 17 - RectangleF");
            Console.WriteLine();

            Console.WriteLine("Прямокутники:");
            Console.WriteLine("rectangle1: " + rectangle1);
            Console.WriteLine("rectangle2: " + rectangle2);
            Console.WriteLine("rectangle3: " + rectangle3);

            Console.WriteLine();
            Console.WriteLine("Властивості rectangle1:");
            Console.WriteLine("X = " + rectangle1.X);
            Console.WriteLine("Y = " + rectangle1.Y);
            Console.WriteLine("Width = " + rectangle1.Width);
            Console.WriteLine("Height = " + rectangle1.Height);

            Console.WriteLine();
            Console.WriteLine("Статична властивість Empty:");
            Console.WriteLine(RectangleF.Empty);

            Console.WriteLine();
            Console.WriteLine("Оператори == та !=:");
            Console.WriteLine("rectangle1 == rectangle2: " + (rectangle1 == rectangle2));
            Console.WriteLine("rectangle1 != rectangle3: " + (rectangle1 != rectangle3));

            Console.WriteLine();
            Console.WriteLine("Equals:");
            Console.WriteLine("rectangle1.Equals(rectangle2): " +
                              rectangle1.Equals(rectangle2));
            Console.WriteLine("rectangle1.Equals(rectangle3): " +
                              rectangle1.Equals(rectangle3));

            Console.WriteLine();
            Console.WriteLine("GetHashCode:");
            Console.WriteLine(rectangle1.GetHashCode());

            Console.WriteLine();
            Console.WriteLine("Перевірка валідації:");

            try
            {
                rectangle1.Width = -10;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }

            try
            {
                rectangle1.Height = -5;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Програму завершено.");
        }
    }
}