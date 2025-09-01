namespace TheMovie.Api.Features.Movies.GetMovies;

/// <summary>
/// Represents the HTTP query parameters for listing movies with optional filters and pagination.
/// </summary>
/// <param name="Title">Optional title filter (partial match).</param>
/// <param name="ReleaseYear">Optional filter by release year.</param>
/// <param name="GenreId">Optional filter by genre identifier.</param>
/// <param name="PageNumber">The page number to retrieve (default 1).</param>
/// <param name="PageSize">The number of items per page (default 10).</param>
public record GetMoviesRequest(
    string? Title,
    int? ReleaseYear,
    Guid? GenreId,
    int PageNumber = 1,
    int PageSize = 10
);

