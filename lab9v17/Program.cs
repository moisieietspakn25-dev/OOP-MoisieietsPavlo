
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

abstract class Validator
{
    public string Input { get; }

    protected Validator(string input)
    {
        Input = input;
    }

    public abstract bool Validate(string input);
}

class EmailValidator : Validator
{
    public EmailValidator(string input) : base(input) { }

    public override bool Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Email не може бути порожнім.");

        return Regex.IsMatch(
            input,
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"
        );
    }
}

class PhoneValidator : Validator
{
    public string CountryCode { get; }

    public PhoneValidator(string input, string countryCode = "+380")
        : base(input)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Код країни не може бути порожнім.");

        CountryCode = countryCode;
    }

    public override bool Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Телефон не може бути порожнім.");

        string digits = Regex.Replace(input, @"[\s()-]", "");

        if (digits.StartsWith("0") && CountryCode == "+380")
            digits = CountryCode + digits.Substring(1);

        return Regex.IsMatch(
            digits,
            @"^\+380\d{9}$"
        );
    }
}

class PasswordValidator : Validator
{
    public int MinLength { get; }

    public PasswordValidator(string input, int minLength = 8)
        : base(input)
    {
        if (minLength < 1)
            throw new ArgumentException(
                "Мінімальна довжина має бути більшою за нуль."
            );

        MinLength = minLength;
    }

    public override bool Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Пароль не може бути порожнім.");

        return input.Length >= MinLength
            && Regex.IsMatch(input, @"[A-Z]")
            && Regex.IsMatch(input, @"[a-z]")
            && Regex.IsMatch(input, @"\d");
    }
}

class NotificationService
{
    public void SendAll(List<Validator> items)
    {
        foreach (Validator item in items)
        {
            try
            {
                bool result = item.Validate(item.Input);

                Console.WriteLine(
                    $"{item.GetType().Name}: " +
                    (result ? "Дані коректні" : "Дані некоректні")
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"{item.GetType().Name}: Помилка — {ex.Message}"
                );
            }
        }
    }
}

class Program
{
    static void Main()
    {
        List<Validator> validators = new List<Validator>
        {
            new EmailValidator("student@example.com"),
            new EmailValidator("wrong-email"),
            new PhoneValidator("+380501234567"),
            new PhoneValidator("0501234567"),
            new PasswordValidator("StrongPass123"),
            new PasswordValidator("123"),
            new EmailValidator("")
        };

        NotificationService service = new NotificationService();

        Console.WriteLine("=== СИСТЕМА ВАЛІДАЦІЇ ДАНИХ ===\n");

        service.SendAll(validators);

        Console.WriteLine("\nПеревірку завершено.");
    }
}
