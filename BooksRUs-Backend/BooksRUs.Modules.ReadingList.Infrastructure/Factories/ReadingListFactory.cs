using BooksRUs.Modules.ReadingList.Application.Abstractions.Factories;
using BooksRUs.Modules.ReadingList.Domain.Entities;

namespace BooksRUs.Modules.ReadingList.Infrastructure.Factories;

public class ReadingListFactory : IReadingListFactory
{
    public ReadingListItem Create(string userId, Guid bookId, DateTime? addedAtUtc = null)
        => new ReadingListItem(userId, bookId, addedAtUtc);
}
