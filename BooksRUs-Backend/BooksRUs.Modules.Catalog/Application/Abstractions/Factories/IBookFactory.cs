using BooksRUs.Modules.Catalog.Domain.Entities;

namespace BooksRUs.Modules.Catalog.Application.Abstractions.Factories;

public interface IBookFactory
{
    Book Create(string isbn, string title, string author, int year, string? description);
}
