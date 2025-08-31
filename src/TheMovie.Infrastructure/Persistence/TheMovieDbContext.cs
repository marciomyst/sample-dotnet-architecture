using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TheMovie.Domain.Aggregates.MovieAggregate;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Infrastructure.Persistence;

/// <summary>
///     Entity Framework Core database context for the Movie application domain.
///     <para>
///         Implements the <see cref="IUnitOfWork"/> pattern, ensuring that all changes to aggregates
///         are coordinated and persisted as a single transaction. Also responsible for dispatching
///         domain events raised by aggregate roots before committing changes.
///     </para>
/// </summary>
/// <remarks>
///     <para>
///         This context should be used as the main entry point for all data access and persistence
///         operations related to the Movie domain. It exposes <see cref="DbSet{TEntity}"/> properties
///         only for aggregate roots, in accordance with DDD best practices.
///     </para>
///     <para>
///         Domain events are dispatched via <see cref="IMediator"/> before changes are saved,
///         ensuring side effects and business rules are handled consistently.
///     </para>
///     <para>
///         Usage example:
///         <code>
///         await using var context = new TheMovieDbContext(options, mediator);
///         var movie = new Movie(...);
///         context.Movies.Add(movie);
///         await context.SaveEntitiesAsync();
///         </code>
///     </para>
/// </remarks>
/// <param name="options">
///     The options to be used by the DbContext, typically configured via dependency injection.
/// </param>
/// <param name="mediator">
///     The mediator instance for publishing domain events. Must not be <c>null</c>.
/// </param>
/// <exception cref="ArgumentNullException">
///     Thrown if <paramref name="mediator"/> is <c>null</c>.
/// </exception>
public class TheMovieDbContext(DbContextOptions<TheMovieDbContext> options, IMediator mediator)
    : DbContext(options), IUnitOfWork
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    /// <summary>
    ///     Gets the <see cref="DbSet{TEntity}"/> for <see cref="Movie"/> aggregate roots.
    ///     <para>
    ///         This property should be used for querying and persisting <see cref="Movie"/> entities,
    ///         which act as aggregate roots in the Movie domain.
    ///     </para>
    /// </summary>
    public DbSet<Movie> Movies => Set<Movie>();

    /// <summary>
    ///     Configures the EF Core model by applying all entity configurations from the current assembly.
    ///     <para>
    ///         This method is called by the EF Core runtime and should not be invoked directly.
    ///         It ensures that all <see cref="IEntityTypeConfiguration{TEntity}"/> implementations
    ///         in the assembly are applied, supporting a modular and maintainable model configuration.
    ///     </para>
    /// </summary>
    /// <param name="modelBuilder">
    ///     The builder used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    ///     Saves all changes made in this context to the database and dispatches domain events.
    ///     <para>
    ///         This method should be used instead of <see cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    ///         to ensure that all domain events are published before the transaction is committed.
    ///     </para>
    /// </summary>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the operation succeeds; otherwise, an exception is thrown.
    /// </returns>
    /// <remarks>
    ///     This method implements the <see cref="IUnitOfWork"/> contract and should be the
    ///     preferred way to persist changes in the application layer.
    /// </remarks>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    ///     Dispatches all domain events for tracked entities using the mediator.
    ///     <para>
    ///         This method is called internally by <see cref="SaveEntitiesAsync"/> to ensure
    ///         that all side effects and business rules encapsulated as domain events are
    ///         handled before the transaction is committed.
    ///     </para>
    /// </summary>
    /// <returns>
    ///     A task representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     After dispatching, all domain events are cleared from the entities to prevent
    ///     duplicate processing.
    /// </remarks>
    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (INotification? domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
