using Microsoft.AspNetCore.Mvc;

namespace TheMovie.Api.Features.Movies.CreateMovie;

/// <summary>
/// Represents the HTTP request to create a new movie.
/// </summary>
/// <remarks>
/// <para>
/// This DTO is bound from the request body of <c>POST /api/movies</c>. Validation is performed by the
/// application layer using FluentValidation before the command handler executes.
/// </para>
/// <para>
/// Example payload:
/// <code><![CDATA[
/// {
///   "title": "Pulp Fiction",
///   "synopsis": "The lives of two mob hitmen...",
///   "releaseYear": 1994,
///   "price": 22.50,
///   "genreId": "f4e5e6f7-1234-5678-9abc-def012345678",
///   "rating": "R"
/// }
/// ]]></code>
/// </para>
/// </remarks>
/// <param name="Body">The request payload containing all movie fields.</param>
public record CreateMovieRequest(
    [FromBody] CreateMovieRequestBody Body
);
