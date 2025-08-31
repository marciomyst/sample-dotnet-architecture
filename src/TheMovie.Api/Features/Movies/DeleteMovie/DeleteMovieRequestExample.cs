using Swashbuckle.AspNetCore.Filters;

namespace TheMovie.Api.Features.Movies.DeleteMovie;

/// <summary>
/// Provides a realistic example of a <see cref="DeleteMovieRequest"/> for OpenAPI/Swagger documentation.
/// </summary>
/// <remarks>
/// <para>
/// Used with Swashbuckle's example filters to enrich the request schema with a meaningful sample payload.
/// Register via <c>services.AddSwaggerExamplesFromAssemblyOf&lt;DeleteMovieRequestExample&gt;()</c> and
/// <c>c.ExampleFilters()</c>.
/// </para>
/// <para>
/// Registration example:
/// <code><![CDATA[
/// services.AddSwaggerGen(c =>
/// {
///     c.ExampleFilters();
/// });
/// services.AddSwaggerExamplesFromAssemblyOf<DeleteMovieRequestExample>();
/// ]]></code>
/// </para>
/// </remarks>
public class DeleteMovieRequestExample : IExamplesProvider<DeleteMovieRequest>
{
    /// <summary>
    /// Returns an example <see cref="DeleteMovieRequest"/> with representative values.
    /// </summary>
    /// <returns>A populated <see cref="DeleteMovieRequest"/> object.</returns>
    public DeleteMovieRequest GetExamples()
    {
        return new(
            Id: Guid.Parse("815acc2d-2224-4253-b119-3c82e7286c07") 
        );
    }
}
