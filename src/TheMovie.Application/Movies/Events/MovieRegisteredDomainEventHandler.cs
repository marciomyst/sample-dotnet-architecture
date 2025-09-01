using MediatR;
using Microsoft.Extensions.Logging;
using TheMovie.Domain.Events;

namespace TheMovie.Application.Movies.Events;

/// <summary>
/// Example handler for <see cref="MovieRegisteredDomainEvent"/> demonstrating best practices
/// (idempotency, correlation, and minimal side effects by default).
/// </summary>
/// <remarks>
/// <para>
/// Handlers should be idempotent since domain events can be retried. Prefer side-effect operations that can
/// tolerate duplicates (e.g., upserts) or use de-duplication strategies when interacting with external systems.
/// </para>
/// <para>
/// This sample logs the occurrence; adapt to update read models or send notifications as needed.
/// </para>
/// </remarks>
public class MovieRegisteredDomainEventHandler(ILogger<MovieRegisteredDomainEventHandler> logger)
    : INotificationHandler<MovieRegisteredDomainEvent>
{
    private readonly ILogger<MovieRegisteredDomainEventHandler> _logger = logger;

    /// <summary>
    /// Handles the <see cref="MovieRegisteredDomainEvent"/>.
    /// </summary>
    /// <param name="notification">The domain event notification.</param>
    /// <param name="cancellationToken">Token to observe while processing.</param>
    public Task Handle(MovieRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEvent: Movie registered. MovieId={MovieId}, Title={Title}",
            notification.Movie.Id,
            notification.Movie.Title);
        return Task.CompletedTask;
    }
}

