using Microsoft.AspNetCore.Mvc;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Api.Features.Movies.UpdateMovie;

/// <summary>
/// Represents the HTTP request payload to update an existing movie.
/// </summary>
/// <remarks>
/// <para>
/// Bound from the request body of <c>PUT /api/movies/{id}</c>. Validation is performed in the application layer via
/// FluentValidation before the handler executes.
/// </para>
/// <para>
/// Example payload:
/// <code><![CDATA[
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
/// <param name="Title">The updated title of the movie.</param>
/// <param name="Synopsis">The updated synopsis of the movie plot.</param>
/// <param name="ReleaseYear">The updated release year of the movie.</param>
/// <param name="Price">The updated ticket price for the movie.</param>
/// <param name="GenreId">The updated unique identifier of the movie's genre.</param>
/// <param name="Rating">The updated content rating of the movie.</param>
public record UpdateMovieRequest(
    [FromRoute] Guid Id,
    [FromBody] string Title,
    [FromBody] string Synopsis,
    [FromBody] int ReleaseYear,
    [FromBody] decimal Price,
    [FromBody] Guid GenreId,
    [FromBody] MovieRating Rating
);
