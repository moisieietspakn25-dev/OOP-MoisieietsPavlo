csharp
using System;

class Calculator
{
    public virtual double Calculate(double a, double b)
    {
        return a + b;
    }
}

class ScientificCalculator : Calculator
{
    public override double Calculate(double a, double b)
    {
        return Math.Pow(a, b);
    }

    public double SquareRoot(double number)
    {
        return Math.Sqrt(number);
    }
}

class BasicCalculator : Calculator
{
    public new double Calculate(double a, double b)
    {
        return a - b;
    }

    public double Multiply(double a, double b)
    {
        return a * b;
    }
}

class Program
{
    static void Main()
    {
        Calculator calculator = new Calculator();
        ScientificCalculator scientificCalculator = new ScientificCalculator();
        BasicCalculator basicCalculator = new BasicCalculator();

        Calculator scientificAsBase = scientificCalculator;
        Calculator basicAsBase = basicCalculator;

        Console.WriteLine("=== Демонстрація override та new ===");
        Console.WriteLine();

        Console.WriteLine("--- Об'єкт Calculator ---");
        Console.WriteLine("Calculate(10, 3) = " + calculator.Calculate(10, 3));
        Console.WriteLine();

        Console.WriteLine("--- Виклик через посилання Calculator ---");
        Console.WriteLine("ScientificCalculator через Calculator: " + scientificAsBase.Calculate(2, 3));
        Console.WriteLine("BasicCalculator через Calculator: " + basicAsBase.Calculate(10, 3));
        Console.WriteLine();

        Console.WriteLine("--- Виклик через посилання похідних класів ---");
        Console.WriteLine("ScientificCalculator: " + scientificCalculator.Calculate(2, 3));
        Console.WriteLine("BasicCalculator: " + basicCalculator.Calculate(10, 3));
        Console.WriteLine();

        Console.WriteLine("--- Додаткові методи ---");
        Console.WriteLine("SquareRoot(25) = " + scientificCalculator.SquareRoot(25));
        Console.WriteLine("Multiply(4, 5) = " + basicCalculator.Multiply(4, 5));
        Console.WriteLine();

        Console.WriteLine("--- Явне приведення типів ---");
        Console.WriteLine("ScientificCalculator Calculate(2, 4) = " + ((ScientificCalculator)scientificAsBase).Calculate(2, 4));
        Console.WriteLine("BasicCalculator Calculate(10, 4) = " + ((BasicCalculator)basicAsBase).Calculate(10, 4));
        Console.WriteLine();

        Console.WriteLine("=== Пояснення ===");
        Console.WriteLine("override забезпечує поліморфізм.");
        Console.WriteLine("new приховує метод базового класу.");
    }
}

