namespace TheMovie.Domain.Aggregates.MovieAggregate;

/// <summary>
/// Enumerates the official content ratings for movies, indicating suitable audience ages
/// and guidance levels for parents or guardians.
/// <para>
/// These ratings are commonly used by film classification boards to help viewers and parents
/// make informed decisions about the content and appropriateness of a movie.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="MovieRating"/> enum is used to classify movies according to their content,
/// providing guidance on age-appropriateness and parental advisories. These values are typically
/// assigned by official rating organizations and may influence how and where a movie is distributed.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var rating = MovieRating.PG13;
/// if (rating == MovieRating.R)
/// {
///     // Restrict access for viewers under 17
/// }
/// </code>
/// </para>
/// </remarks>
public enum MovieRating
{
    /// <summary>
    /// No official rating assigned.
    /// </summary>
    Unrated = 0,

    /// <summary>
    /// General Audiences: Suitable for all ages; contains no content that would offend parents.
    /// </summary>
    G = 1,

    /// <summary>
    /// Parental Guidance Suggested: Some material may not be suitable for children;
    /// parents are urged to give guidance.
    /// </summary>
    PG = 2,

    /// <summary>
    /// Parents Strongly Cautioned: Contains material that may be inappropriate for children
    /// under 13; parents are strongly urged to provide guidance.
    /// </summary>
    PG13 = 3,

    /// <summary>
    /// Restricted: Contains adult material; viewers under 17 require accompanying parent
    /// or adult guardian.
    /// </summary>
    R = 4,

    /// <summary>
    /// Adults Only: Contains content that is only appropriate for adults; no one under 17
    /// admitted.
    /// </summary>
    NC17 = 5
}
