using Swashbuckle.AspNetCore.Filters;

namespace TheMovie.Api.Features.Movies.CreateMovie;

/// <summary>
/// Provides a realistic example of a <see cref="CreateMovieResponse"/> for OpenAPI/Swagger documentation.
/// </summary>
/// <remarks>
/// <para>
/// Used with Swashbuckle's example filters to enrich the response schema with a meaningful sample payload.
/// Register via <c>services.AddSwaggerExamplesFromAssemblyOf&lt;CreateMovieResponseExample&gt;()</c> and
/// <c>c.ExampleFilters()</c>.
/// </para>
/// <para>
/// Registration example:
/// <code><![CDATA[
/// services.AddSwaggerGen(c =>
/// {
///     c.ExampleFilters();
/// });
/// services.AddSwaggerExamplesFromAssemblyOf<CreateMovieResponseExample>();
/// ]]></code>
/// </para>
/// </remarks>
public class CreateMovieResponseExample : IExamplesProvider<CreateMovieResponse>
{
    /// <summary>
    /// Returns an example <see cref="CreateMovieResponse"/> with a representative identifier.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used by Swashbuckle Example Filters to demonstrate the typical 201 Created response body returned
    /// by the Create endpoint. In addition to this JSON body, the endpoint also sets the Location header
    /// pointing to the newly created resource (route name "GetMovieById").
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// {
    ///   "id": "815acc2d-2224-4253-b119-3c82e7286c07"
    /// }
    /// ]]></code>
    /// </para>
    /// </remarks>
    /// <returns>A populated <see cref="CreateMovieResponse"/> object.</returns>
    public CreateMovieResponse GetExamples()
    {
        return new (
            Id: Guid.Parse("815acc2d-2224-4253-b119-3c82e7286c07")
        );
    }
}
