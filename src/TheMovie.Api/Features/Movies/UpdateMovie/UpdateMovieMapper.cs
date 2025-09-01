using TheMovie.Application.Movies.Commands.UpdateMovie;

namespace TheMovie.Api.Features.Movies.UpdateMovie;

/// <summary>
/// Maps between HTTP-layer DTOs and application-layer commands for the Update Movie feature.
/// </summary>
/// <remarks>
/// <para>
/// These helpers keep Minimal API endpoints concise by encapsulating translation from the transport DTO
/// (<see cref="UpdateMovieRequest"/>) to the application command (<see cref="UpdateMovieCommand"/>).
/// </para>
/// <para>
/// Example usage inside an endpoint delegate:
/// <code><![CDATA[
/// var command = request.ToCommand();
/// var result  = await mediator.Send(command, cancellationToken);
/// return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
/// ]]></code>
/// </para>
/// </remarks>
internal static class UpdateMovieMapper
{
    /// <summary>
    /// Creates an <see cref="UpdateMovieCommand"/> from the HTTP request by combining route id and body.
    /// </summary>
    /// <param name="request">The update request containing the route id and the body payload.</param>
    /// <returns>An <see cref="UpdateMovieCommand"/> populated with values from the request.</returns>
    public static UpdateMovieCommand ToCommand(this UpdateMovieRequest request)
    {
        return new(
            Id: request.Id,
            Title: request.Body.Title,
            Synopsis: request.Body.Synopsis,
            ReleaseYear: request.Body.ReleaseYear,
            Price: request.Body.Price,
            GenreId: request.Body.GenreId,
            Rating: request.Body.Rating
        );
    }
}
