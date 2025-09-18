namespace BooksRUs.Tests.ReadingList.Infrastructure.Services;

using System;
using System.Threading.Tasks;
using BooksRUs.Modules.ReadingList.Infrastructure;
using BooksRUs.Modules.ReadingList.Infrastructure.Factories;
using BooksRUs.Modules.ReadingList.Infrastructure.Services;
using BooksRUs.Tests.Common;
using Microsoft.Extensions.Logging.Abstractions;

public class ReadingListCommandsTests
{
    [Fact]
    public async Task AddAsync_inserts_and_prevents_duplicate_pair()
    {
        using var db = TestDb.NewReadingList();
        var factory = new ReadingListFactory();
        var svc = new ReadingListCommands(db, factory, NullLogger<ReadingListCommands>.Instance);

        var userId = "u1";
        var bookId = Guid.NewGuid();

        var created = await svc.AddAsync(userId, bookId, default);
        created.UserId.Should().Be(userId);
        created.BookId.Should().Be(bookId);

        var dup = async () => await svc.AddAsync(userId, bookId, default);
        await dup.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("*Already in reading list*");
    }
}
