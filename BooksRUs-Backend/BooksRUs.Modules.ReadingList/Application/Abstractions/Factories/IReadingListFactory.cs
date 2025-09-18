using BooksRUs.Modules.ReadingList.Domain.Entities;

namespace BooksRUs.Modules.ReadingList.Application.Abstractions.Factories;

public interface IReadingListFactory
{
    ReadingListItem Create(string userId, Guid bookId, DateTime? addedAtUtc = null);
}
