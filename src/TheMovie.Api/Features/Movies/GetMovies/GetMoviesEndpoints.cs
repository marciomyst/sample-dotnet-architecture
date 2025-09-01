using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TheMovie.Application.Movies.Queries.GetMovies;

namespace TheMovie.Api.Features.Movies.GetMovies;

/// <summary>
/// Defines HTTP API endpoint for listing movies with pagination and filters.
/// </summary>
public static class GetMoviesEndpoints
{
    /// <summary>
    /// Maps the HTTP GET endpoint used to retrieve a paginated list of movies.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void MapGetMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/movies", async (
            [AsParameters] GetMoviesRequest request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            GetMoviesQuery query = request.ToQuery();
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result.ToResponse());
        })
        .WithName("GetMovies")
        .WithTags("Movies")
        .WithOpenApi((OpenApiOperation operation) =>
        {
            operation.Summary = "Gets a paginated list of movies.";
            operation.Description = "Retrieves movies optionally filtered by title, release year, and genre. Pagination is validated: pageNumber >= 1 and 1 <= pageSize <= 100.";
            return operation;
        })
        .Produces<GetMoviesResponse>(StatusCodes.Status200OK);
    }
}
