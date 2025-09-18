using BooksRUs.Modules.ReadingList.Application.Abstractions.Factories;
using BooksRUs.Modules.ReadingList.Application.Abstractions.Services;
using BooksRUs.Modules.ReadingList.Infrastructure.Factories;
using BooksRUs.Modules.ReadingList.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksRUs.Modules.ReadingList.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddReadingListModule(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Default");
        services.AddDbContext<ReadingListDbContext>(o => o.UseNpgsql(cs));
        services.AddScoped<IReadingListFactory, ReadingListFactory>();
        services.AddScoped<IReadingListCommands, ReadingListCommands>();
        services.AddScoped<IReadingListQueries, ReadingListQueries>();
        return services;
    }
}
