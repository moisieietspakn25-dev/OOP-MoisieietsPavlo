
namespace lab12v17;

public class Comment : IEntity
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public int PostId { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Автор: {AuthorName}, " +
               $"Публікація: {PostId}, Коментар: {Text}";
    }
}
