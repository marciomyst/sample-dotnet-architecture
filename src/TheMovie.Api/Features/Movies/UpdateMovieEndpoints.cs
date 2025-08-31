using MediatR;
using TheMovie.Application.Movies.Commands.UpdateMovie;

namespace TheMovie.Api.Features.Movies;

public static class UpdateMovieEndpoints
{
    public static RouteGroupBuilder MapUpdateMovie(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}", async (IMediator mediator, Guid id, UpdateMovieCommand command) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateMovie")
        .WithTags("Movies");

        return group;
    }
}
