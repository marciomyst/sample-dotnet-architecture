using MediatR;
using TheMovie.Application.Movies.Queries.GetMovieById;

namespace TheMovie.Api.Features.Movies;

public static class GetMovieByIdEndpoints
{
    public static RouteGroupBuilder MapGetMovieById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (IMediator mediator, Guid id) =>
        {
            var movie = await mediator.Send(new GetMovieByIdQuery(id));
            return movie is not null ? Results.Ok(movie) : Results.NotFound();
        }).WithName("GetMovieById");

        return group;
    }
}
