using Microsoft.AspNetCore.Mvc;
using TheMovie.Domain.Aggregates.MovieAggregate;

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
/// <param name="Title">The title of the movie, bound from the request body.</param>
/// <param name="Synopsis">A brief synopsis of the movie plot, bound from the request body.</param>
/// <param name="ReleaseYear">The year the movie was released (must be greater than 1888), bound from the request body.</param>
/// <param name="Price">The ticket price for the movie (must be greater than zero), bound from the request body.</param>
/// <param name="GenreId">The unique identifier of the movie's genre, bound from the request body.</param>
/// <param name="Rating">The content rating of the movie, bound from the request body.</param>
public record CreateMovieRequest(
    [FromBody] string Title,
    [FromBody] string Synopsis,
    [FromBody] int ReleaseYear,
    [FromBody] decimal Price,
    [FromBody] Guid GenreId,
    [FromBody] MovieRating Rating
);
