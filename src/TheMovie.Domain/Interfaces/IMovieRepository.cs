using TheMovie.Domain.Aggregates.MovieAggregate;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Domain.Interfaces;

/// <summary>
/// Contract for persisting and retrieving <see cref="Movie"/> aggregate roots.
/// </summary>
/// <remarks>
/// <para>
/// The Repository pattern abstracts the data store, offering a collection-like API to add, update,
/// remove and query movies while keeping the domain decoupled from infrastructure concerns.
/// </para>
/// <para>
/// This interface is intended for aggregate roots only and should be implemented in the Infrastructure layer
/// (e.g., with Entity Framework Core).
/// </para>
/// <para>
/// Changes are not persisted immediately; repositories cooperate with a Unit of Work (<see cref="IUnitOfWork"/>)
/// so that multiple operations are committed atomically when the unit of work completes.
/// </para>
/// <para>
/// Example:
/// <code>
/// var movie = new Movie("Inception", "A mind-bending thriller.", 2010, 15.0m, genreId, MovieRating.PG13);
/// movieRepository.Add(movie);
/// // ... other operations ...
/// await unitOfWork.SaveEntitiesAsync(cancellationToken);
/// </code>
/// </para>
/// </remarks>
public interface IMovieRepository
{
    /// <summary>
    /// Adds a new <see cref="Movie"/> to the repository.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Stages the entity for insertion; the physical write occurs when
    /// <see cref="IUnitOfWork.SaveEntitiesAsync(System.Threading.CancellationToken)"/> is called.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var movie = new Movie("Inception", "A mind-bending thriller.", 2010, 15.0m, genreId, MovieRating.PG13);
    /// movieRepository.Add(movie);
    /// await unitOfWork.SaveEntitiesAsync(cancellationToken);
    /// </code>
    /// </para>
    /// </remarks>
    /// <param name="movie">The movie entity to add. Must not be <c>null</c>.</param>
    void Add(Movie movie);

    /// <summary>
    /// Finds a <see cref="Movie"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the movie.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the movie,
    /// or <c>null</c> if not found.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Implementations typically use no-tracking queries for read-only scenarios.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var movie = await movieRepository.GetByIdAsync(id);
    /// if (movie is null)
    /// {
    ///     // handle not found
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    Task<Movie?> GetByIdAsync(Guid id);

    /// <summary>
    /// Finds a <see cref="Movie"/> by its title using an exact match.
    /// </summary>
    /// <param name="title">The title to search for. Must not be <c>null</c> or empty.</param>
    /// <returns>
    /// A task whose result is the matching <see cref="Movie"/>, or <c>null</c> if no movie matches the provided title.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Implementations typically use no-tracking queries for read-only scenarios. Matching semantics (case sensitivity,
    /// culture) depend on the underlying database collation.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var existing = await movieRepository.GetByTitleAsync("Inception");
    /// if (existing is not null)
    /// {
    ///     // title already in use
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    Task<Movie?> GetByTitleAsync(string title);

    /// <summary>
    /// Marks an existing <see cref="Movie"/> as modified in the current Unit of Work.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Although change tracking may detect modifications automatically, explicitly calling <c>Update</c>
    /// can be useful to ensure intent or attach detached entities.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var movie = await movieRepository.GetByIdAsync(id);
    /// if (movie is not null)
    /// {
    ///     movie.Update(newTitle, newSynopsis, newYear, newPrice, newGenreId, newRating);
    ///     movieRepository.Update(movie);
    ///     await unitOfWork.SaveEntitiesAsync(cancellationToken);
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    /// <param name="movie">The movie entity to update. Must not be <c>null</c>.</param>
    void Update(Movie movie);

    /// <summary>
    /// Removes an existing <see cref="Movie"/> from the repository.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method marks the entity for deletion within the current Unit of Work; the physical
    /// database operation occurs when <see cref="IUnitOfWork.SaveEntitiesAsync(System.Threading.CancellationToken)"/>
    /// is called.
    /// </para>
    /// <para>
    /// Implementations may perform a hard delete or a soft delete depending on the persistence strategy.
    /// </para>
    /// <para>
    /// Usage example:
    /// <code>
    /// var movie = await movieRepository.GetByIdAsync(id);
    /// if (movie is not null)
    /// {
    ///     movieRepository.Remove(movie);
    ///     await unitOfWork.SaveEntitiesAsync(cancellationToken);
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    /// <param name="movie">The movie entity to remove. Must not be <c>null</c>.</param>
    void Remove(Movie movie);
}
