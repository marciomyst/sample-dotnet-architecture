using TheMovie.Application.Movies.Commands.DeleteMovie;

namespace TheMovie.Api.Features.Movies.DeleteMovie;

/// <summary>
/// Maps HTTP-layer DTOs to application-layer commands for the DeleteMovie feature.
/// </summary>
/// <remarks>
/// <para>
/// Keeps the Minimal API endpoint focused on transport concerns by centralizing the translation to MediatR commands.
/// </para>
/// <para>
/// Example:
/// <code><![CDATA[
/// var command = request.ToCommand();
/// var result  = await mediator.Send(command, cancellationToken);
/// ]]></code>
/// </para>
/// </remarks>
internal static class DeleteMovieMapper
{
    /// <summary>
    /// Converts a <see cref="DeleteMovieRequest"/> into a <see cref="DeleteMovieCommand"/>.
    /// </summary>
    /// <param name="request">The incoming HTTP request DTO.</param>
    /// <returns>A <see cref="DeleteMovieCommand"/> ready to be sent via MediatR.</returns>
    public static DeleteMovieCommand ToCommand(this DeleteMovieRequest request)
        => new(request.Id);
}
