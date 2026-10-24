using lab12v17;
using Xunit;

namespace lab12v17.Tests;

public class RepositoryTests
{
    [Fact]
    public void Add_ShouldAddNewComment()
    {
        var repository = new InMemoryRepository<Comment>();

        var comment = new Comment
        {
            Id = 1,
            Text = "Тест",
            AuthorName = "Павло",
            PostId = 10
        };

        repository.Add(comment);

        Assert.Equal("Тест", repository.GetById(1).Text);
    }
}