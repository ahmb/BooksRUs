using Microsoft.EntityFrameworkCore;
using BooksRUs.Modules.Catalog.Infrastructure;
using BooksRUs.Modules.ReadingList.Infrastructure;

namespace BooksRUs.Tests.Common;

public static class TestDb
{
    public static CatalogDbContext NewCatalog(string? name = null)
    {
        var opts = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(name ?? Guid.NewGuid().ToString())
            .Options;
        return new CatalogDbContext(opts);
    }

    public static ReadingListDbContext NewReadingList(string? name = null)
    {
        var opts = new DbContextOptionsBuilder<ReadingListDbContext>()
            .UseInMemoryDatabase(name ?? Guid.NewGuid().ToString())
            .Options;
        return new ReadingListDbContext(opts);
    }
}
