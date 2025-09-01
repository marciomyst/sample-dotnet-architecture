using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Api.Features.Movies.CreateMovie;

/// <summary>
/// Represents the JSON body for creating a movie.
/// </summary>
/// <remarks>
/// <para>
/// This record maps directly to the JSON object sent by clients and is nested inside
/// <see cref="CreateMovieRequest"/> to keep the Minimal API signature with a single body parameter.
/// </para>
/// </remarks>
/// <param name="Title">The title of the movie.</param>
/// <param name="Synopsis">A brief synopsis of the movie plot.</param>
/// <param name="ReleaseYear">The year the movie was released (must be greater than 1888).</param>
/// <param name="Price">The ticket price for the movie (must be greater than zero).</param>
/// <param name="GenreId">The unique identifier of the movie's genre.</param>
/// <param name="Rating">The content rating of the movie.</param>
public record CreateMovieRequestBody(
    string Title,
    string Synopsis,
    int ReleaseYear,
    decimal Price,
    Guid GenreId,
    MovieRating Rating
);
