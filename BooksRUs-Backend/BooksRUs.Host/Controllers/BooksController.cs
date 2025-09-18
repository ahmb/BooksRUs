using BooksRUs.Modules.Catalog.Application.Abstractions.Services;
using BooksRUs.Modules.Catalog.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksRUs.Host.Controllers;

public static class BooksController
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder routes)
    {
        var grp = routes.MapGroup("/api/books")
                        .WithTags("Books");

        grp.MapGet("", async (IBookQueries q, string? search, int page = 1, int size = 20, CancellationToken ct = default) =>
        {
            var items = await q.BrowseAsync(search, page, size, ct);
            return Results.Ok(items.Select(BookDto.From));
        })
        .WithSummary("List or search books")
        .WithDescription("Returns a paged list of books. Use `search` to fuzzy match title/author. `page` is 1-based; default size is 20.")
        .Produces<IEnumerable<BookDto>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        grp.MapGet("{id:guid}", async (IBookQueries q, Guid id, CancellationToken ct) =>
        {
            var book = await q.GetAsync(id, ct);
            return book is null ? Results.NotFound() : Results.Ok(BookDto.From(book));
        })
        .WithSummary("Get a book by id")
        .WithDescription("Returns a single book by its GUID identifier.")
        .Produces<BookDto>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        grp.MapPost("", async (IBookCommands svc, CreateBookRequest req, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(req.Title) || string.IsNullOrWhiteSpace(req.Isbn))
                return Results.ValidationProblem(new Dictionary<string, string[]> { ["title/isbn"] = new[] { "Required" } });

            try
            {
                var created = await svc.CreateAsync(req.Isbn, req.Title, req.Author, req.Year, req.Description, ct);
                return Results.Created($"/api/books/{created.Id}", BookDto.From(created));
            }
            catch (ArgumentException ex)
            {
                var field = ex.ParamName ?? "payload";
                return Results.ValidationProblem(
                    new Dictionary<string, string[]> { [field] = new[] { ex.Message } }
                );
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        })
        .WithSummary("Create a book")
        .WithDescription("Creates a book in the catalog. `isbn` must be unique.")
        .Accepts<CreateBookRequest>("application/json")
        .Produces<BookDto>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return routes;
    }
}
