namespace BooksRUs.Tests.Catalog.Infrastructure.Services;

using System;
using System.Threading.Tasks;
using BooksRUs.Modules.Catalog.Infrastructure;
using BooksRUs.Modules.Catalog.Infrastructure.Factories;
using BooksRUs.Modules.Catalog.Infrastructure.Services;
using BooksRUs.Tests.Common;
using Microsoft.Extensions.Logging.Abstractions;

public class BookCommandsTests
{
    [Fact]
    public async Task CreateAsync_inserts_and_prevents_duplicate_isbn()
    {
        using var db = TestDb.NewCatalog();
        var factory = new BookFactory();
        var svc = new BookCommands(db, factory, NullLogger<BookCommands>.Instance);

        var created = await svc.CreateAsync("9780140449136", "The Odyssey", "Homer", 1996, "Classic", default);
        created.Id.Should().NotBeEmpty();

        var dup = async () => await svc.CreateAsync("9780140449136", "Other", "Someone", 1999, null, default);
        await dup.Should().ThrowAsync<InvalidOperationException>()
                  .WithMessage("*already exists*");
    }
}
