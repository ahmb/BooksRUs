using BooksRUs.Modules.Catalog.Domain.Entities;
using BooksRUs.Modules.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksRUs.Modules.Catalog.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> b)
    {
        b.ToTable("Books", "catalog");
        b.HasKey(x => x.Id);
        b.Property(x => x.Isbn).HasConversion(v => v.Value, v => new Isbn(v)).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Isbn).IsUnique();
        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Author).HasMaxLength(120).IsRequired();
    }
}
