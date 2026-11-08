namespace InvertedIndexDemo;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Document(int id, string title, string content)
    {
        Id = id;
        Title = title;
        Content = content;
    }

    public override string ToString()
    {
        return $"[{Id}] {Title}";
    }
}