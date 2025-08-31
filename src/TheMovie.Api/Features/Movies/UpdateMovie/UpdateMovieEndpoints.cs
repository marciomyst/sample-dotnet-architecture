using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TheMovie.Api.Extensions;
using TheMovie.Application.Movies.Commands.UpdateMovie;

namespace TheMovie.Api.Features.Movies.UpdateMovie;

/// <summary>
/// Defines HTTP API endpoints for updating movies.
/// </summary>
/// <remarks>
/// <para>
/// Maps the PUT endpoint at <c>/api/movies/{id}</c>. On success, returns <c>204 No Content</c>.
/// Validation errors are surfaced via the MediatR validation pipeline (FluentValidation).
/// </para>
/// </remarks>
public static class UpdateMovieEndpoints
{
    /// <summary>
    /// Maps the HTTP PUT endpoint used to update an existing movie.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <remarks>
    /// <para>
    /// Request body example:
    /// <code><![CDATA[
    /// PUT /api/movies/{id}
    /// Content-Type: application/json
    ///
    /// {
    ///   "title": "Pulp Fiction (Remastered)",
    ///   "synopsis": "Updated synopsis...",
    ///   "releaseYear": 1994,
    ///   "price": 25.00,
    ///   "genreId": "f4e5e6f7-1234-5678-9abc-def012345678",
    ///   "rating": "R"
    /// }
    /// ]]></code>
    /// </para>
    /// </remarks>
    public static void MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/movies/{id:guid}", async (            
            [AsParameters] UpdateMovieRequest request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            UpdateMovieCommand command = request.ToCommand(request.Id);
            var result = await mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.NoContent();
        })
        .WithName("UpdateMovie")
        .WithTags("Movies")
        .WithOpenApi((OpenApiOperation operation) =>
        {
            operation.Summary = "Updates an existing movie.";
            operation.Description = "Updates details of an existing movie. Returns 204 on success.";
            return operation;
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}
