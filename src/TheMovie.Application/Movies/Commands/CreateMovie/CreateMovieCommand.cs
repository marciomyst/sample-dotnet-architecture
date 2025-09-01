using MediatR;
using TheMovie.Application.Shared;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Application.Movies.Commands.CreateMovie;

/// <summary>
/// Command (CQRS) that requests the creation of a new movie aggregate.
/// </summary>
/// <remarks>
/// <para>
/// This immutable request is handled by a corresponding handler via MediatR. Validation is performed
/// earlier in the pipeline by FluentValidation, keeping the handler focused on orchestrating the use case.
/// The command returns a <see cref="Result{T}"/> with the identifier (<see cref="Guid"/>) of the newly created movie
/// when successful, or a failure <see cref="Result"/> with domain errors when business rules are violated.
/// </para>
/// <para>
/// Example:
/// <code><![CDATA[
/// var command = new CreateMovieCommand(
///     Title: "Inception",
///     Synopsis: "A thief who steals corporate secrets through dream-sharing technology...",
///     ReleaseYear: 2010,
///     Price: 15.00m,
///     GenreId: genreId,
///     Rating: MovieRating.PG13);
/// Result<Guid> result = await mediator.Send(cmd);
/// ]]></code>
/// </para>
/// </remarks>
/// <param name="Title">The title of the movie.</param>
/// <param name="Synopsis">A brief synopsis of the movie.</param>
/// <param name="ReleaseYear">The year the movie was released.</param>
/// <param name="Price">The ticket price for the movie.</param>
/// <param name="GenreId">The unique identifier for the movie's genre.</param>
/// <param name="Rating">The content rating of the movie.</param>
public record CreateMovieCommand(
    string Title,
    string Synopsis,
    int ReleaseYear,
    decimal Price,
    Guid GenreId,
    MovieRating Rating) : IRequest<Result<Guid>>;
