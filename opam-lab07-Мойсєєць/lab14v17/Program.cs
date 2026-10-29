
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

class Payment
{
    public int PaymentId { get; }
    public int AccountId { get; }
    public decimal Amount { get; }
    public string Description { get; }

    public Payment(int paymentId, int accountId, decimal amount, string description)
    {
        PaymentId = paymentId;
        AccountId = accountId;
        Amount = amount;
        Description = description;
    }

    public override string ToString()
    {
        return $"Платіж №{PaymentId} | Рахунок: {AccountId} | " +
               $"Сума: {Amount:F2} грн | Опис: {Description}";
    }
}

class PaymentManager
{
    private readonly HashSet<int> accounts = new HashSet<int>();
    private readonly Queue<Payment> paymentQueue = new Queue<Payment>();
    private readonly Stack<Payment> paymentHistory = new Stack<Payment>();

    private int nextPaymentId = 1;

    public void AddAccount()
    {
        Console.Write("Введіть ID нового рахунку: ");

        if (!int.TryParse(Console.ReadLine(), out int accountId) ||
            accountId <= 0)
        {
            Console.WriteLine("Помилка: ID має бути додатним цілим числом.");
            return;
        }

        if (accounts.Add(accountId))
        {
            Console.WriteLine($"Рахунок {accountId} успішно додано.");
        }
        else
        {
            Console.WriteLine($"Рахунок {accountId} вже існує.");
        }
    }

    public void ShowAccounts()
    {
        Console.WriteLine("\n--- Список рахунків ---");

        if (accounts.Count == 0)
        {
            Console.WriteLine("Рахунків поки немає.");
            return;
        }

        foreach (int accountId in accounts)
        {
            Console.WriteLine($"ID рахунку: {accountId}");
        }

        Console.WriteLine($"Усього рахунків: {accounts.Count}");
    }

    public void CreatePayment()
    {
        Console.Write("Введіть ID рахунку: ");

        if (!int.TryParse(Console.ReadLine(), out int accountId) ||
            !accounts.Contains(accountId))
        {
            Console.WriteLine("Помилка: рахунок не знайдено.");
            return;
        }

        Console.Write("Введіть суму платежу: ");

        string amountInput = Console.ReadLine() ?? "";

        amountInput = amountInput.Replace(',', '.');

        if (!decimal.TryParse(
                amountInput,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Помилка: сума має бути більшою за нуль.");
            return;
        }

        Console.Write("Введіть опис платежу: ");
        string description = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(description))
        {
            description = "Без опису";
        }

        Payment payment = new Payment(
            nextPaymentId++,
            accountId,
            amount,
            description
        );

        paymentQueue.Enqueue(payment);

        Console.WriteLine("Платіж додано до черги.");
        Console.WriteLine(payment);
    }

    public void ProcessPayment()
    {
        if (paymentQueue.Count == 0)
        {
            Console.WriteLine("Черга платежів порожня.");
            return;
        }

        Payment payment = paymentQueue.Dequeue();
        paymentHistory.Push(payment);

        Console.WriteLine("Платіж успішно оброблено:");
        Console.WriteLine(payment);
    }

    public void ProcessAllPayments()
    {
        if (paymentQueue.Count == 0)
        {
            Console.WriteLine("Немає платежів для обробки.");
            return;
        }

        Console.WriteLine("\n--- Обробка платежів ---");

        while (paymentQueue.Count > 0)
        {
            ProcessPayment();
        }

        Console.WriteLine("Усі платежі з черги оброблено.");
    }

    public void ShowQueue()
    {
        Console.WriteLine("\n--- Черга платежів ---");

        if (paymentQueue.Count == 0)
        {
            Console.WriteLine("Черга порожня.");
            return;
        }

        foreach (Payment payment in paymentQueue)
        {
            Console.WriteLine(payment);
        }

        Console.WriteLine($"Кількість платежів у черзі: {paymentQueue.Count}");
    }

    public void ShowHistory()
    {
        Console.WriteLine("\n--- Історія оброблених платежів ---");

        if (paymentHistory.Count == 0)
        {
            Console.WriteLine("Історія платежів порожня.");
            return;
        }

        foreach (Payment payment in paymentHistory)
        {
            Console.WriteLine(payment);
        }
    }

    public void RefundLastPayment()
    {
        if (paymentHistory.Count == 0)
        {
            Console.WriteLine("Немає платежів для повернення.");
            return;
        }

        Payment payment = paymentHistory.Pop();

        Console.WriteLine("Виконано повернення останнього платежу:");
        Console.WriteLine(payment);
        Console.WriteLine($"Повернуто: {payment.Amount:F2} грн");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        PaymentManager manager = new PaymentManager();

        while (true)
        {
            Console.WriteLine("\n================================");
            Console.WriteLine("   СИСТЕМА УПРАВЛІННЯ ПЛАТЕЖАМИ");
            Console.WriteLine("================================");
            Console.WriteLine("1. Додати рахунок");
            Console.WriteLine("2. Переглянути рахунки");
            Console.WriteLine("3. Створити платіж");
            Console.WriteLine("4. Обробити один платіж");
            Console.WriteLine("5. Обробити всі платежі");
            Console.WriteLine("6. Переглянути чергу");
            Console.WriteLine("7. Переглянути історію");
            Console.WriteLine("8. Повернути останній платіж");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    manager.AddAccount();
                    break;

                case "2":
                    manager.ShowAccounts();
                    break;

                case "3":
                    manager.CreatePayment();
                    break;

                case "4":
                    manager.ProcessPayment();
                    break;

                case "5":
                    manager.ProcessAllPayments();
                    break;

                case "6":
                    manager.ShowQueue();
                    break;

                case "7":
                    manager.ShowHistory();
                    break;

                case "8":
                    manager.RefundLastPayment();
                    break;

                case "0":
                    Console.WriteLine("Завершення роботи програми.");
                    return;

                default:
                    Console.WriteLine("Невірний пункт меню. Спробуйте ще раз.");
                    break;
            }
        }
    }
}

