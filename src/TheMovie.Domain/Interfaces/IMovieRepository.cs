using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Domain.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages persistence and retrieval of <see cref="Movie"/> aggregate roots.
/// <para>
/// The repository pattern abstracts the data store, providing a collection-like interface for accessing and modifying
/// <see cref="Movie"/> entities. It is responsible for encapsulating all logic required to add, update, and retrieve
/// movies, ensuring that the domain model remains decoupled from infrastructure concerns.
/// </para>
/// <para>
/// In a DDD context, repositories should only be defined for aggregate roots. This interface should be implemented
/// in the infrastructure layer, typically using an ORM such as Entity Framework Core.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// The repository does not immediately persist changes to the database. Instead, it works in conjunction with the
/// Unit of Work pattern (<see cref="IUnitOfWork"/>), so that all changes are committed as a single transaction
/// when the unit of work is completed.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var movie = new Movie(...);
/// movieRepository.Add(movie);
/// // ... other operations ...
/// await unitOfWork.SaveEntitiesAsync();
/// </code>
/// </para>
/// </remarks>
public interface IMovieRepository
{
    /// <summary>
    /// Adds a new <see cref="Movie"/> to the repository.
    /// <para>
    /// The actual database insertion is performed when the Unit of Work is committed.
    /// </para>
    /// </summary>
    /// <param name="movie">The movie entity to add. Must not be <c>null</c>.</param>
    void Add(Movie movie);

    /// <summary>
    /// Finds a <see cref="Movie"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the movie.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the movie, or <c>null</c> if not found.
    /// </returns>
    Task<Movie?> GetByIdAsync(Guid id);

    /// <summary>
    /// Finds a <see cref="Movie"/> by its title using an exact match.
    /// </summary>
    /// <param name="title">The title to search for. Must not be <c>null</c> or empty.</param>
    /// <returns>
    /// A task whose result is the matching <see cref="Movie"/>, or <c>null</c> if no movie matches the provided title.
    /// </returns>
    Task<Movie?> GetByTitleAsync(string title);

    /// <summary>
    /// Marks an existing <see cref="Movie"/> as modified in the unit of work.
    /// <para>
    /// While EF Core's change tracker can often detect changes automatically,
    /// explicitly calling <c>Update</c> can be useful in some scenarios and makes the intent clear.
    /// </para>
    /// </summary>
    /// <param name="movie">The movie entity to update. Must not be <c>null</c>.</param>
    void Update(Movie movie);
}
