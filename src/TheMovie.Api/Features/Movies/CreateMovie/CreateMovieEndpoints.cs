using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TheMovie.Api.Extensions;

namespace TheMovie.Api.Features.Movies.CreateMovie;

/// <summary>
/// Defines HTTP API endpoints for creating movies.
/// </summary>
/// <remarks>
/// <para>
/// Maps the POST endpoint at <c>/api/movies</c>. On success, returns <c>201 Created</c> with a
/// <see cref="CreateMovieResponse"/> and a Location header pointing to <c>GetMovieById</c>. On failure, maps domain
/// errors to RFC 7807 <see cref="ProblemDetails"/> via <see cref="HttpErrorMapper.ToProblemDetails(TheMovie.Application.Shared.Result)"/>.
/// </para>
/// </remarks>
public static class CreateMovieEndpoint
{
    /// <summary>
    /// Maps the HTTP POST endpoint used to create a new movie.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <remarks>
    /// <para>
    /// Request body example:
    /// <code><![CDATA[
    /// POST /api/movies
    /// Content-Type: application/json
    ///
    /// {
    ///   "title": "Pulp Fiction",
    ///   "synopsis": "The lives of two mob hitmen...",
    ///   "releaseYear": 1994,
    ///   "price": 22.50,
    ///   "genreId": "f4e5e6f7-1234-5678-9abc-def012345678",
    ///   "rating": "R"
    /// }
    /// ]]></code>
    /// </para>
    /// <para>
    /// Successful response (201):
    /// <code><![CDATA[
    /// {
    ///   "id": "815acc2d-2224-4253-b119-3c82e7286c07"
    /// }
    /// ]]></code>
    /// </para>
    /// </remarks>
    public static void MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/movies", async (
            [FromBody] CreateMovieRequest request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            var response = new CreateMovieResponse(result.Value);
            return Results.CreatedAtRoute("GetMovieById", new { id = response.Id }, response);
        })
        .WithName("CreateMovie")
        .WithTags("Movies")
        .WithOpenApi((OpenApiOperation operation) =>
        {
            operation.Summary = "Creates a new movie.";
            operation.Description = "Creates a new movie in the catalog. The request body must contain the movie's details.";
            return operation;
        })
        .Produces<CreateMovieResponse>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}
