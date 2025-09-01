namespace TheMovie.Api.Features.Movies.GetMovieById;

/// <summary>
/// Represents the HTTP response payload for a movie lookup by id.
/// </summary>
/// <param name="Id">The movie identifier.</param>
/// <param name="Title">The movie title.</param>
/// <param name="Synopsis">The synopsis of the movie.</param>
/// <param name="ReleaseYear">The release year.</param>
/// <param name="Price">The ticket price.</param>
/// <param name="GenreName">The genre name.</param>
/// <param name="Rating">The rating as a string.</param>
public record GetMovieByIdResponse(
    Guid Id,
    string Title,
    string Synopsis,
    int ReleaseYear,
    decimal Price,
    string GenreName,
    string Rating
);

