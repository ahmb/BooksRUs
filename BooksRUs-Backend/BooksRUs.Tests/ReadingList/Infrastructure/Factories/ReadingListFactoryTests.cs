namespace BooksRUs.Tests.ReadingList.Infrastructure.Factories;

using System;
using BooksRUs.Modules.ReadingList.Infrastructure.Factories;

public class ReadingListFactoryTests
{
    [Fact]
    public void Create_item_succeeds()
    {
        var f = new ReadingListFactory();
        var bookId = Guid.NewGuid();
        var it = f.Create("u1", bookId);

        it.Id.Should().NotBeEmpty();
        it.UserId.Should().Be("u1");
        it.BookId.Should().Be(bookId);
        it.AddedAt.Should().BeOnOrBefore(DateTime.UtcNow);
    }

    [Fact]
    public void Create_invalid_throws()
    {
        var f = new ReadingListFactory();
        var a1 = () => f.Create("", Guid.NewGuid());
        var a2 = () => f.Create("u1", Guid.Empty);
        a1.Should().Throw<ArgumentException>();
        a2.Should().Throw<ArgumentException>();
    }
}
