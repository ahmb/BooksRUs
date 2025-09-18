using BooksRUs.Modules.ReadingList.Domain.Entities;

namespace BooksRUs.Modules.ReadingList.Application.Abstractions.Services;

public interface IReadingListCommands
{
    Task<ReadingListItem> AddAsync(string userId, Guid bookId, CancellationToken ct);
}
