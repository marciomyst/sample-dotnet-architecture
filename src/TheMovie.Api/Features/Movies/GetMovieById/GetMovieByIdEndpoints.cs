using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TheMovie.Application.Movies.Queries.GetMovieById;

namespace TheMovie.Api.Features.Movies.GetMovieById;

/// <summary>
/// Defines HTTP API endpoint for retrieving a movie by id.
/// </summary>
/// <remarks>
/// <para>
/// Maps the GET endpoint at <c>/api/movies/{id}</c>. On success, returns <c>200 OK</c> with the movie details.
/// When no movie is found, returns <c>404 Not Found</c> with a ProblemDetails payload.
/// </para>
/// </remarks>
public static class GetMovieByIdEndpoints
{
    /// <summary>
    /// Registers the GET endpoint used to retrieve a movie by its identifier.
    /// </summary>
    public static void MapGetMovieById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/movies/{id:guid}", async (
            [AsParameters] GetMovieByIdRequest request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            GetMovieByIdQuery query = request.ToQuery();
            MovieDetailDto? result = await mediator.Send(query, cancellationToken);
            if (result is null)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found"
                };
                return Results.Problem(problem);
            }

            return Results.Ok(result.ToResponse());
        })
        .WithName("GetMovieById")
        .WithTags("Movies")
        .WithOpenApi((OpenApiOperation operation) =>
        {
            operation.Summary = "Gets a movie by id.";
            operation.Description = "Retrieves detailed information about a movie by its identifier.";
            return operation;
        })
        .Produces<GetMovieByIdResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}

