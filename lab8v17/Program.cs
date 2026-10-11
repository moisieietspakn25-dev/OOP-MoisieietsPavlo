
using System;
using System.Collections.Generic;

class Alert
{
    public string Message { get; set; }

    public Alert(string message)
    {
        Message = message;
    }

    public virtual void Trigger()
    {
        Console.WriteLine($"Сповіщення: {Message}");
    }
}

class EmailAlert : Alert
{
    public string RecipientEmail { get; set; }

    public EmailAlert(string message, string recipientEmail)
        : base(message)
    {
        RecipientEmail = recipientEmail;
    }

    public override void Trigger()
    {
        Console.WriteLine($"[Email] {RecipientEmail}: {Message}");
    }
}

class SMSAlert : Alert
{
    public string PhoneNumber { get; set; }

    public SMSAlert(string message, string phoneNumber)
        : base(message)
    {
        PhoneNumber = phoneNumber;
    }

    public override void Trigger()
    {
        Console.WriteLine($"[SMS] {PhoneNumber}: {Message}");
    }
}

class PushAlert : Alert
{
    public string AppId { get; set; }

    public PushAlert(string message, string appId)
        : base(message)
    {
        AppId = appId;
    }

    public override void Trigger()
    {
        Console.WriteLine($"[Push] {AppId}: {Message}");
    }
}

class Program
{
    static void Main()
    {
        List<Alert> alerts = new List<Alert>
        {
            new EmailAlert("Замовлення оформлено", "user@example.com"),
            new SMSAlert("Код підтвердження: 1234", "+380501234567"),
            new PushAlert("Нове повідомлення", "APP-001")
        };

        Console.WriteLine("=== Спрацьовані сповіщення ===");

        foreach (Alert alert in alerts)
        {
            alert.Trigger();
        }

        Console.WriteLine($"Усього сповіщень: {alerts.Count}");
    }
}
