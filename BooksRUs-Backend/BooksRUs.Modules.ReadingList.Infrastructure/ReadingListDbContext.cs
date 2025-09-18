using BooksRUs.Modules.ReadingList.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksRUs.Modules.ReadingList.Infrastructure;

public class ReadingListDbContext(DbContextOptions<ReadingListDbContext> opt) : DbContext(opt)
{
    public DbSet<ReadingListItem> Items => Set<ReadingListItem>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("readinglist");
        b.ApplyConfigurationsFromAssembly(typeof(ReadingListDbContext).Assembly);
    }
}
