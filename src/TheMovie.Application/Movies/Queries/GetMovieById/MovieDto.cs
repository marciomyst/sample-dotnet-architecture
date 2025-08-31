namespace TheMovie.Application.Movies.Queries.GetMovieById;

/// <summary>
/// Data Transfer Object containing detailed information for a single movie.
/// This is a read model independent of the domain entity.
/// </summary>
/// <param name="Id">The unique identifier of the movie.</param>
/// <param name="Title">The title of the movie.</param>
/// <param name="Synopsis">The synopsis of the movie.</param>
/// <param name="ReleaseYear">The year the movie was released.</param>
/// <param name="TicketPrice">The ticket price of the movie.</param>
/// <param name="GenreName">The name of the genre.</param>
/// <param name="Rating">The rating of the movie.</param>
public record MovieDetailDto(
    Guid Id,
    string Title,
    string Synopsis,
    int ReleaseYear,
    decimal TicketPrice,
    string GenreName,
    string Rating
);
