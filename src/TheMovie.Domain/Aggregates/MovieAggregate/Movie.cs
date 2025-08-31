using TheMovie.Domain.Events;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Domain.Aggregates.MovieAggregate;

/// <summary>
/// Represents a film entity as the aggregate root within the Movie aggregate.
/// <para>
/// In Domain-Driven Design (DDD), the <see cref="Movie"/> class enforces domain invariants and encapsulates
/// all business rules and behaviors related to a movie. It is responsible for managing its own state and
/// raising domain events when significant changes occur.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="Movie"/> aggregate root ensures that all invariants are maintained, such as valid title,
/// release year, and price. Domain events are raised to signal important changes, such as registration or
/// genre changes, allowing other parts of the system to react accordingly.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var movie = new Movie("Inception", "A mind-bending thriller.", 2010, 15.0m, genreId, MovieRating.PG13);
/// movie.UpdatePrice(18.0m);
/// movie.ChangeGenre(newGenreId);
/// </code>
/// </para>
/// </remarks>
public class Movie : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets the title of the movie.
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Gets a brief synopsis or summary of the movie plot.
    /// </summary>
    public string Synopsis { get; private set; } = null!;

    /// <summary>
    /// Gets the year in which the movie was released.
    /// </summary>
    public int ReleaseYear { get; private set; }

    /// <summary>
    /// Gets the current price of the movie.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the genre classification of the movie.
    /// </summary>
    public Guid GenreId { get; private set; }

    /// <summary>
    /// Gets the official content rating of the movie.
    /// </summary>
    public MovieRating Rating { get; private set; }

    /// <summary>
    /// Private constructor required for Entity Framework Core materialization.
    /// </summary>
    private Movie() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Movie"/> class with the specified parameters,
    /// ensuring the entity is always in a valid state.
    /// </summary>
    /// <param name="title">The movie title. Must not be null or whitespace.</param>
    /// <param name="synopsis">A short description of the movie plot.</param>
    /// <param name="releaseYear">
    /// The year the movie was released.
    /// Must be greater than 1888 (the year of the first known film).
    /// </param>
    /// <param name="price">
    /// The price to view the movie.
    /// Must be greater than zero.
    /// </param>
    /// <param name="genreId">The unique identifier of the movie genre.</param>
    /// <param name="rating">The official content rating of the movie. Defaults to <see cref="MovieRating.Unrated"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="title"/> is null or whitespace.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="releaseYear"/> is less than or equal to 1888,
    /// or when <paramref name="price"/> is less than or equal to zero.
    /// </exception>
    public Movie(
        string title,
        string synopsis,
        int releaseYear,
        decimal price,
        Guid genreId,
        MovieRating rating = MovieRating.Unrated)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title), "Movie title cannot be empty.");
        }

        if (releaseYear <= 1888)
        {
            throw new ArgumentOutOfRangeException(nameof(releaseYear), "Release year must be greater than 1888.");
        }

        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");
        }

        Title = title;
        Synopsis = synopsis;
        ReleaseYear = releaseYear;
        Price = price;
        GenreId = genreId;
        Rating = rating;

        AddDomainEvent(new MovieRegisteredDomainEvent(this));
    }

    /// <summary>
    /// Updates the price to a new value, enforcing domain rules and raising a domain event.
    /// </summary>
    /// <param name="newPrice">New price. Must be greater than zero.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="newPrice"/> is less than or equal to zero.
    /// </exception>
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newPrice), "New price must be greater than zero.");
        }

        if (newPrice == Price)
        {
            return;
        }

        decimal oldPrice = Price;
        Price = newPrice;

        AddDomainEvent(new MoviePriceChangedDomainEvent(Id, oldPrice, newPrice));
    }

    /// <summary>
    /// Changes the genre classification of the movie.
    /// </summary>
    /// <param name="newGenreId">The new genre identifier. Must not be <see cref="Guid.Empty"/>.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="newGenreId"/> is <see cref="Guid.Empty"/>.</exception>
    public void ChangeGenre(Guid newGenreId)
    {
        if (newGenreId == Guid.Empty)
        {
            throw new ArgumentException("New genre cannot be null.", nameof(newGenreId));
        }

        if (newGenreId == GenreId)
        {
            return;
        }

        Guid oldGenreId = GenreId;

        GenreId = newGenreId;

        AddDomainEvent(new MovieGenreChangedDomainEvent(Id, oldGenreId, newGenreId));
    }

    /// <summary>
    /// Updates the movie's details, enforcing domain rules and raising domain events where applicable.
    /// </summary>
    /// <param name="title">The new movie title.</param>
    /// <param name="synopsis">The new synopsis.</param>
    /// <param name="releaseYear">The new release year.</param>
    /// <param name="price">The new price.</param>
    /// <param name="genreId">The new genre identifier.</param>
    /// <param name="rating">The new content rating.</param>
    public void Update(
        string title,
        string synopsis,
        int releaseYear,
        decimal price,
        Guid genreId,
        MovieRating rating)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title), "Movie title cannot be empty.");
        }

        if (releaseYear <= 1888)
        {
            throw new ArgumentOutOfRangeException(nameof(releaseYear), "Release year must be greater than 1888.");
        }

        Title = title;
        Synopsis = synopsis;
        ReleaseYear = releaseYear;
        Rating = rating;

        // Use existing methods to ensure domain events are raised for price and genre changes.
        UpdatePrice(price);
        ChangeGenre(genreId);
    }
}
