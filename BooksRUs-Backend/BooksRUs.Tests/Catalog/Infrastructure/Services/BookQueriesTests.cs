namespace BooksRUs.Tests.Catalog.Infrastructure.Services;

using System.Linq;
using System.Threading.Tasks;
using BooksRUs.Modules.Catalog.Domain.Entities;
using BooksRUs.Modules.Catalog.Domain.ValueObjects;
using BooksRUs.Modules.Catalog.Infrastructure;
using BooksRUs.Modules.Catalog.Infrastructure.Services;
using BooksRUs.Tests.Common;
using Microsoft.Extensions.Logging.Abstractions;

public class BookQueriesTests
{
    [Fact]
    public async Task Get_and_Browse_work_with_paging()
    {
        using var db = TestDb.NewCatalog();

        db.Books.AddRange(
            new Book(new Isbn("1111111111"), "A book", "Alice", 2000, null),
            new Book(new Isbn("2222222222"), "B book", "Bob", 2001, null),
            new Book(new Isbn("3333333333"), "C book", "Carl", 2002, null)
        );
        await db.SaveChangesAsync();

        var svc = new BookQueries(db, NullLogger<BookQueries>.Instance);

        var secondId = db.Books.AsQueryable().OrderBy(b => b.Title).Skip(1).Select(x => x.Id).First();
        var found = await svc.GetAsync(secondId, default);
        found!.Title.Should().Be("B book");

        var page1 = await svc.BrowseAsync(null, 1, 2, default);
        page1.Should().HaveCount(2);

        var page2 = await svc.BrowseAsync(null, 2, 2, default);
        page2.Should().HaveCount(1);
    }
}
