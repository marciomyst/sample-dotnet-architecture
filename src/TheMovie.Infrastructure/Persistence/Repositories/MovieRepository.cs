using Microsoft.EntityFrameworkCore;
using TheMovie.Domain.Aggregates.MovieAggregate;
using TheMovie.Domain.Interfaces;

namespace TheMovie.Infrastructure.Persistence.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IMovieRepository"/> for the <see cref="Movie"/> aggregate.
/// </summary>
/// <remarks>
/// <para>
/// This repository encapsulates persistence operations against <see cref="TheMovieDbContext"/>, providing
/// add, update, and query capabilities for <see cref="Movie"/> aggregate roots. It cooperates with a Unit of
/// Work (e.g., via <c>DbContext.SaveChangesAsync</c>) to commit changes as a single transaction.
/// </para>
/// <para>
/// Retrieval methods typically use <see cref="EntityFrameworkQueryableExtensions.AsNoTracking{TEntity}(IQueryable{TEntity})"/>
/// to avoid change tracking when not required by the caller.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var repo = new MovieRepository(dbContext);
/// repo.Add(movie);
/// // ... additional operations
/// await dbContext.SaveChangesAsync(cancellationToken);
/// </code>
/// </para>
/// </remarks>
/// <param name="context">The EF Core database context used for persistence.</param>
public class MovieRepository(TheMovieDbContext context) : IMovieRepository, IDisposable
{
    private readonly TheMovieDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    /// <inheritdoc />
    public void Add(Movie movie)
    {
        ArgumentNullException.ThrowIfNull(movie);
        _context.Movies.Add(movie);
    }

    /// <inheritdoc />
    public void Update(Movie movie)
    {
        ArgumentNullException.ThrowIfNull(movie);
        _context.Movies.Update(movie);
    }

    /// <inheritdoc />
    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        return await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <inheritdoc />
    public async Task<Movie?> GetByTitleAsync(string title)
    {
        return await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Title == title);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
