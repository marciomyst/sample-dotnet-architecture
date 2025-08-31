using MediatR;
using TheMovie.Application.Shared;
using TheMovie.Domain.Aggregates.MovieAggregate;
using TheMovie.Domain.Interfaces;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Application.Movies.Commands.CreateMovie;

/// <summary>
/// Handler for the <see cref="CreateMovieCommand"/>.
/// Responsible for orchestrating the movie creation use case.
/// </summary>
/// <remarks>
/// <para>
/// Validation runs earlier in the MediatR pipeline via the ValidationBehaviour and FluentValidation validators,
/// so this handler assumes the request is valid. It creates the <see cref="Movie"/> aggregate, enlists it via
/// <see cref="IMovieRepository"/>, and relies on <see cref="IUnitOfWork"/> to persist changes and dispatch domain events.
/// </para>
/// <para>
/// Example:
/// <code><![CDATA[
/// var id = await mediator.Send(new CreateMovieCommand(
///     title: "Inception",
///     synopsis: "A thief who steals corporate secrets through dream-sharing technology...",
///     releaseYear: 2010,
///     ticketPrice: 15.00m,
///     genreId: someGenreId,
///     rating: Rating.PG13));
/// ]]></code>
/// </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="CreateMovieCommandHandler"/> class.
/// </remarks>
/// <param name="movieRepository">The repository for the Movie aggregate.</param>
/// <param name="unitOfWork">The unit of work for committing the transaction.</param>
public class CreateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateMovieCommand, Result<Guid>>
{
    /// <summary>
    /// Handles the movie creation command.
    /// </summary>
    /// <param name="request">The create movie command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Result{T}"/> containing the new movie identifier on success; otherwise, a failure result.</returns>
    /// <remarks>
    /// <para>
    /// This method constructs the domain aggregate and stages it for persistence. The actual database write and
    /// domain event publication occur when the unit of work commits.
    /// </para>
    /// </remarks>
    public async Task<Result<Guid>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie(
            request.Title,
            request.Synopsis,
            request.ReleaseYear,
            request.Price,
            request.GenreId,
            request.Rating);

        movieRepository.Add(movie);

        await unitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result<Guid>.Ok(movie.Id);
    }
}
