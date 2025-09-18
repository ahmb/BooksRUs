using BooksRUs.Modules.Catalog.Application.Abstractions.Services;
using BooksRUs.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksRUs.Modules.Catalog.Infrastructure.Services;

public class BookQueries : IBookQueries
{
    private readonly CatalogDbContext _db;
    private readonly ILogger<BookQueries> _logger;

    public BookQueries(CatalogDbContext db, ILogger<BookQueries> logger)
    { _db = db; _logger = logger; }

    public async Task<IReadOnlyList<Book>> BrowseAsync(string? q, int page, int size, CancellationToken ct)
    {
        _logger.LogInformation("BrowseAsync started: q={Q}, page={Page}, size={Size}", q, page, size);
        try
        {
            var query = _db.Books.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(b => EF.Functions.ILike(b.Title, $"%{q}%") ||
                                         EF.Functions.ILike(b.Author, $"%{q}%"));

            var result = await query.OrderBy(b => b.Title)
                                    .Skip((page - 1) * size)
                                    .Take(size)
                                    .ToListAsync(ct);

            _logger.LogInformation("BrowseAsync succeeded: count={Count}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BrowseAsync failed: q={Q}, page={Page}, size={Size}", q, page, size);
            throw;
        }
    }

    public async Task<Book?> GetAsync(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("GetAsync started: id={Id}", id);
        try
        {
            var book = await _db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, ct);
            _logger.LogInformation("GetAsync {Outcome}: id={Id}", book is null ? "not found" : "succeeded", id);
            return book;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAsync unexpected error : id={Id}", id);
            throw;
        }
    }
}
