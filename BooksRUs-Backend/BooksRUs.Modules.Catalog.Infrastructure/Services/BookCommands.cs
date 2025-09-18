using BooksRUs.Modules.Catalog.Application.Abstractions.Factories;
using BooksRUs.Modules.Catalog.Application.Abstractions.Services;
using BooksRUs.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksRUs.Modules.Catalog.Infrastructure.Services;

public class BookCommands : IBookCommands
{
    private readonly CatalogDbContext _db;
    private readonly IBookFactory _factory;
    private readonly ILogger<BookCommands> _logger;

    public BookCommands(CatalogDbContext db, IBookFactory factory, ILogger<BookCommands> logger)
    { _db = db; _factory = factory; _logger = logger; }

    public async Task<Book> CreateAsync(string isbn, string title, string author, int year, string? description, CancellationToken ct)
    {
        _logger.LogInformation("CreateAsync started: ISBN={Isbn}, Title={Title}", isbn, title);
        try
        {
            if (await _db.Books.AnyAsync(b => b.Isbn.Value == isbn, ct))
                throw new InvalidOperationException("ISBN already exists.");

            var entity = _factory.Create(isbn, title, author, year, description);
            _db.Books.Add(entity);
            var saved = await _db.SaveChangesAsync(ct);

            _logger.LogInformation("CreateAsync succeeded: BookId={BookId}, Changes={Saved}", entity.Id, saved);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "CreateAsync DB error: ISBN={Isbn}", isbn);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateAsync unexpected error: ISBN={Isbn}", isbn);
            throw;
        }
    }
}
