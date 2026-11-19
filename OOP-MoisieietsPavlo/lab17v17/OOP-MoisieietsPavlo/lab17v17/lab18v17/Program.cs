
using System;
using System.Collections.Generic;
using System.IO;

public class ConfigurationLoadException : Exception
{
    public string ConfigPath { get; }

    public ConfigurationLoadException()
        : base("Не вдалося завантажити конфігурацію.")
    {
        ConfigPath = string.Empty;
    }

    public ConfigurationLoadException(string configPath)
        : base($"Файл конфігурації не знайдено: {configPath}")
    {
        ConfigPath = configPath;
    }

    public ConfigurationLoadException(string configPath, Exception innerException)
        : base($"Помилка завантаження конфігурації: {configPath}", innerException)
    {
        ConfigPath = configPath;
    }
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
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
        return new Result<T>(default, false, error);
    }

    public static Result<T> Failure(Exception ex)
    {
        return Failure(ex.Message);
    }
}

public class ConfigurationService
{
    private readonly string _configPath;

    private readonly Dictionary<string, string> _settings = new()
    {
        ["AppName"] = "MyApplication",
        ["Version"] = "1.0",
        ["Language"] = "uk-UA"
    };

    public ConfigurationService(string configPath)
    {
        _configPath = configPath;
    }

    public string LoadSetting(string key)
    {
        if (!File.Exists(_configPath))
        {
            throw new ConfigurationLoadException(_configPath);
        }

        if (!_settings.TryGetValue(key, out string? value))
        {
            throw new ConfigurationLoadException(
                _configPath,
                new KeyNotFoundException($"Ключ '{key}' не знайдено."));
        }

        return value;
    }

    public Result<bool> SaveSetting(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result<bool>.Failure(
                "Ключ налаштування не може бути порожнім.");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<bool>.Failure(
                "Значення налаштування не може бути порожнім.");
        }

        try
        {
            _settings[key] = value;

            string[] lines = new string[_settings.Count];

            int index = 0;
            foreach (var setting in _settings)
            {
                lines[index++] = $"{setting.Key}={setting.Value}";
            }

            File.WriteAllLines(_configPath, lines);

            return Result<bool>.Success(true);
        }
        catch (IOException ex)
        {
            return Result<bool>.Failure(ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Result<bool>.Failure(ex);
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string configPath = Path.Combine(
            AppContext.BaseDirectory,
            "config.txt");

        var service = new ConfigurationService(configPath);

        Console.WriteLine("====================================");
        Console.WriteLine(" ЛАБОРАТОРНА РОБОТА №18");
        Console.WriteLine(" Варіант 17 — Обробка конфігурації");
        Console.WriteLine("====================================");

        Console.WriteLine("\n--- 1. Завантаження налаштування ---");

        try
        {
            string setting = service.LoadSetting("AppName");
            Console.WriteLine($"Назва програми: {setting}");
        }
        catch (ConfigurationLoadException ex)
        {
            Console.WriteLine($"Виняток: {ex.Message}");
            Console.WriteLine($"Шлях: {ex.ConfigPath}");
        }

        Console.WriteLine("\n--- 2. Збереження налаштування ---");

        Result<bool> saveResult =
            service.SaveSetting("Theme", "Dark");

        if (saveResult.IsSuccess)
        {
            Console.WriteLine("Налаштування успішно збережено.");
        }
        else
        {
            Console.WriteLine($"Помилка: {saveResult.Error}");
        }

        Console.WriteLine("\n--- 3. Перевірка некоректного ключа ---");

        Result<bool> invalidResult =
            service.SaveSetting("", "Value");

        if (invalidResult.IsSuccess)
        {
            Console.WriteLine("Операція успішна.");
        }
        else
        {
            Console.WriteLine($"Помилка: {invalidResult.Error}");
        }

        Console.WriteLine("\n--- 4. Повторне завантаження ---");

        try
        {
            string setting = service.LoadSetting("Theme");
            Console.WriteLine($"Тема програми: {setting}");
        }
        catch (ConfigurationLoadException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine("\n====================================");
        Console.WriteLine(" Роботу завершено.");
        Console.WriteLine("====================================");
    }
}
