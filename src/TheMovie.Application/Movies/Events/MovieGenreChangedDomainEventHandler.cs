using MediatR;
using Microsoft.Extensions.Logging;
using TheMovie.Domain.Events;

namespace TheMovie.Application.Movies.Events;

/// <summary>
/// Example handler for <see cref="MovieGenreChangedDomainEvent"/> illustrating correlation and idempotency.
/// </summary>
/// <remarks>
/// When updating denormalized views keyed by genre, ensure operations are idempotent. Consumers should recompute the
/// target state based on the latest aggregate data whenever possible.
/// </remarks>
public class MovieGenreChangedDomainEventHandler(ILogger<MovieGenreChangedDomainEventHandler> logger)
    : INotificationHandler<MovieGenreChangedDomainEvent>
{
    private readonly ILogger<MovieGenreChangedDomainEventHandler> _logger = logger;

    /// <summary>
    /// Handles the <see cref="MovieGenreChangedDomainEvent"/> by logging the change.
    /// </summary>
    public Task Handle(MovieGenreChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEvent: Movie genre changed. MovieId={MovieId}, OldGenre={OldGenre}, NewGenre={NewGenre}",
            notification.MovieId,
            notification.OldGenreId,
            notification.NewGenreId);
        return Task.CompletedTask;
    }
}

