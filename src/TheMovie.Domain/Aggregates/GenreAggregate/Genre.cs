using TheMovie.Domain.Events;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Domain.Aggregates.GenreAggregate;

/// <summary>
/// Represents a film genre as an aggregate root in the domain model.
/// <para>
/// In Domain-Driven Design (DDD), a genre is modeled as a value object with identity, 
/// but in this context, it is also treated as an aggregate root to allow for persistence and domain event management.
/// Instances are considered equal if they share the same name, and are immutable after creation.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// This implementation handles equality directly without relying on a base ValueObject class.
/// The <see cref="Genre"/> class also raises a <see cref="GenreRegisteredDomainEvent"/> when a new genre is created,
/// enabling other parts of the system to react to this event.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var genre = new Genre("Action");
/// // The domain event is automatically registered upon creation.
/// </code>
/// </para>
/// </remarks>
public sealed class Genre : Entity, IAggregateRoot
{
    /// <summary>
    /// Name of the genre.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Required and immutable after construction. This value participates in equality: two <see cref="Genre"/>
    /// instances are considered equal when their <see cref="Name"/> values match (ordinal, case-sensitive comparison).
    /// </para>
    /// <para>
    /// No normalization is applied on assignment; callers are responsible for any canonicalization (e.g., casing
    /// or trimming) at the boundaries or via validators.
    /// </para>
    /// </remarks>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Private constructor for Entity Framework Core materialization.
    /// </summary>
    private Genre() { }

    /// <summary>
    /// Initializes a new instance of <see cref="Genre"/> with the specified name.
    /// </summary>
    /// <param name="name">The genre name. Must not be null, empty, or whitespace.</param>
    /// <returns>A new, valid <see cref="Genre"/> aggregate root.</returns>
    /// <remarks>
    /// <para>
    /// Enforces the invariant that <paramref name="name"/> is non-empty and immediately raises a
    /// <see cref="GenreRegisteredDomainEvent"/> signalling the registration of a new genre. No normalization
    /// (trimming/casing) is applied; pass canonicalized values if required by business rules.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
    public Genre(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Genre name cannot be empty.");
        }
        Name = name;

        AddDomainEvent(new GenreRegisteredDomainEvent(this));
    }

    /// <summary>
    /// Determines whether this instance and another <see cref="Genre"/> have the same name.
    /// </summary>
    /// <param name="other">The other genre to compare. Must not be <c>null</c>.</param>
    /// <returns><c>true</c> when both genres share the same <see cref="Name"/>; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Uses an ordinal, case-sensitive comparison on <see cref="Name"/>. This method does not guard against
    /// <paramref name="other"/> being <c>null</c>; pass a non-null instance.
    /// </remarks>
    public bool Equals(Genre other)
    {
        return Name == other.Name;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current genre.
    /// </summary>
    /// <param name="obj">The object to compare with the current genre.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Genre"/> with the same <see cref="Name"/>; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Handles <c>null</c> and reference-equality checks first, then defers to <see cref="Equals(Genre)"/>.
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((Genre) obj);
    }

    /// <summary>
    /// Returns a hash code for this genre derived from <see cref="Name"/>.
    /// </summary>
    /// <returns>An integer hash code consistent with <see cref="Equals(object)"/>.</returns>
    /// <remarks>
    /// Must remain consistent with the equality semantics: two equal genres (same <see cref="Name"/>) produce the same hash code.
    /// </remarks>
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
