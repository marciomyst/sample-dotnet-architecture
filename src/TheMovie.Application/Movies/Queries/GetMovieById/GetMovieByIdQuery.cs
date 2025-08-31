using MediatR;

namespace TheMovie.Application.Movies.Queries.GetMovieById;

/// <summary>
/// Query to retrieve the details of a single movie by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the movie.</param>
/// <returns>A <see cref="MovieDetailDto"/> containing detailed information if found; otherwise <c>null</c>.</returns>
public record GetMovieByIdQuery(Guid Id) : IRequest<MovieDetailDto?>;
