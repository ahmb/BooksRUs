namespace BooksRUs.Modules.ReadingList.Domain.Entities;

public class ReadingListItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = default!;
    public Guid BookId { get; private set; }
    public DateTime AddedAt { get; private set; } = DateTime.UtcNow;

    private ReadingListItem() { }
    public ReadingListItem(string userId, Guid bookId, DateTime? addedAtUtc = null)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId required", nameof(userId));
        if (bookId == Guid.Empty) throw new ArgumentException("BookId required", nameof(bookId));
        UserId = userId.Trim();
        BookId = bookId;
        AddedAt = addedAtUtc ?? DateTime.UtcNow;
    }
}
