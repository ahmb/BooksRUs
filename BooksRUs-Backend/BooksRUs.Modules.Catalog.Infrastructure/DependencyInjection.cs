using BooksRUs.Modules.Catalog.Application.Abstractions.Factories;
using BooksRUs.Modules.Catalog.Application.Abstractions.Services;
using BooksRUs.Modules.Catalog.Infrastructure.Factories;
using BooksRUs.Modules.Catalog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksRUs.Modules.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Default");
        services.AddDbContext<CatalogDbContext>(o => o.UseNpgsql(cs));
        services.AddScoped<IBookFactory, BookFactory>();
        services.AddScoped<IBookCommands, BookCommands>();
        services.AddScoped<IBookQueries, BookQueries>();
        return services;
    }
}
