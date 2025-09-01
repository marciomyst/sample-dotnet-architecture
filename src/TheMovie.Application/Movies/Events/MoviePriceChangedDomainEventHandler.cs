using MediatR;
using Microsoft.Extensions.Logging;
using TheMovie.Domain.Events;

namespace TheMovie.Application.Movies.Events;

/// <summary>
/// Example handler for <see cref="MoviePriceChangedDomainEvent"/> showing a simple, idempotent reaction.
/// </summary>
/// <remarks>
/// Prefer non-throwing behavior and idempotent side effects. When updating projections, consider upserts keyed by
/// <see cref="MoviePriceChangedDomainEvent.MovieId"/> and store current price only.
/// </remarks>
public class MoviePriceChangedDomainEventHandler(ILogger<MoviePriceChangedDomainEventHandler> logger)
    : INotificationHandler<MoviePriceChangedDomainEvent>
{
    private readonly ILogger<MoviePriceChangedDomainEventHandler> _logger = logger;

    /// <summary>
    /// Handles the <see cref="MoviePriceChangedDomainEvent"/> by logging the change.
    /// </summary>
    public Task Handle(MoviePriceChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEvent: Movie price changed. MovieId={MovieId}, Old={Old}, New={New}",
            notification.MovieId,
            notification.OldPrice,
            notification.NewPrice);
        return Task.CompletedTask;
    }
}

