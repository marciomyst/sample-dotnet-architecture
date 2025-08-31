using Microsoft.AspNetCore.Mvc;

namespace TheMovie.Api.Features.Movies.DeleteMovie;

/// <summary>
/// Represents the HTTP request for deleting a movie resource.
/// </summary>
/// <remarks>
/// <para>
/// This DTO is populated via route binding from <c>DELETE /api/movies/{id}</c> and is used to keep the
/// endpoint lean and consistent with the feature pattern adopted across the API.
/// </para>
/// <para>
/// Example request:
/// <code><![CDATA[
/// DELETE /api/movies/815acc2d-2224-4253-b119-3c82e7286c07
/// ]]></code>
/// </para>
/// <para>
/// The request is mapped to an application command by <see cref="DeleteMovieMapper"/> and handled by the
/// application layer via MediatR.
/// </para>
/// </remarks>
/// <param name="Id">The unique identifier of the movie to delete, bound from the route.</param>
public record DeleteMovieRequest([FromRoute] Guid Id);
