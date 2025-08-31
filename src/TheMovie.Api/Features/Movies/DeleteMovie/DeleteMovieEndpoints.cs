using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TheMovie.Api.Extensions;
using TheMovie.Application.Movies.Commands.DeleteMovie;

namespace TheMovie.Api.Features.Movies.DeleteMovie;

/// <summary>
/// Defines the HTTP API surface for deleting movies from the catalog.
/// </summary>
/// <remarks>
/// <para>
/// Maps the DELETE endpoint at <c>/api/movies/{id}</c>. On success, returns <c>204 No Content</c>. On failure,
/// converts application-layer errors to RFC 7807 <see cref="ProblemDetails"/> via
/// <see cref="HttpErrorMapper.ToProblemDetails(TheMovie.Application.Shared.Result)"/>.
/// </para>
/// <para>
/// Example request:
/// <code><![CDATA[
/// DELETE /api/movies/815acc2d-2224-4253-b119-3c82e7286c07
/// ]]></code>
/// </para>
/// <para>
/// Possible responses:
/// - 204 No Content — movie deleted.
/// - 404 Not Found — when the movie does not exist.
/// - 400 Bad Request — invalid id format or validation errors.
/// </para>
/// </remarks>
public static class DeleteMovieEndpoints
{
    /// <summary>
    /// Registers the DELETE endpoint used to remove a movie by its identifier.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <example>
    /// <code>
    /// app.MapDeleteMovie();
    /// </code>
    /// </example>
    public static void MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/movies/{id:guid}", async (
            [AsParameters] DeleteMovieRequest request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.NoContent();
        })
        .WithName("DeleteMovie")
        .WithTags("Movies")
        .WithOpenApi((OpenApiOperation operation) =>
        {
            operation.Summary = "Deletes a movie by id.";
            operation.Description = "Removes an existing movie from the catalog. Returns 204 on success.";
            return operation;
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}
