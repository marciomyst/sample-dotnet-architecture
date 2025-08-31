namespace TheMovie.Application.Movies.Queries.GetMovies;

/// <summary>
/// Data Transfer Object representing a movie for listing purposes.
/// </summary>
/// <param name="Id">The unique identifier of the movie.</param>
/// <param name="Title">The title of the movie.</param>
/// <param name="ReleaseYear">The year the movie was released.</param>
/// <param name="GenreName">The name of the movie's genre.</param>
public record MovieDto(
    Guid Id,
    string Title,
    int ReleaseYear,
    string GenreName
);
