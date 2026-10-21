
using System;
using System.Collections.Generic;

// Інтерфейс плагіна
interface IPlugin
{
    string Name { get; }
    void Initialize();
    void Execute();
}

// Плагін текстового редактора
class TextEditorPlugin : IPlugin
{
    public string Name => "Текстовий редактор";

    public void Initialize()
    {
        Console.WriteLine($"{Name}: ініціалізацію виконано.");
    }

    public void Execute()
    {
        Console.WriteLine($"{Name}: редагування текстового документа.");
    }
}

// Плагін графічного редактора
class ImageEditorPlugin : IPlugin
{
    public string Name => "Графічний редактор";

    public void Initialize()
    {
        Console.WriteLine($"{Name}: ініціалізацію виконано.");
    }

    public void Execute()
    {
        Console.WriteLine($"{Name}: редагування зображення.");
    }
}

// Абстрактний клас хоста плагінів
abstract class PluginHost
{
    protected List<IPlugin> Plugins = new List<IPlugin>();

    public abstract void LoadPlugin(IPlugin plugin);

    public abstract void RunAllPlugins();

    // Конкретний метод базового класу
    public void DisplayPluginInfo(IPlugin plugin)
    {
        Console.WriteLine($"Назва плагіна: {plugin.Name}");
        Console.WriteLine($"Тип плагіна: {plugin.GetType().Name}");
    }
}

// Простий хост плагінів
class SimplePluginHost : PluginHost
{
    public override void LoadPlugin(IPlugin plugin)
    {
        Plugins.Add(plugin);
        Console.WriteLine($"SimplePluginHost: завантажено {plugin.Name}");
        plugin.Initialize();
    }

    public override void RunAllPlugins()
    {
        Console.WriteLine("\nSimplePluginHost: запуск усіх плагінів.");

        foreach (IPlugin plugin in Plugins)
        {
            plugin.Execute();
        }
    }
}

// Розширений хост плагінів
class AdvancedPluginHost : PluginHost
{
    public override void LoadPlugin(IPlugin plugin)
    {
        if (Plugins.Exists(p => p.Name == plugin.Name))
        {
            Console.WriteLine($"AdvancedPluginHost: плагін {plugin.Name} вже завантажений.");
            return;
        }

        Plugins.Add(plugin);
        Console.WriteLine($"AdvancedPluginHost: завантажено {plugin.Name}");
        plugin.Initialize();
    }

    public override void RunAllPlugins()
    {
        Console.WriteLine("\nAdvancedPluginHost: запуск усіх плагінів.");

        foreach (IPlugin plugin in Plugins)
        {
            try
            {
                DisplayPluginInfo(plugin);
                plugin.Execute();
                Console.WriteLine("Статус: виконання успішне.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка виконання плагіна: {ex.Message}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Колекція об'єктів інтерфейсу
        List<IPlugin> plugins = new List<IPlugin>
        {
            new TextEditorPlugin(),
            new ImageEditorPlugin()
        };

        Console.WriteLine("=== Демонстрація інтерфейсу IPlugin ===");

        foreach (IPlugin plugin in plugins)
        {
            Console.WriteLine($"\nПлагін: {plugin.Name}");
            plugin.Initialize();
            plugin.Execute();
        }

        // Демонстрація простого хоста
        Console.WriteLine("\n=== Простий хост ===");

        PluginHost simpleHost = new SimplePluginHost();

        foreach (IPlugin plugin in plugins)
        {
            simpleHost.LoadPlugin(plugin);
            simpleHost.DisplayPluginInfo(plugin);
        }

        simpleHost.RunAllPlugins();

        // Демонстрація розширеного хоста
        Console.WriteLine("\n=== Розширений хост ===");

        PluginHost advancedHost = new AdvancedPluginHost();

        foreach (IPlugin plugin in plugins)
        {
            advancedHost.LoadPlugin(plugin);
        }

        advancedHost.RunAllPlugins();

        Console.WriteLine("Програму завершено.");
    }
}

