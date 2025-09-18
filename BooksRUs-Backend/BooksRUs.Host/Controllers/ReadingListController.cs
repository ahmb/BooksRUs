using BooksRUs.Modules.ReadingList.Application.Abstractions.Services;
using BooksRUs.Modules.ReadingList.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksRUs.Host.Controllers;

public static class ReadingListController
{
    public static IEndpointRouteBuilder MapReadingListEndpoints(this IEndpointRouteBuilder routes)
    {
        var grp = routes.MapGroup("/api/reading-list")
                        .WithTags("ReadingList");

        grp.MapGet("", async (IReadingListQueries q, string userId, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Results.ValidationProblem(new Dictionary<string, string[]> { ["userId"] = new[] { "Required" } });

            var items = await q.ListAsync(userId, ct);
            return Results.Ok(items.Select(ReadingListItemDto.From));
        })
        .WithSummary("List a user's reading list")
        .WithDescription("Returns all reading-list items for the specified `userId`.")
        .Produces<IEnumerable<ReadingListItemDto>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        grp.MapPost("", async (IReadingListCommands cmd, AddReadingListRequest req, CancellationToken ct) =>
        {
            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(req.UserId)) errors["userId"] = new[] { "Required" };
            if (req.BookId == Guid.Empty) errors["bookId"] = new[] { "Required" };
            if (errors.Count > 0) return Results.ValidationProblem(errors);

            try
            {
                var created = await cmd.AddAsync(req.UserId, req.BookId, ct);
                return Results.Created($"/api/reading-list/{created.Id}", ReadingListItemDto.From(created));
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        })
        .WithSummary("Add a book to a user's reading list")
        .WithDescription("Adds a (userId, bookId) pair to the reading list if not already present.")
        .Accepts<AddReadingListRequest>("application/json")
        .Produces<ReadingListItemDto>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return routes;
    }
}
