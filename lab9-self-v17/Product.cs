public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Product(int id, string name, string category, decimal price, int quantity)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        Quantity = quantity;
    }

    public override string ToString()
    {
        return $"ID: {Id}, Назва: {Name}, Категорія: {Category}, " +
               $"Ціна: {Price} грн, Кількість: {Quantity}";
    }
}
