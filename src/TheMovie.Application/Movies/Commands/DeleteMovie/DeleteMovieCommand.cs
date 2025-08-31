using MediatR;
using TheMovie.Application.Shared;

namespace TheMovie.Application.Movies.Commands.DeleteMovie;

/// <summary>
/// Represents a request to delete an existing movie from the system.
/// </summary>
/// <remarks>
/// <para>
/// This command follows the CQRS pattern and is handled by
/// <see cref="DeleteMovieCommandHandler"/>. It returns a <see cref="Result"/> indicating success or failure
/// without a payload.
/// </para>
/// <para>
/// Example usage with MediatR:
/// <code><![CDATA[
/// Result result = await mediator.Send(new DeleteMovieCommand(id));
/// if (result.IsFailure)
/// {
///     // inspect result.Errors
/// }
/// ]]></code>
/// </para>
/// </remarks>
/// <param name="Id">The unique identifier of the movie to delete.</param>
public record DeleteMovieCommand(Guid Id) : IRequest<Result>;
