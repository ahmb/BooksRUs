namespace BooksRUs.Tests.ReadingList.Infrastructure.Services;

using System;
using System.Linq;
using System.Threading.Tasks;
using BooksRUs.Modules.ReadingList.Domain.Entities;
using BooksRUs.Modules.ReadingList.Infrastructure;
using BooksRUs.Modules.ReadingList.Infrastructure.Services;
using BooksRUs.Tests.Common;
using Microsoft.Extensions.Logging.Abstractions;

public class ReadingListQueriesTests
{
    [Fact]
    public async Task ListAsync_returns_items_for_user_sorted_desc()
    {
        using var db = TestDb.NewReadingList();

        var u1 = "u1"; var u2 = "u2";
        var b1 = Guid.NewGuid(); var b2 = Guid.NewGuid(); var b3 = Guid.NewGuid();

        db.Items.AddRange(
            new ReadingListItem(u1, b1, DateTime.UtcNow.AddMinutes(-10)),
            new ReadingListItem(u1, b2, DateTime.UtcNow.AddMinutes(-5)),
            new ReadingListItem(u2, b3, DateTime.UtcNow.AddMinutes(-1))
        );
        await db.SaveChangesAsync();

        var svc = new ReadingListQueries(db, NullLogger<ReadingListQueries>.Instance);
        var items = await svc.ListAsync(u1, default);

        items.Should().HaveCount(2);
        items.First().AddedAt.Should().BeOnOrAfter(items.Last().AddedAt);
        items.All(i => i.UserId == u1).Should().BeTrue();
    }
}
