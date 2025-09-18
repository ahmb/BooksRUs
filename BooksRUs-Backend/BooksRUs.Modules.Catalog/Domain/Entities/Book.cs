using BooksRUs.Modules.Catalog.Domain.ValueObjects;

namespace BooksRUs.Modules.Catalog.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Isbn Isbn { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int Year { get; private set; }
    public string? Description { get; private set; }

    private Book() { } // EF
    public Book(Isbn isbn, string title, string author, int year, string? description)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title required", nameof(title));
        Isbn = isbn;
        Title = title.Trim();
        Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author.Trim();
        Year = (year is < 0 or > 2100) ? DateTime.UtcNow.Year : year;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }
}
