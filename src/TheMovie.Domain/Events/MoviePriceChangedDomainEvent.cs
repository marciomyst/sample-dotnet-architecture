using MediatR;

namespace TheMovie.Domain.Events;

/// <summary>
/// Domain event raised when the price of a <c>Movie</c> aggregate is changed.
/// <para>
/// In Domain-Driven Design (DDD), domain events are used to capture and communicate significant changes or occurrences
/// within the domain model. This event provides the necessary context for handlers to react to a price change,
/// such as updating projections, triggering notifications, or integrating with external systems.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// This event should be published by the <c>Movie</c> aggregate root whenever its price is changed.
/// Event handlers can use the provided identifiers and price values to perform additional logic or side effects.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var domainEvent = new MoviePriceChangedDomainEvent(movie.Id, oldPrice, newPrice);
/// movie.AddDomainEvent(domainEvent);
/// </code>
/// </para>
/// </remarks>
/// <param name="movieId">The unique identifier of the movie whose price has changed.</param>
/// <param name="oldPrice">The previous price of the movie.</param>
/// <param name="newPrice">The new price of the movie.</param>
public class MoviePriceChangedDomainEvent(Guid movieId, decimal oldPrice, decimal newPrice) : INotification
{
    /// <summary>
    /// Gets the unique identifier of the movie whose price has changed.
    /// </summary>
    /// <remarks>Use this to correlate the event back to the aggregate or projections.</remarks>
    public Guid MovieId { get; } = movieId;

    /// <summary>
    /// Gets the previous price of the movie.
    /// </summary>
    /// <remarks>Represents the value before the change occurred; useful for differential updates.</remarks>
    public decimal OldPrice { get; } = oldPrice;

    /// <summary>
    /// Gets the new price of the movie.
    /// </summary>
    /// <remarks>Represents the value after the change; consumers should not assume currency conversions.</remarks>
    public decimal NewPrice { get; } = newPrice;
}
