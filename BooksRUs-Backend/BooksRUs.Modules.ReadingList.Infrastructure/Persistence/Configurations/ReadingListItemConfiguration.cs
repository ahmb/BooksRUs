using BooksRUs.Modules.ReadingList.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksRUs.Modules.ReadingList.Infrastructure.Persistence.Configurations;

public class ReadingListItemConfiguration : IEntityTypeConfiguration<ReadingListItem>
{
    public void Configure(EntityTypeBuilder<ReadingListItem> b)
    {
        b.ToTable("Items", "readinglist");
        b.HasKey(x => x.Id);
        b.HasIndex(x => new { x.UserId, x.BookId }).IsUnique();
        b.Property(x => x.UserId).HasMaxLength(100).IsRequired();
    }
}
