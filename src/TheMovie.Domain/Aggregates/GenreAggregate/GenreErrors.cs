using Microsoft.Extensions.Localization;
using TheMovie.Domain.SeedWork;

namespace TheMovie.Domain.Resources;

public static partial class DomainErrors
{
    /// <summary>
    /// Factory methods that create <see cref="TheMovie.Domain.SeedWork.Error"/> instances for the <see cref="Genre"/> aggregate.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Methods in this class use stable error codes (for example, <c>"Genre.NotFound"</c>) that correspond to entries in the
    /// <c>Errors.resx</c> resource file. The provided <see cref="Microsoft.Extensions.Localization.IStringLocalizer"/>
    /// resolves the localized message for the code, while the <see cref="Error"/> instance carries both the code and message.
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// var error = DomainErrors.GenreErrors.NotFound(localizer);
    /// // error.Code == "Genre.NotFound"
    /// // error.Message is localized from Errors.resx
    /// ]]></code>
    /// </para>
    /// </remarks>
    public static class GenreErrors
    {
        private const string NotFoundCode = "Genre.NotFound";

        /// <summary>
        /// Creates an <see cref="Error"/> for a genre that was not found.
        /// </summary>
        /// <param name="localizer">The localizer used to resolve the resource message.</param>
        public static Error NotFound(IStringLocalizer localizer) =>
            new(NotFoundCode, localizer[NotFoundCode]);

        /// <summary>
        /// Creates an <see cref="Error"/> for a genre that was not found, formatting with <paramref name="genreId"/> when supported by the resource.
        /// </summary>
        /// <param name="localizer">The localizer used to resolve the resource message.</param>
        /// <param name="genreId">An optional identifier to include in the message if the resource contains placeholders.</param>
        public static Error NotFound(IStringLocalizer localizer, Guid genreId) =>
            new(NotFoundCode, localizer[NotFoundCode, genreId]);
    }
}
