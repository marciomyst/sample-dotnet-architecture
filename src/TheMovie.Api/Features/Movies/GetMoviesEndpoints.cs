using MediatR;
using TheMovie.Application.Movies.Queries.GetMovies;

namespace TheMovie.Api.Features.Movies;

public static class GetMoviesEndpoints
{
    public static RouteGroupBuilder MapGetMovies(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator) =>
        {
            var movies = await mediator.Send(new GetMoviesQuery(null, null, null));
            return Results.Ok(movies);
        }).WithName("GetMovies");

        return group;
    }
}
