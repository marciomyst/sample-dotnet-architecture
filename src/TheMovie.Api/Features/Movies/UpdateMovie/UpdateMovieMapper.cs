using TheMovie.Application.Movies.Commands.UpdateMovie;

namespace TheMovie.Api.Features.Movies.UpdateMovie;

/// <summary>
/// Maps between HTTP-layer DTOs and application-layer commands for the Update Movie feature.
/// </summary>
/// <remarks>
/// <para>
/// These helpers keep Minimal API endpoints concise by encapsulating translation from the transport DTO
/// (<see cref="UpdateMovieRequest"/>) plus the route identifier to the application command
/// (<see cref="UpdateMovieCommand"/>).
/// </para>
/// <para>
/// Example usage inside an endpoint delegate:
/// <code><![CDATA[
/// var command = request.ToCommand(id);
/// var result  = await mediator.Send(command, cancellationToken);
/// return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
/// ]]></code>
/// </para>
/// </remarks>
internal static class UpdateMovieMapper
{
    /// <summary>
    /// Creates an <see cref="UpdateMovieCommand"/> from the HTTP request payload and route identifier.
    /// </summary>
    /// <param name="id">The movie identifier from the route (<c>PUT /api/movies/{id}</c>).</param>
    /// <param name="request">The update payload bound from the body.</param>
    /// <returns>An <see cref="UpdateMovieCommand"/> populated with values from the request.</returns>
    public static UpdateMovieCommand ToCommand(this UpdateMovieRequest request, Guid id)
    {
        return new(
            Id: id,
            Title: request.Title,
            Synopsis: request.Synopsis,
            ReleaseYear: request.ReleaseYear,
            Price: request.Price,
            GenreId: request.GenreId,
            Rating: request.Rating
        );
    }
}
