using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Api.Features.Movies.UpdateMovie;

/// <summary>
/// Represents the JSON body for updating a movie.
/// </summary>
/// <remarks>
/// <para>
/// This record maps directly to the JSON object sent by clients as the request body of the update endpoint.
/// It is nested under <see cref="UpdateMovieRequest"/> to keep the Minimal API with a single body parameter.
/// </para>
/// </remarks>
/// <param name="Title">The updated movie title.</param>
/// <param name="Synopsis">The updated synopsis.</param>
/// <param name="ReleaseYear">The updated release year (must not be in the future).</param>
/// <param name="Price">The updated price (must be greater than zero).</param>
/// <param name="GenreId">The updated genre identifier.</param>
/// <param name="Rating">The updated content rating.</param>
public record UpdateMovieRequestBody(
   string Title,
   string Synopsis,
   int ReleaseYear,
   decimal Price,
   Guid GenreId,
   MovieRating Rating
);
