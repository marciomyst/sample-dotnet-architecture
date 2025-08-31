namespace TheMovie.Api.Features.Movies.CreateMovie;

/// <summary>
/// Represents the HTTP response after creating a new movie.
/// </summary>
/// <remarks>
/// <para>
/// This lightweight response DTO is returned by the Movies feature when a movie is successfully created.
/// It is typically returned with the 201 Created status code and a Location header pointing to the new resource.
/// </para>
/// <para>
/// Example:
/// <code><![CDATA[
/// return Results.Created($"/movies/{result.Value}", new CreateMovieResponse(result.Value));
/// ]]></code>
/// </para>
/// </remarks>
/// <param name="Id">The unique identifier of the created movie.</param>
public record CreateMovieResponse(Guid Id);
