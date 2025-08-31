using TheMovie.Api.Features.Movies.CreateMovie;
using TheMovie.Api.Features.Movies.DeleteMovie;
using TheMovie.Api.Features.Movies.UpdateMovie;

namespace TheMovie.Api.Features.Movies;

/// <summary>
/// Centralizes registration of all Movie feature endpoints.
/// </summary>
/// <remarks>
/// <para>
/// Keeps <c>Program.cs</c> concise by exposing a single method to wire up all endpoints related to movies.
/// Internally, it delegates to each feature-specific mapper (Create, Delete, etc.).
/// </para>
/// <para>
/// Example:
/// <code>
/// app.MapMovieEndpoints();
/// </code>
/// </para>
/// </remarks>
public static class MoviesEndpoints
{
    /// <summary>
    /// Maps all movie-related endpoints.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateMovie();
        app.MapDeleteMovie();
        app.MapUpdateMovie();
        // Future features:
        // app.MapGetMovieById();
        // app.MapGetMovies();
    }
}
