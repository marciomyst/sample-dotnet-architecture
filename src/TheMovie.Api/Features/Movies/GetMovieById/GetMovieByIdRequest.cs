using Microsoft.AspNetCore.Mvc;

namespace TheMovie.Api.Features.Movies.GetMovieById;

/// <summary>
/// Represents the HTTP request for retrieving a movie by its identifier.
/// </summary>
/// <param name="Id">The unique identifier of the movie, bound from the route.</param>
public record GetMovieByIdRequest([FromRoute] Guid Id);

