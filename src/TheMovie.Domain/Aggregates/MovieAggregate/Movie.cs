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
    /// Title of the movie.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Required and non-empty. Updated via <see cref="Update(string, string, int, decimal, Guid, MovieRating)"/>.
    /// No automatic normalization is applied; callers/validators should enforce casing/trim and length limits.
    /// </para>
    /// </remarks>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Brief synopsis or summary of the movie plot.
    /// </summary>
    /// <remarks>
    /// Updated via <see cref="Update(string, string, int, decimal, Guid, MovieRating)"/>. Constraints like maximum
    /// length are validated at the application boundary.
    /// </remarks>
    public string Synopsis { get; private set; } = null!;

    /// <summary>
    /// Year in which the movie was released.
    /// </summary>
    /// <remarks>
    /// Must be greater than 1888. Updated via <see cref="Update(string, string, int, decimal, Guid, MovieRating)"/>.
    /// Additional checks (e.g., not in the future) are enforced by validators.
    /// </remarks>
    public int ReleaseYear { get; private set; }

    /// <summary>
    /// Current price of the movie.
    /// </summary>
    /// <remarks>
    /// Must be greater than zero. Changes should go through <see cref="UpdatePrice(decimal)"/>, which raises
    /// <see cref="MoviePriceChangedDomainEvent"/> when the value actually changes. Also invoked by
    /// <see cref="Update(string, string, int, decimal, Guid, MovieRating)"/>.
    /// </remarks>
    public decimal Price { get; private set; }

    /// <summary>
    /// Unique identifier of the movie's genre classification.
    /// </summary>
    /// <remarks>
    /// Must not be <see cref="Guid.Empty"/>. Changes should go through <see cref="ChangeGenre(Guid)"/>, which raises
    /// <see cref="MovieGenreChangedDomainEvent"/> when the value changes. Also invoked by
    /// <see cref="Update(string, string, int, decimal, Guid, MovieRating)"/>.
    /// </remarks>
    public Guid GenreId { get; private set; }

    /// <summary>
    /// Official content rating of the movie.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="MovieRating.Unrated"/> if not explicitly set. Updated via
    /// <see cref="Update(string, string, int, decimal, Guid, MovieRating)"/>.
    /// </remarks>
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
    /// Updates the movie's details, enforcing domain rules and raising domain events where applicable.
    /// </summary>
    /// <param name="title">The new movie title.</param>
    /// <param name="synopsis">The new synopsis.</param>
    /// <param name="releaseYear">The new release year.</param>
    /// <param name="price">The new price.</param>
    /// <param name="genreId">The new genre identifier.</param>
    /// <param name="rating">The new content rating.</param>
    /// <remarks>
    /// <para>
    /// Sets <see cref="Title"/>, <see cref="Synopsis"/>, <see cref="ReleaseYear"/> and <see cref="Rating"/> after basic validation,
    /// then delegates mutations of <see cref="Price"/> and <see cref="GenreId"/> to <see cref="UpdatePrice(decimal)"/> and
    /// <see cref="ChangeGenre(Guid)"/> so that domain events are raised only when values change.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// movie.Update(
    ///     title: "Inception (4K)",
    ///     synopsis: movie.Synopsis,
    ///     releaseYear: movie.ReleaseYear,
    ///     price: 21.99m,
    ///     genreId: movie.GenreId,
    ///     rating: movie.Rating);
    /// </code>
    /// </para>
    /// </remarks>
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

        UpdatePrice(price);
        ChangeGenre(genreId);
    }


    /// <summary>
    /// Updates the price to a new value, enforcing domain rules and raising a domain event.
    /// </summary>
    /// <param name="newPrice">New price. Must be greater than zero.</param>
    /// <remarks>
    /// <para>
    /// No-op when <paramref name="newPrice"/> equals the current <see cref="Price"/>. When the value changes, raises a
    /// <see cref="MoviePriceChangedDomainEvent"/> with both old and new prices.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// movie.UpdatePrice(19.99m);
    /// </code>
    /// </para>
    /// </remarks>
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
    /// <remarks>
    /// <para>
    /// No-op when <paramref name="newGenreId"/> equals the current <see cref="GenreId"/>. When the value changes, assigns the new
    /// identifier and raises a <see cref="MovieGenreChangedDomainEvent"/> with previous and current values.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// movie.ChangeGenre(newGenreId);
    /// </code>
    /// </para>
    /// </remarks>
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
}
