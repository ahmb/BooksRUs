using BooksRUs.Modules.Catalog.Application.Abstractions.Factories;
using BooksRUs.Modules.Catalog.Domain.Entities;
using BooksRUs.Modules.Catalog.Domain.ValueObjects;

namespace BooksRUs.Modules.Catalog.Infrastructure.Factories;

public class BookFactory : IBookFactory
{
    public Book Create(string isbn, string title, string author, int year, string? description)
        => new Book(new Isbn(isbn), title, author, year, description);
}
