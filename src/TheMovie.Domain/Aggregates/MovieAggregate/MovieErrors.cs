using Microsoft.Extensions.Localization;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Domain.Resources;

public static partial class DomainErrors
{
    /// <summary>
    /// Factory methods that create <see cref="TheMovie.Domain.SeedWork.Error"/> instances for the <see cref="Movie"/> aggregate.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each method emits an <see cref="Error"/> with a stable code (for example, <c>"Movie.NotFound"</c>) matching an entry in
    /// the <c>Errors.resx</c> resource file. The supplied <see cref="Microsoft.Extensions.Localization.IStringLocalizer"/>
    /// resolves a localized message based on that code.
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// var error = DomainErrors.MovieErrors.DuplicateTitle(localizer);
    /// // error.Code == "Movie.DuplicateTitle"
    /// // error.Message is localized from Errors.resx
    /// ]]></code>
    /// </para>
    /// </remarks>
    public static class MovieErrors
    {
        /// <summary>
        /// Creates an <see cref="Error"/> for a movie that was not found.
        /// </summary>
        /// <param name="localizer">The localizer used to resolve the resource message.</param>
        public static Error NotFound(IStringLocalizer localizer) =>
            new(nameof(Errors.Movie_NotFound), localizer[nameof(Errors.Movie_NotFound)]);

        /// <summary>
        /// Creates an <see cref="Error"/> for a movie that was not found, formatting with <paramref name="movieId"/> when supported by the resource.
        /// </summary>
        /// <param name="localizer">The localizer used to resolve the resource message.</param>
        /// <param name="movieId">An optional identifier to include in the message if the resource contains placeholders.</param>
        public static Error NotFound(IStringLocalizer localizer, Guid movieId) =>
            new(nameof(Errors.Movie_NotFound), localizer[nameof(Errors.Movie_NotFound), movieId]);

        /// <summary>
        /// Creates an <see cref="Error"/> indicating a duplicate movie title.
        /// </summary>
        /// <param name="localizer">The localizer used to resolve the resource message.</param>
        public static Error DuplicateTitle(IStringLocalizer localizer) =>
            new(nameof(Errors.Movie_DuplicateTitle), localizer[nameof(Errors.Movie_DuplicateTitle)]);

        /// <summary>
        /// Creates an <see cref="Error"/> indicating the release year is invalid (for example, in the future or outside acceptable range).
        /// </summary>
        /// <param name="localizer">The localizer used to resolve the resource message.</param>
        public static Error InvalidReleaseYear(IStringLocalizer localizer) =>
            new(nameof(Errors.Movie_InvalidReleaseYear), localizer[nameof(Errors.Movie_InvalidReleaseYear)]);

        /// <summary>
        /// Creates an <see cref="Error"/> indicating the price value is invalid (for example, negative or zero when positive is required).
        /// </summary>
        /// <param name="localizer">The localizer used to resolve the resource message.</param>s
        public static Error InvalidPrice(IStringLocalizer localizer) =>
            new(nameof(Errors.Movie_InvalidPrice), localizer[nameof(Errors.Movie_InvalidPrice)]);
    }
}
