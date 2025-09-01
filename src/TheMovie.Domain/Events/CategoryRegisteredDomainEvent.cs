using MediatR;
using TheMovie.Domain.Aggregates.GenreAggregate;

namespace TheMovie.Domain.Events;

/// <summary>
/// Domain event that is triggered when a new <see cref="Genre"/> is registered in the system.
/// <para>
/// This event allows other parts of the system to react to the registration of a new genre,
/// such as updating read models, sending notifications, or integrating with external systems.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// This event should be published by the aggregate root or application service responsible for registering a new genre.
/// Handlers for this event can be implemented to perform side effects or additional business logic in response to the genre registration.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var genre = new Genre("Action");
/// var domainEvent = new GenreRegisteredDomainEvent(genre);
/// genre.AddDomainEvent(domainEvent);
/// </code>
/// </para>
/// </remarks>
/// <param name="genre">The <see cref="Genre"/> that has been registered.</param>
public class GenreRegisteredDomainEvent(Genre genre) : INotification
{
    /// <summary>
    /// Gets the <see cref="Genre"/> aggregate associated with this event.
    /// </summary>
    /// <remarks>
    /// Represents the newly created genre at the time the event was raised. Handlers should treat this instance
    /// as a read-only snapshot and avoid mutating aggregate state within the event handler.
    /// </remarks>
    public Genre Genre { get; } = genre;
}
