using BooksRUs.Modules.ReadingList.Application.Abstractions.Services;
using BooksRUs.Modules.ReadingList.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksRUs.Modules.ReadingList.Infrastructure.Services;

public class ReadingListQueries : IReadingListQueries
{
    private readonly ReadingListDbContext _db;
    private readonly ILogger<ReadingListQueries> _logger;

    public ReadingListQueries(ReadingListDbContext db, ILogger<ReadingListQueries> logger)
    { _db = db; _logger = logger; }

    public async Task<IReadOnlyList<ReadingListItem>> ListAsync(string userId, CancellationToken ct)
    {
        _logger.LogInformation("ListAsync started: userId={UserId}", userId);
        try
        {
            var items = await _db.Items.AsNoTracking()
                                       .Where(x => x.UserId == userId)
                                       .OrderByDescending(x => x.AddedAt)
                                       .ToListAsync(ct);

            _logger.LogInformation("ListAsync succeeded: userId={UserId}, count={Count}", userId, items.Count);
            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ListAsync failed: userId={UserId}", userId);
            throw;
        }
    }
}
