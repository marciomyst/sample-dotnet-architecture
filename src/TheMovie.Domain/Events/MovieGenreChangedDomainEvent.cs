using MediatR;

namespace TheMovie.Domain.Events;

/// <summary>
/// Domain event raised when the genre of a <c>Movie</c> aggregate is changed.
/// <para>
/// In Domain-Driven Design (DDD), domain events are used to capture and communicate significant changes or occurrences
/// within the domain model. This event provides the necessary context for handlers to react to a genre change,
/// such as updating projections, triggering notifications, or integrating with external systems.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// This event should be published by the <c>Movie</c> aggregate root whenever its genre is changed.
/// Event handlers can use the provided identifiers to fetch additional details if needed.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var domainEvent = new MovieGenreChangedDomainEvent(movie.Id, oldGenreId, newGenreId);
/// movie.AddDomainEvent(domainEvent);
/// </code>
/// </para>
/// </remarks>
/// <param name="movieId">The unique identifier of the movie whose genre has changed.</param>
/// <param name="oldGenreId">The unique identifier of the previous genre.</param>
/// <param name="newGenreId">The unique identifier of the new genre.</param>
public class MovieGenreChangedDomainEvent(Guid movieId, Guid oldGenreId, Guid newGenreId) : INotification
{
    /// <summary>
    /// Gets the unique identifier of the movie whose genre has changed.
    /// </summary>
    /// <remarks>
    /// Stable identifier used to correlate the event with the source aggregate and to fetch the latest state if needed.
    /// </remarks>
    public Guid MovieId { get; } = movieId;

    /// <summary>
    /// Gets the unique identifier of the previous genre.
    /// </summary>
    /// <remarks>
    /// Useful for removing or updating entries from denormalized projections indexed by the old genre.
    /// </remarks>
    public Guid OldGenreId { get; } = oldGenreId;

    /// <summary>
    /// Gets the unique identifier of the new genre.
    /// </summary>
    /// <remarks>
    /// Target genre identifier to which the movie was moved; use to add or upsert entries in projections.
    /// </remarks>
    public Guid NewGenreId { get; } = newGenreId;
}
