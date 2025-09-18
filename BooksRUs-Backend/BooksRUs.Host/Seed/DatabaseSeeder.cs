using Bogus;
using BooksRUs.Modules.Catalog.Domain.Entities;
using BooksRUs.Modules.Catalog.Infrastructure;
using BooksRUs.Modules.ReadingList.Domain.Entities;
using BooksRUs.Modules.ReadingList.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BooksRUs.Host.Seed;

public static class DatabaseSeeder
{
    public static async Task MigrateAndSeedAsync(this IServiceProvider services, ILogger logger)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;
        var cat = sp.GetRequiredService<CatalogDbContext>();
        var rl = sp.GetRequiredService<ReadingListDbContext>();

        await cat.Database.MigrateAsync();
        await rl.Database.MigrateAsync();

        if (!await cat.Books.AnyAsync())
        {
            logger.LogInformation("Seeding Catalog...");
            var faker = new Faker<Book>()
                .CustomInstantiator(f =>
                    new Book(
                        new(f.Random.ReplaceNumbers("##########")),
                        f.Lorem.Sentence(3, 3),
                        $"{f.Name.FirstName()} {f.Name.LastName()}",
                        f.Random.Int(1950, 2025),
                        f.Lorem.Sentences(2)
                    )
                );
            await cat.Books.AddRangeAsync(faker.Generate(200));
            await cat.SaveChangesAsync();
        }

        if (!await rl.Items.AnyAsync())
        {
            logger.LogInformation("Seeding ReadingList...");
            var bookIds = await cat.Books.AsNoTracking().Select(b => b.Id).Take(50).ToListAsync();
            var users = new[] { "u1-demo", "u2-demo" };
            var now = DateTime.UtcNow;
            var items = new List<ReadingListItem>();
            foreach (var u in users)
                foreach (var bid in bookIds.OrderBy(_ => Guid.NewGuid()).Take(10))
                    items.Add(new ReadingListItem(u, bid, now.AddMinutes(-Random.Shared.Next(0, 20_000))));
            await rl.Items.AddRangeAsync(items);
            await rl.SaveChangesAsync();
        }
        logger.LogInformation("Database seed complete.");
    }
}
