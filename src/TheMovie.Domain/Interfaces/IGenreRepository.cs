using TheMovie.Domain.Aggregates.GenreAggregate;

namespace TheMovie.Domain.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages persistence and retrieval of <see cref="Genre"/> aggregate roots.
/// <para>
/// The repository pattern abstracts the data access layer, providing a collection-like interface for accessing and modifying
/// <see cref="Genre"/> entities. It is responsible for encapsulating all logic required to add, remove, and retrieve
/// genres, ensuring that the domain model remains decoupled from infrastructure concerns.
/// </para>
/// <para>
/// In a DDD context, repositories should only be defined for aggregate roots. This interface should be implemented
/// in the infrastructure layer, typically using an ORM such as Entity Framework Core.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// The repository does not immediately persist changes to the database. Instead, it works in conjunction with the
/// Unit of Work pattern (<c>IUnitOfWork</c>), so that all changes are committed as a single transaction
/// when the unit of work is completed.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var genre = new Genre("Action");
/// await genreRepository.AddAsync(genre, cancellationToken);
/// // ... other operations ...
/// await unitOfWork.SaveEntitiesAsync(cancellationToken);
/// </code>
/// </para>
/// </remarks>
public interface IGenreRepository : IDisposable
{
    /// <summary>
    /// Adds a new <see cref="Genre"/> to the repository.
    /// <para>
    /// The actual database insertion is performed when the Unit of Work is committed.
    /// </para>
    /// </summary>
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
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a <see cref="Genre"/> from the repository.
    /// <para>
    /// The actual database removal is performed when the Unit of Work is committed.
    /// </para>
    /// </summary>
    /// <param name="genre">The genre entity to remove. Must not be <c>null</c>.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(Genre genre, CancellationToken cancellationToken = default);
}
