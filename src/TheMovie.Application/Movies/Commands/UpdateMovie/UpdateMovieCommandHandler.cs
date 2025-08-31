using MediatR;
using TheMovie.Application.Shared;
using TheMovie.Domain.Interfaces;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Application.Movies.Commands.UpdateMovie;

/// <summary>
/// Handles <see cref="UpdateMovieCommand"/> by applying changes to an existing movie aggregate and persisting them.
/// </summary>
/// <param name="movieRepository">Repository for managing movie persistence.</param>
/// <param name="unitOfWork">Unit of work responsible for committing changes.</param>
public class UpdateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateMovieCommand, Result>
{
    private readonly IMovieRepository _movieRepository = movieRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Handles the <see cref="UpdateMovieCommand"/> by updating and persisting a movie.
    /// </summary>
    /// <param name="request">The command containing movie details.</param>
    /// <param name="cancellationToken">Cancellation token to observe while processing.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
    public async Task<Result> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id);

        if (movie is null)
        {
            return Result.Fail(new Error("Movie.NotFound", "Movie not found."));
        }

        movie.Update(
            request.Title,
            request.Synopsis,
            request.ReleaseYear,
            request.Price,
            request.GenreId,
            request.Rating
        );

        _movieRepository.Update(movie);
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result.Ok();
    }
}
