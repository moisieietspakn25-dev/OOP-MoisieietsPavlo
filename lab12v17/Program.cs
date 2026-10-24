
using lab12v17;

var repository = new InMemoryRepository<Comment>();

repository.Add(new Comment
{
    Id = 1,
    Text = "Чудова публікація!",
    AuthorName = "Павло",
    PostId = 101
});

repository.Add(new Comment
{
    Id = 2,
    Text = "Дякую за інформацію.",
    AuthorName = "Олена",
    PostId = 101
});

Console.WriteLine("Усі коментарі:");

foreach (var comment in repository.GetAll())
{
    Console.WriteLine(comment);
}

Console.WriteLine("\nПошук коментаря з ID 1:");
Console.WriteLine(repository.GetById(1));

repository.Update(new Comment
{
    Id = 1,
    Text = "Оновлений коментар",
    AuthorName = "Павло",
    PostId = 101
});

Console.WriteLine("\nПісля оновлення:");
Console.WriteLine(repository.GetById(1));

repository.Delete(2);

Console.WriteLine("\nПісля видалення коментаря з ID 2:");

foreach (var comment in repository.GetAll())
{
    Console.WriteLine(comment);
}
