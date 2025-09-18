using BooksRUs.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksRUs.Modules.Catalog.Infrastructure;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> opt) : DbContext(opt)
{
    public DbSet<Book> Books => Set<Book>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("catalog");
        b.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
