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
    /// Gets the name of the genre.
    /// </summary>
    /// <remarks>
    /// The name is immutable after construction and is used for equality comparison.
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
    /// <param name="other">The other genre to compare.</param>
    /// <returns>True if both genres share the same name; otherwise, false.</returns>
    public bool Equals(Genre other)
    {
        return Name == other.Name;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current genre.
    /// </summary>
    /// <param name="obj">The object to compare with the current genre.</param>
    /// <returns>
    /// True if the object is a <see cref="Genre"/> and has the same name; otherwise, false.
    /// </returns>
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
    /// Returns the hash code for this genre, based on its name.
    /// </summary>
    /// <returns>A hash code derived from the genre name.</returns>
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
