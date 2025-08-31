using MediatR;
using TheMovie.Application.Shared;
using TheMovie.Domain.Interfaces;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Application.Movies.Commands.DeleteMovie;

/// <summary>
/// Handles the <see cref="DeleteMovieCommand"/> use case by removing a movie aggregate.
/// </summary>
/// <remarks>
/// <para>
/// Orchestrates the deletion by fetching the aggregate via <see cref="IMovieRepository"/>,
/// validating existence, marking it for removal, and committing the transaction through
/// <see cref="IUnitOfWork"/>.
/// </para>
/// <para>
/// Failure codes returned:
/// - <c>Movie.InvalidId</c> — when the id is empty.
/// - <c>Movie.NotFound</c> — when the movie does not exist.
/// </para>
/// </remarks>
/// <param name="movieRepository">Repository for accessing and mutating movies.</param>
/// <param name="unitOfWork">Unit of work to commit changes.</param>
public class DeleteMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteMovieCommand, Result>
{
    /// <inheritdoc />
    public async Task<Result> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            return Result.Fail(new Error("Movie.InvalidId", "Movie id is invalid."));
        }

        var movie = await movieRepository.GetByIdAsync(request.Id);
        if (movie is null)
        {
            // Use a code that maps to 404 via HttpErrorMapper (ends with .NotFound)
            return Result.Fail(new Error("Movie.NotFound", "Movie not found."));
        }

        movieRepository.Remove(movie);
        await unitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result.Ok();
    }
}
