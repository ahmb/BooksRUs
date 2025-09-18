using BooksRUs.Modules.Catalog.Domain.Entities;

namespace BooksRUs.Modules.Catalog.Application.Abstractions.Services;

public interface IBookQueries
{
    Task<IReadOnlyList<Book>> BrowseAsync(string? q, int page, int size, CancellationToken ct);
    Task<Book?> GetAsync(Guid id, CancellationToken ct);
}
