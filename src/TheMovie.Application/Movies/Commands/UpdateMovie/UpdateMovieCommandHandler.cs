using MediatR;
using TheMovie.Domain.Interfaces;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Application.Movies.Commands.UpdateMovie;

/// <summary>
/// Handler for <see cref="UpdateMovieCommand"/> that updates an existing movie in the domain.
/// </summary>
/// <param name="movieRepository">Repository for managing movie persistence.</param>
/// <param name="unitOfWork">Unit of work responsible for committing changes.</param>
public class UpdateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateMovieCommand, Unit>
{
    private readonly IMovieRepository _movieRepository = movieRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Handles the <see cref="UpdateMovieCommand"/> by updating and persisting a movie.
    /// </summary>
    /// <param name="request">The command containing movie details.</param>
    /// <param name="cancellationToken">Cancellation token to observe while processing.</param>
    /// <returns>A <see cref="Unit"/> value.</returns>
    public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.MovieAggregate.Movie? movie = await _movieRepository.GetByIdAsync(request.Id);

        if (movie is null)
        {
            // Or throw a custom exception
            return Unit.Value;
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

        return Unit.Value;
    }
}
