using TheMovie.Application.Movies.Commands.CreateMovie;

namespace TheMovie.Api.Features.Movies.CreateMovie;

/// <summary>
/// Maps between HTTP-layer DTOs and application-layer commands/responses for the Create Movie feature.
/// </summary>
/// <remarks>
/// <para>
/// These extension methods keep the API endpoint lean by encapsulating translation from
/// <see cref="CreateMovieRequest"/> to <see cref="CreateMovieCommand"/> and from the created identifier to
/// <see cref="CreateMovieResponse"/>.
/// </para>
/// <para>
/// Example:
/// <code><![CDATA[
/// var cmd = request.ToCommand();
/// var result = await mediator.Send(cmd);
/// return result.IsSuccess
///     ? Results.Created($"/movies/{result.Value}", result.Value.ToResponse())
///     : result.ToProblemDetails();
/// ]]></code>
/// </para>
/// </remarks>
internal static class CreateMovieMapper
{
    /// <summary>
    /// Maps a CreateMovieRequest to a CreateMovieCommand.
    /// </summary>
    /// <param name="request">The HTTP request containing movie creation payload.</param>
    /// <returns>A CreateMovieCommand populated with values from the payload.</returns>
    public static CreateMovieCommand ToCommand(this CreateMovieRequest request)
    {
        return new (
            request.Title,
            request.Synopsis,
            request.ReleaseYear,
            request.Price,
            request.GenreId,
            request.Rating
        );
    }

    /// <summary>
    /// Converts a newly created movie identifier to a <see cref="CreateMovieResponse"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the created movie.</param>
    /// <returns>A <see cref="CreateMovieResponse"/> containing the created movie identifier.</returns>
    /// <remarks>
    /// <para>
    /// Useful for minimal API endpoints returning 201 Created with a response body.
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// var response = result.Value.ToResponse();
    /// return Results.CreatedAtRoute("GetMovieById", new { id = response.Id }, response);
    /// ]]></code>
    /// </para>
    /// </remarks>
    public static CreateMovieResponse ToResponse(this Guid id)
    {
        return new (id);
    }
}
