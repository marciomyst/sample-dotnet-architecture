using Swashbuckle.AspNetCore.Filters;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Api.Features.Movies.UpdateMovie;

/// <summary>
/// Provides a realistic example of a <see cref="UpdateMovieRequest"/> for OpenAPI/Swagger documentation.
/// </summary>
/// <remarks>
/// <para>
/// Used with Swashbuckle's example filters to enrich the request schema with a meaningful sample payload.
/// Register via <c>services.AddSwaggerExamplesFromAssemblyOf&lt;UpdateMovieRequestExample&gt;()</c> and
/// <c>c.ExampleFilters()</c>.
/// </para>
/// <para>
/// Registration example:
/// <code><![CDATA[
/// services.AddSwaggerGen(c =>
/// {
///     c.ExampleFilters();
/// });
/// services.AddSwaggerExamplesFromAssemblyOf<UpdateMovieRequestExample>();
/// ]]></code>
/// </para>
/// </remarks>
public class UpdateMovieRequestExample : IExamplesProvider<UpdateMovieRequest>
{
    /// <summary>
    /// Returns an example <see cref="UpdateMovieRequest"/> with representative values.
    /// </summary>
    /// <returns>A populated <see cref="UpdateMovieRequest"/> object.</returns>
    public UpdateMovieRequest GetExamples()
    {
        return new(
            Id: Guid.Parse("f4e5e6f7-1234-5678-9abc-def012345678"),
            Title: "Pulp Fiction (Remastered)",
            Synopsis: "The classic Tarantino film with remastered audio and scenes.",
            ReleaseYear: 1994,
            Price: 25.00m,
            GenreId: Guid.Parse("f4e5e6f7-1234-5678-9abc-def012345678"),
            Rating: MovieRating.R
        );
    }
}

