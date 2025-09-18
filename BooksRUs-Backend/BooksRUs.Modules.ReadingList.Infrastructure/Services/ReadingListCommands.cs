using BooksRUs.Modules.ReadingList.Application.Abstractions.Factories;
using BooksRUs.Modules.ReadingList.Application.Abstractions.Services;
using BooksRUs.Modules.ReadingList.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksRUs.Modules.ReadingList.Infrastructure.Services;

public class ReadingListCommands : IReadingListCommands
{
    private readonly ReadingListDbContext _db;
    private readonly IReadingListFactory _factory;
    private readonly ILogger<ReadingListCommands> _logger;

    public ReadingListCommands(ReadingListDbContext db, IReadingListFactory factory, ILogger<ReadingListCommands> logger)
    { _db = db; _factory = factory; _logger = logger; }

    public async Task<ReadingListItem> AddAsync(string userId, Guid bookId, CancellationToken ct)
    {
        _logger.LogInformation("AddAsync started: userId={UserId}, bookId={BookId}", userId, bookId);
        try
        {
            var exists = await _db.Items.AsNoTracking().AnyAsync(x => x.UserId == userId && x.BookId == bookId, ct);
            if (exists) throw new InvalidOperationException("Already in reading list.");

            var item = _factory.Create(userId, bookId);
            _db.Items.Add(item);
            var saved = await _db.SaveChangesAsync(ct);

            _logger.LogInformation("AddAsync succeeded: itemId={ItemId}, Changes={Saved}", item.Id, saved);
            return item;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "AddAsync DB error: userId={UserId}, bookId={BookId}", userId, bookId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddAsync unexpected error: userId={UserId}, bookId={BookId}", userId, bookId);
            throw;
        }
    }
}
