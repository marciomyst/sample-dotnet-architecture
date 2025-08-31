using MediatR;
using TheMovie.Application.Shared;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Application.Movies.Commands.CreateMovie;

/// <summary>
/// Represents the command to create a new movie.
/// This is an immutable data transfer object that carries the necessary information for the use case.
/// It implements IRequest from MediatR, specifying the expected return type.
/// </summary>
/// <param name="Title">The title of the movie.</param>
/// <param name="Synopsis">A brief synopsis of the movie.</param>
/// <param name="ReleaseYear">The year the movie was released.</param>
/// <param name="Price">The price of a single movie.</param>
/// <param name="GenreId">The unique identifier for the movie's genre.</param>
/// <param name="Rating">The content rating of the movie.</param>
public record CreateMovieCommand(
    string Title,
    string Synopsis,
    int ReleaseYear,
    decimal Price,
    Guid GenreId,
    MovieRating Rating) : IRequest<Result<Guid>>;
