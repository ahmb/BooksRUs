using BooksRUs.Modules.ReadingList.Domain.Entities;

namespace BooksRUs.Modules.ReadingList.Application.Abstractions.Services;

public interface IReadingListQueries
{
    Task<IReadOnlyList<ReadingListItem>> ListAsync(string userId, CancellationToken ct);
}
