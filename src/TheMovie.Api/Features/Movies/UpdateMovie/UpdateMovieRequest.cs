using Microsoft.AspNetCore.Mvc;

namespace TheMovie.Api.Features.Movies.UpdateMovie;

/// <summary>
/// Represents the HTTP request to update an existing movie.
/// </summary>
/// <remarks>
/// <para>
/// Combines the route identifier from <c>PUT /api/movies/{id}</c> with a JSON body containing the updated fields.
/// Validation is performed in the application layer via FluentValidation before the handler executes.
/// </para>
/// <para>
/// Example body payload:
/// <code><![CDATA[
/// {
///   "title": "Pulp Fiction (Remastered)",
///   "synopsis": "Updated synopsis...",
///   "releaseYear": 1994,
///   "price": 25.00,
///   "genreId": "f4e5e6f7-1234-5678-9abc-def012345678",
///   "rating": "R"
/// }
/// ]]></code>
/// </para>
/// </remarks>
/// <param name="Id">The movie identifier bound from the route.</param>
/// <param name="Body">The request body with the updated movie fields.</param>
public record UpdateMovieRequest(
    [FromRoute] Guid Id,
    [FromBody] UpdateMovieRequestBody Body
);
