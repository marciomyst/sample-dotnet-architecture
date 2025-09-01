using TheMovie.Domain.Aggregates.GenreAggregate;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Domain.Interfaces;

/// <summary>
/// Contract for persisting and retrieving <see cref="Genre"/> aggregate roots.
/// </summary>
/// <remarks>
/// <para>
/// The Repository pattern abstracts the data store, providing a collection-like API to add, remove and query genres
/// while keeping the domain decoupled from infrastructure concerns.
/// </para>
/// <para>
/// Changes are not persisted immediately; repositories cooperate with a Unit of Work (<see cref="IUnitOfWork"/>) so that
/// multiple operations are committed atomically when the unit of work completes.
/// </para>
/// <para>
/// Example:
/// <code>
/// var genre = new Genre("Action");
/// await genreRepository.AddAsync(genre, cancellationToken);
/// await unitOfWork.SaveEntitiesAsync(cancellationToken);
/// </code>
/// </para>
/// </remarks>
public interface IGenreRepository : IDisposable
{
    /// <summary>
    /// Adds a new <see cref="Genre"/> to the repository.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The entity is staged for insertion and persisted when the Unit of Work commits via
    /// <see cref="IUnitOfWork.SaveEntitiesAsync(System.Threading.CancellationToken)"/>.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var genre = new Genre("Action");
    /// await genreRepository.AddAsync(genre, cancellationToken);
    /// await unitOfWork.SaveEntitiesAsync(cancellationToken);
    /// </code>
    /// </para>
    /// </remarks>
    /// <param name="genre">The genre entity to add. Must not be <c>null</c>.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(Genre genre, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a <see cref="Genre"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the genre.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>
    /// The <see cref="Genre"/> if found; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Implementations typically use no-tracking queries for read-only scenarios.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var genre = await genreRepository.GetByIdAsync(id, cancellationToken);
    /// if (genre is null)
    /// {
    ///     // handle not found
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a <see cref="Genre"/> from the repository.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Marks the entity for deletion within the current Unit of Work; the physical delete occurs when
    /// <see cref="IUnitOfWork.SaveEntitiesAsync(System.Threading.CancellationToken)"/> is called.
    /// </para>
    /// <para>
    /// Implementations may perform hard or soft deletes depending on persistence strategy.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var genre = await genreRepository.GetByIdAsync(id, cancellationToken);
    /// if (genre is not null)
    /// {
    ///     await genreRepository.RemoveAsync(genre, cancellationToken);
    ///     await unitOfWork.SaveEntitiesAsync(cancellationToken);
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    /// <param name="genre">The genre entity to remove. Must not be <c>null</c>.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(Genre genre, CancellationToken cancellationToken = default);
}
