```csharp
using System;

class PrinterConnection : IDisposable
{
    private string _printerName;
    private bool _isConnected;
    private bool _disposed = false;

    // Публічні властивості
    public string PrinterName
    {
        get { return _printerName; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _printerName = value;
        }
    }

    public bool IsConnected
    {
        get { return _isConnected; }
    }

    // Конструктор
    public PrinterConnection(string printerName)
    {
        _printerName = printerName;
        _isConnected = true;

        Console.WriteLine($"Підключено до принтера: {_printerName}");
    }

    // Метод друку
    public void Print(string document)
    {
        if (_disposed)
        {
            Console.WriteLine("Помилка: з'єднання вже закрито.");
            return;
        }

        if (_isConnected)
        {
            Console.WriteLine($"Принтер {_printerName} друкує документ: {document}");
        }
        else
        {
            Console.WriteLine("Принтер не підключений.");
        }
    }

    // Захищений віртуальний метод Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Звільнення керованих ресурсів
                Console.WriteLine("Звільнення керованих ресурсів.");
            }

            // Звільнення некерованих ресурсів
            if (_isConnected)
            {
                Console.WriteLine($"З'єднання з принтером {_printerName} закрито.");
                _isConnected = false;
            }

            _disposed = true;
        }
    }

    // Публічний метод Dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Деструктор
    ~PrinterConnection()
    {
        Dispose(false);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Використання using
        Console.WriteLine("=== 1. Використання using ===");

        using (PrinterConnection printer = new PrinterConnection("HP LaserJet"))
        {
            printer.Print("Звіт.docx");
        }

        // 2. Явний виклик Dispose()
        Console.WriteLine("\n=== 2. Явний виклик Dispose() ===");

        PrinterConnection printer2 = new PrinterConnection("Canon Pixma");
        printer2.Print("Лабораторна робота.pdf");
        printer2.Dispose();

        // 3. Виклик деструктора через GC
        Console.WriteLine("\n=== 3. Робота деструктора ===");

        PrinterConnection printer3 = new PrinterConnection("Epson EcoTank");
        printer3.Print("Документ.txt");

        printer3 = null;

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("\nПрограму завершено.");
    }
}
```
