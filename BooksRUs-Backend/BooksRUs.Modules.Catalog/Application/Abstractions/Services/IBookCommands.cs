using BooksRUs.Modules.Catalog.Domain.Entities;

namespace BooksRUs.Modules.Catalog.Application.Abstractions.Services;

public interface IBookCommands
{
    Task<Book> CreateAsync(string isbn, string title, string author, int year, string? description, CancellationToken ct);
}
