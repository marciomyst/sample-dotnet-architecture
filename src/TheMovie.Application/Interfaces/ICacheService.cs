namespace TheMovie.Application.Interfaces;

/// <summary>
/// Defines a minimal, async-first contract for cache operations.
/// </summary>
/// <remarks>
/// <para>
/// The cache stores arbitrary values keyed by strings and supports optional absolute expiration.
/// Implementations may be in-memory or distributed; callers should treat values as ephemeral and idempotently recomputable.
/// </para>
/// <para>
/// Key guidance: prefer namespaced keys (e.g., <c>"movie:by-id:42"</c>) and keep them stable across the app to maximize reuse.
/// </para>
/// <para>
/// Example:
/// <code>
/// var key = $"movie:by-id:{id}";
/// var cached = await cache.GetAsync<Movie>(key);
/// if (cached is null)
/// {
///     var fresh = await repository.GetByIdAsync(id);
///     if (fresh is not null)
///     {
///         await cache.SetAsync(key, fresh, TimeSpan.FromMinutes(5));
///     }
///     return fresh;
/// }
/// return cached;
/// </code>
/// </para>
/// </remarks>
public interface ICacheService
{
    /// <summary>
    /// Retrieves a value from the cache associated with the specified key.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is non-throwing for cache-miss scenarios: when no entry exists or it has expired,
    /// it simply returns <c>null</c>. Implementations should avoid throwing for transient cache issues
    /// and allow callers to fall back to the source of truth when appropriate.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var key = $"movie:by-id:{id}";
    /// Movie? movie = await cache.GetAsync<Movie>(key);
    /// if (movie is null)
    /// {
    ///     movie = await repository.GetByIdAsync(id);
    ///     if (movie is not null)
    ///     {
    ///         await cache.SetAsync(key, movie, TimeSpan.FromMinutes(5));
    ///     }
    /// }
    /// return movie;
    /// </code>
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    /// <param name="key">The cache key to look up.</param>
    /// <returns>The cached value, or <c>null</c> if not found or expired.</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Inserts or updates a value in the cache with an optional absolute expiration.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Uses absolute expiration relative to the current time when <paramref name="absoluteExpirationRelativeToNow"/> is provided.
    /// If omitted, the implementation's default TTL (time-to-live) is applied. Prefer short TTLs for volatile data
    /// and longer TTLs for read-heavy, infrequently changing data. Keys should be stable and namespaced.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var key = $"movie:by-id:{id}";
    /// await cache.SetAsync(key, movie, TimeSpan.FromMinutes(5));
    /// </code>
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key to set.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="absoluteExpirationRelativeToNow">Optional expiration relative to now; if <c>null</c>, uses the implementation default.</param>
    Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);

    /// <summary>
    /// Removes the value associated with the specified key from the cache.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation should be idempotent: removing a non-existent key must not throw and is considered a no-op.
    /// Prefer targeted invalidation for single-entity updates and, when necessary, use namespaced keys to enable
    /// coarse-grained invalidation (e.g., evict all keys with a given prefix using provider-specific capabilities).
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var key = $"movie:by-id:{id}";
    /// await cache.RemoveAsync(key);
    /// </code>
    /// </para>
    /// </remarks>
    /// <param name="key">The cache key to remove.</param>
    Task RemoveAsync(string key);
}
