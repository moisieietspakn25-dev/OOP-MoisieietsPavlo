
using System;
using System.Collections.Generic;
using System.IO;

// ==========================================
// Власний виняток ConfigurationLoadException
// ==========================================

public class ConfigurationLoadException : Exception
{
    public string ConfigPath { get; }

    public ConfigurationLoadException()
        : base("Помилка завантаження конфігурації.")
    {
        ConfigPath = string.Empty;
    }

    public ConfigurationLoadException(string message)
        : base(message)
    {
        ConfigPath = string.Empty;
    }

    public ConfigurationLoadException(string message, Exception innerException)
        : base(message, innerException)
    {
        ConfigPath = string.Empty;
    }

    public ConfigurationLoadException(string configPath, bool isPath)
        : base($"Файл конфігурації не знайдено: {configPath}")
    {
        ConfigPath = configPath;
    }
}

// ==========================================
// Узагальнений клас Result<T>
// ==========================================

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(T value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default!, false, error);
    }

    public static Result<T> Failure(Exception ex)
    {
        return new Result<T>(default!, false, ex.Message);
    }
}

// ==========================================
// Клас ConfigurationService
// ==========================================

public class ConfigurationService
{
    private readonly string _configPath;

    public ConfigurationService(string configPath)
    {
        _configPath = configPath;
    }

    // Завантаження налаштування
    // Викидає власний виняток, якщо файл відсутній
    public string LoadSetting(string key)
    {
        if (!File.Exists(_configPath))
        {
            throw new ConfigurationLoadException(_configPath, true);
        }

        string[] lines = File.ReadAllLines(_configPath);

        foreach (string line in lines)
        {
            string[] parts = line.Split('=', 2);

            if (parts.Length == 2 &&
                parts[0].Trim() == key)
            {
                return parts[1].Trim();
            }
        }

        throw new KeyNotFoundException(
            $"Налаштування '{key}' не знайдено.");
    }

    // Збереження налаштування через Result<T>
    public Result<bool> SaveSetting(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result<bool>.Failure(
                "Ключ налаштування не може бути порожнім.");
        }

        if (key.Contains('=') || key.Contains('\n'))
        {
            return Result<bool>.Failure(
                "Ключ містить недопустимі символи.");
        }

        try
        {
            string? directory = Path.GetDirectoryName(_configPath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            List<string> lines = File.Exists(_configPath)
                ? new List<string>(File.ReadAllLines(_configPath))
                : new List<string>();

            bool updated = false;

            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split('=', 2);

                if (parts.Length == 2 &&
                    parts[0].Trim() == key)
                {
                    lines[i] = $"{key}={value}";
                    updated = true;
                    break;
                }
            }

            if (!updated)
            {
                lines.Add($"{key}={value}");
            }

            File.WriteAllLines(_configPath, lines);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex);
        }
    }
}

// ==========================================
// Головний клас програми
// ==========================================

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==================================");
        Console.WriteLine(" ЛАБОРАТОРНА РОБОТА №18");
        Console.WriteLine(" Власні винятки та патерн Result");
        Console.WriteLine(" Варіант №17: Обробка конфігурації");
        Console.WriteLine("==================================\n");

        string configPath = "config.txt";

        ConfigurationService service =
            new ConfigurationService(configPath);

        // 1. Збереження налаштувань
        Console.WriteLine("1. ЗБЕРЕЖЕННЯ НАЛАШТУВАНЬ");

        Result<bool> saveResult =
            service.SaveSetting("Theme", "Dark");

        if (saveResult.IsSuccess)
        {
            Console.WriteLine("Налаштування Theme успішно збережено.");
        }
        else
        {
            Console.WriteLine($"Помилка: {saveResult.Error}");
        }

        Result<bool> saveResult2 =
            service.SaveSetting("Language", "Ukrainian");

        if (saveResult2.IsSuccess)
        {
            Console.WriteLine("Налаштування Language успішно збережено.");
        }
        else
        {
            Console.WriteLine($"Помилка: {saveResult2.Error}");
        }

        // 2. Перевірка некоректного ключа
        Console.WriteLine("\n2. ПЕРЕВІРКА RESULT FAILURE");

        Result<bool> invalidResult =
            service.SaveSetting("", "Test");

        if (invalidResult.IsSuccess)
        {
            Console.WriteLine("Операція успішна.");
        }
        else
        {
            Console.WriteLine($"Помилка: {invalidResult.Error}");
        }

        // 3. Завантаження налаштування
        Console.WriteLine("\n3. ЗАВАНТАЖЕННЯ НАЛАШТУВАНЬ");

        try
        {
            string theme = service.LoadSetting("Theme");
            Console.WriteLine($"Тема програми: {theme}");

            string language = service.LoadSetting("Language");
            Console.WriteLine($"Мова програми: {language}");
        }
        catch (ConfigurationLoadException ex)
        {
            Console.WriteLine($"Власний виняток: {ex.Message}");
            Console.WriteLine($"Шлях: {ex.ConfigPath}");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        // 4. Демонстрація власного винятку
        Console.WriteLine("\n4. ПЕРЕВІРКА ВІДСУТНЬОГО ФАЙЛУ");

        ConfigurationService missingService =
            new ConfigurationService("missing/config.txt");

        try
        {
            string setting = missingService.LoadSetting("Theme");
            Console.WriteLine(setting);
        }
        catch (ConfigurationLoadException ex)
        {
            Console.WriteLine("Перехоплено власний виняток!");
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Файл: {ex.ConfigPath}");
        }

        Console.WriteLine("\n==================================");
        Console.WriteLine("Виконання програми завершено.");
        Console.WriteLine("==================================");
    }
}

