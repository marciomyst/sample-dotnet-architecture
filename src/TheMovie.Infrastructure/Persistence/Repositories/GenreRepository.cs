using Microsoft.EntityFrameworkCore;
using TheMovie.Domain.Aggregates.GenreAggregate;
using TheMovie.Domain.Interfaces;

namespace TheMovie.Infrastructure.Persistence.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IGenreRepository"/> for the <see cref="Genre"/> aggregate.
/// </summary>
/// <remarks>
/// <para>
/// Encapsulates persistence operations against <see cref="TheMovieDbContext"/> for <see cref="Genre"/> aggregates.
/// It leverages asynchronous methods and <see cref="CancellationToken"/> to support cooperative cancellation.
/// </para>
/// <para>
/// Retrieval methods use <see cref="EntityFrameworkQueryableExtensions.AsNoTracking{TEntity}(IQueryable{TEntity})"/>
/// to avoid tracking when not required. Changes are persisted when the calling application commits the unit of work
/// (e.g., <c>DbContext.SaveChangesAsync</c>).
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var repo = new GenreRepository(dbContext);
/// await repo.AddAsync(genre, cancellationToken);
/// // ... additional operations
/// await dbContext.SaveChangesAsync(cancellationToken);
/// </code>
/// </para>
/// </remarks>
/// <param name="context">The EF Core database context used for persistence.</param>
public class GenreRepository(TheMovieDbContext context) : IGenreRepository, IDisposable
{
    private readonly TheMovieDbContext context = context ?? throw new ArgumentNullException(nameof(context));

    /// <inheritdoc />
    public async Task AddAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(genre);
        await context.Set<Genre>().AddAsync(genre, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Set<Genre>()
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task RemoveAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(genre);
        context.Set<Genre>().Remove(genre);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
