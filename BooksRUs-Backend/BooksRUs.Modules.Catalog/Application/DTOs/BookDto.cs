using BooksRUs.Modules.Catalog.Domain.Entities;

namespace BooksRUs.Modules.Catalog.Application.DTOs;

public record BookDto(Guid Id, string Isbn, string Title, string Author, int Year, string? Description)
{
    public static BookDto From(Book b) => new(b.Id, b.Isbn.Value, b.Title, b.Author, b.Year, b.Description);
}
public record CreateBookRequest(string Isbn, string Title, string Author, int Year, string? Description);
