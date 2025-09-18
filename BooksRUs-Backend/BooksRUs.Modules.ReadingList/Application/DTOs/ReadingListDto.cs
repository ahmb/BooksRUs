using BooksRUs.Modules.ReadingList.Domain.Entities;

namespace BooksRUs.Modules.ReadingList.Application.DTOs;

public record ReadingListItemDto(Guid Id, string UserId, Guid BookId, DateTime AddedAt)
{
    public static ReadingListItemDto From(ReadingListItem x) => new(x.Id, x.UserId, x.BookId, x.AddedAt);
}
public record AddReadingListRequest(string UserId, Guid BookId);
