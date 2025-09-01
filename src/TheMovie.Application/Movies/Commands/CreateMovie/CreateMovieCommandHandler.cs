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
    /// <returns>
    /// A <see cref="Result{T}"/> containing the newly created movie identifier when successful; otherwise, a failure result.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Processing steps:
    /// <list type="number">
    /// <item>Constructs the <see cref="Movie"/> aggregate from the command data.</item>
    /// <item>Enlists it for persistence via <see cref="IMovieRepository.Add(TheMovie.Domain.Aggregates.MovieAggregate.Movie)"/>.</item>
    /// <item>Commits the Unit of Work (<see cref="IUnitOfWork.SaveEntitiesAsync(System.Threading.CancellationToken)"/>) to persist and dispatch domain events.</item>
    /// </list>
    /// Validation is executed earlier in the MediatR pipeline (FluentValidation). Assuming it passes, this handler
    /// typically returns <c>Result.Ok(movie.Id)</c>.
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// Result<Guid> result = await handler.Handle(command, cancellationToken);
    /// if (result.IsSuccess)
    /// {
    ///     Guid id = result.Value!;
    /// }
    /// ]]></code>
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
