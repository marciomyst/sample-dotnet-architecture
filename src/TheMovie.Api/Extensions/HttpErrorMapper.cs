using Microsoft.AspNetCore.Mvc;
using TheMovie.Application.Shared;

namespace TheMovie.Api.Extensions;

/// <summary>
/// Maps application <see cref="Result"/> failures to RFC 7807 <see cref="ProblemDetails"/> responses.
/// </summary>
/// <remarks>
/// <para>
/// Determines the HTTP status code from the first error's code using simple conventions
/// (e.g., codes ending with <c>.NotFound</c> map to 404, codes containing <c>Duplicate</c> map to 409).
/// All errors contained in the <see cref="Result"/> are exposed under <c>problemDetails.Extensions["errors"]</c>.
/// </para>
/// <para>
/// Example:
/// <code><![CDATA[
/// app.MapPost("/movies", async (IMediator mediator, CreateMovieCommand cmd) =>
/// {
///     var result = await mediator.Send(cmd);
///     return result.IsSuccess
///         ? Results.Created($"/movies/{result.Value}", new { id = result.Value })
///         : result.ToProblemDetails();
/// });
/// ]]></code>
/// </para>
/// </remarks>
public static class HttpErrorMapper
{
    /// <summary>
    /// Converts a failed <see cref="Result"/> into an <see cref="IResult"/> with an RFC 7807 <see cref="ProblemDetails"/> body.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Mapping rules:
    /// - Uses the first error's <c>Code</c> to infer the HTTP status (e.g., codes ending with <c>NotFound</c> map to 404,
    ///   codes containing <c>Duplicate</c> to 409, codes containing <c>Unauthorized</c> to 401; otherwise 400).
    /// - Returns a <see cref="ProblemDetails"/> with <c>Status</c>, a default <c>Title</c> from
    ///   <see cref="GetTitleFromStatusCode(int)"/>, and attaches the full error list under <c>Extensions["errors"]</c>.
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// var result = await mediator.Send(command);
    /// return result.IsFailure ? result.ToProblemDetails() : Results.NoContent();
    /// ]]></code>
    /// Produz, por exemplo, 404 quando o primeiro erro tem código <c>Movie.NotFound</c>.
    /// </para>
    /// </remarks>
    /// <param name="result">The failed result to convert.</param>
    /// <returns>An HTTP result with the appropriate status code and problem details payload.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="result"/> is successful.</exception>
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert a successful result to a problem details response.");
        }

        var firstError = result.Errors.First();

        int statusCode = firstError.Code switch
        {
            var code when code.EndsWith("NotFound") => StatusCodes.Status404NotFound,
            var code when code.Contains("Duplicate") => StatusCodes.Status409Conflict,
            var code when code.Contains("Unauthorized") => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitleFromStatusCode(statusCode),
            Extensions =
            {
                ["errors"] = result.Errors
            }
        };

        return Results.Problem(problemDetails);
    }

    /// <summary>
    /// Returns a default RFC 7807 title for a given HTTP status code.
    /// </summary>
    /// <remarks>
    /// Covers common client error statuses used by this API. For unmapped codes, returns a generic title.
    /// </remarks>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns>A short, human-readable title recommended by RFC 7807.</returns>
    private static string GetTitleFromStatusCode(int statusCode) =>
        statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status409Conflict => "Conflict",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            _ => "An error occurred"
        };
}
