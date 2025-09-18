namespace BooksRUs.Tests.Catalog.Infrastructure.Factories;

using System;
using BooksRUs.Modules.Catalog.Infrastructure.Factories;

public class BookFactoryTests
{
    [Fact]
    public void Create_valid_book_succeeds()
    {
        var f = new BookFactory();
        var b = f.Create("9780140449136", "The Odyssey", "Homer", 1996, "Classic");

        b.Id.Should().NotBeEmpty();
        b.Isbn.Value.Should().Be("9780140449136");
        b.Title.Should().Be("The Odyssey");
        b.Author.Should().Be("Homer");
        b.Year.Should().Be(1996);
        b.Description.Should().Be("Classic");
    }

    [Fact]
    public void Create_invalid_isbn_throws()
    {
        var f = new BookFactory();
        var act = () => f.Create("bad", "T", "A", 2000, null);
        act.Should().Throw<ArgumentException>();
    }
}
