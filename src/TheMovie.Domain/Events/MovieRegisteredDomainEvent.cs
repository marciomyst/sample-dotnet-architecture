using MediatR;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Domain.Events;

/// <summary>
/// Domain event raised when a new <see cref="Movie"/> is registered in the system.
/// <para>
/// In Domain-Driven Design (DDD), domain events are used to capture and communicate significant changes or occurrences
/// within the domain model. This event allows other parts of the system to react to the registration of a new movie,
/// such as updating read models, sending notifications, or integrating with external systems.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// This event should be published by the aggregate root or application service responsible for registering a new movie.
/// Handlers for this event can be implemented to perform side effects or additional business logic in response to the movie registration.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var movie = new Movie(...);
/// var domainEvent = new MovieRegisteredDomainEvent(movie);
/// movie.AddDomainEvent(domainEvent);
/// </code>
/// </para>
/// </remarks>
/// <param name="movie">The <see cref="Movie"/> that has been registered.</param>
public class MovieRegisteredDomainEvent(Movie movie) : INotification
{
    /// <summary>
    /// Gets the <see cref="Movie"/> associated with this event.
    /// </summary>
    public Movie Movie { get; } = movie;
}
