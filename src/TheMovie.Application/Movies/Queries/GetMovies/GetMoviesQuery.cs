using MediatR;
using TheMovie.Application.Shared;

namespace TheMovie.Application.Movies.Queries.GetMovies;

/// <summary>
/// Query to retrieve a paginated list of movies with optional filters.
/// </summary>
/// <param name="Title">Title filter for movie search; matches partial or full title (optional).</param>
/// <param name="ReleaseYear">Filter by the year the movie was released (optional).</param>
/// <param name="GenreId">Filter by genre identifier (optional).</param>
/// <param name="PageNumber">The page number to retrieve (defaults to 1).</param>
/// <param name="PageSize">The number of items per page (defaults to 10).</param>
/// <returns>
/// A <see cref="PagedResult{MovieDto}"/> containing the paginated list of movies.
/// </returns>
public record GetMoviesQuery(
    string? Title,
    int? ReleaseYear,
    Guid? GenreId,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedResult<MovieDto>>;
