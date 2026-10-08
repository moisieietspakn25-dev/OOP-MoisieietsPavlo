using System;

class Plant
{
    private string _name;
    private double _height;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public double Height
    {
        get { return _height; }
        set
        {
            if (value >= 0)
                _height = value;
            else
                _height = 0;
        }
    }

    public Plant(string name, double height)
    {
        Name = name;
        Height = height;
    }

    public virtual void Grow()
    {
        Console.WriteLine($"Рослина {Name} росте.");
    }

    public string GetPlantType()
    {
        return "Це рослина.";
    }
}

class Tree : Plant
{
    private double _trunkDiameter;

    public double TrunkDiameter
    {
        get { return _trunkDiameter; }
        set
        {
            if (value >= 0)
                _trunkDiameter = value;
            else
                _trunkDiameter = 0;
        }
    }

    public Tree(string name, double height, double trunkDiameter)
        : base(name, height)
    {
        TrunkDiameter = trunkDiameter;
    }

    public override void Grow()
    {
        Console.WriteLine($"Дерево {Name} росте. Висота: {Height} м.");
    }

    public void ShedLeaves()
    {
        Console.WriteLine($"Дерево {Name} скидає листя.");
    }

    public new string GetPlantType()
    {
        return "Це дерево.";
    }
}

class Flower : Plant
{
    private string _petalColor;

    public string PetalColor
    {
        get { return _petalColor; }
        set { _petalColor = value; }
    }

    public Flower(string name, double height, string petalColor)
        : base(name, height)
    {
        PetalColor = petalColor;
    }

    public override void Grow()
    {
        Console.WriteLine($"Квітка {Name} росте. Висота: {Height} м.");
    }

    public void Bloom()
    {
        Console.WriteLine($"Квітка {Name} розквітає. Колір: {PetalColor}.");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Лабораторна робота №6 ===");
        Console.WriteLine("Варіант 17: Plant -> Tree -> Flower");
        Console.WriteLine();

        Plant plant = new Plant("Рослина", 0.5);
        Tree tree = new Tree("Дуб", 15, 50);
        Flower flower = new Flower("Троянда", 0.6, "червоний");

        Console.WriteLine("--- Grow() ---");
        plant.Grow();
        tree.Grow();
        flower.Grow();

        Console.WriteLine();

        Console.WriteLine("--- Унікальні методи ---");
        tree.ShedLeaves();
        flower.Bloom();

        Console.WriteLine();

        Console.WriteLine("--- Поліморфізм ---");

        Plant treeAsPlant = new Tree("Береза", 12, 30);
        Plant flowerAsPlant = new Flower("Тюльпан", 0.4, "жовтий");

        treeAsPlant.Grow();
        flowerAsPlant.Grow();

        Console.WriteLine();

        Console.WriteLine("--- Демонстрація new ---");

        Console.WriteLine("Tree:");
        Console.WriteLine(tree.GetPlantType());

        Console.WriteLine("Plant:");
        Plant plantReference = tree;
        Console.WriteLine(plantReference.GetPlantType());

        Console.WriteLine();

        Console.WriteLine("Роботу завершено.");
    }
}