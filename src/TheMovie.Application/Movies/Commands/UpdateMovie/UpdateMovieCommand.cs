using MediatR;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Application.Movies.Commands.UpdateMovie;

/// <summary>
/// Represents a command to update an existing movie in the system.
/// </summary>
/// <param name="Id">The unique identifier of the movie to update.</param>
/// <param name="Title">The new title of the movie.</param>
/// <param name="Synopsis">The new synopsis of the movie plot.</param>
/// <param name="ReleaseYear">The new release year of the movie.</param>
/// <param name="Price">The new ticket price for the movie.</param>
/// <param name="GenreId">The new unique identifier of the movie's genre.</param>
/// <param name="Rating">The new content rating of the movie.</param>
public record UpdateMovieCommand(
    Guid Id,
    string Title,
    string Synopsis,
    int ReleaseYear,
    decimal Price,
    Guid GenreId,
    MovieRating Rating
) : IRequest<Unit>;
