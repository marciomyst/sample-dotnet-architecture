using Swashbuckle.AspNetCore.Filters;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Api.Features.Movies.CreateMovie;

/// <summary>
/// Provides a realistic example of a <see cref="CreateMovieRequest"/> for OpenAPI/Swagger documentation.
/// </summary>
/// <remarks>
/// <para>
/// Used with Swashbuckle's example filters to enrich the request schema with a meaningful sample payload.
/// Register via <c>services.AddSwaggerExamplesFromAssemblyOf&lt;CreateMovieRequestExample&gt;()</c> and
/// <c>c.ExampleFilters()</c>.
/// </para>
/// <para>
/// Registration example:
/// <code><![CDATA[
/// services.AddSwaggerGen(c =>
/// {
///     c.ExampleFilters();
/// });
/// services.AddSwaggerExamplesFromAssemblyOf<CreateMovieRequestExample>();
/// ]]></code>
/// </para>
/// </remarks>
public class CreateMovieRequestExample : IExamplesProvider<CreateMovieRequest>
{
    /// <summary>
    /// Returns an example <see cref="CreateMovieRequest"/> with representative values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used by Swashbuckle Example Filters to enrich the request schema with a concrete payload. The
    /// example uses a deterministic <c>GenreId</c> to ensure stable documentation/tests. The structure
    /// mirrors the nested Body pattern adopted by this API, i.e., <see cref="CreateMovieRequest"/> wraps
    /// the JSON body inside its <c>Body</c> property.
    /// </para>
    /// <para>
    /// Example (shape):
    /// <code><![CDATA[
    /// {
    ///   "body": {
    ///     "title": "Pulp Fiction",
    ///     "synopsis": "The lives of two mob hitmen...",
    ///     "releaseYear": 1994,
    ///     "price": 22.50,
    ///     "genreId": "f4e5e6f7-1234-5678-9abc-def012345678",
    ///     "rating": "R"
    ///   }
    /// }
    /// ]]></code>
    /// </para>
    /// </remarks>
    /// <returns>A populated <see cref="CreateMovieRequest"/> object.</returns>
    public CreateMovieRequest GetExamples()
    {
        return new(
            Body: new CreateMovieRequestBody(
                Title: "Pulp Fiction",
                Synopsis: "The lives of two mob hitmen, a boxer, a gangster's wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                ReleaseYear: 1994,
                Price: 22.50m,
                GenreId: Guid.Parse("f4e5e6f7-1234-5678-9abc-def012345678"),
                Rating: MovieRating.R
            )
        );
    }
}
