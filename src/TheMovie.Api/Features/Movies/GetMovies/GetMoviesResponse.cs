namespace TheMovie.Api.Features.Movies.GetMovies;

/// <summary>
/// Represents a paginated list of movies.
/// </summary>
/// <param name="Items">The movies in the current page.</param>
/// <param name="PageNumber">The current page number.</param>
/// <param name="PageSize">The size of each page.</param>
/// <param name="TotalCount">The total number of records.</param>
public record GetMoviesResponse(
    IReadOnlyList<GetMoviesItem> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
);

/// <summary>
/// Represents a single movie entry in the list response.
/// </summary>
/// <param name="Id">The unique identifier of the movie.</param>
/// <param name="Title">The title of the movie.</param>
/// <param name="ReleaseYear">The year the movie was released.</param>
/// <param name="GenreName">The name of the genre.</param>
public record GetMoviesItem(
    Guid Id,
    string Title,
    int ReleaseYear,
    string GenreName
);

