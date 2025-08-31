using Microsoft.AspNetCore.Mvc;
using TheMovie.Application.Shared;

namespace TheMovie.Api.Extensions
{
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
        /// Converts a failed <see cref="Result"/> into an <see cref="IResult"/> containing a <see cref="ProblemDetails"/> payload.
        /// </summary>
        /// <param name="result">The result to convert. Must represent a failure.</param>
        /// <returns>An HTTP result with status code and problem details describing the failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="result"/> is successful.</exception>
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                // This should never happen; guard against accidental misuse.
                throw new InvalidOperationException("Cannot convert a successful result to a problem details response.");
            }

            // Take the first error to determine the primary status code
            Domain.SeedWork.Error firstError = result.Errors.First();

            // Map internal error codes to HTTP status codes
            int statusCode = firstError.Code switch
            {
                var code when code.EndsWith(".NotFound") => StatusCodes.Status404NotFound,
                var code when code.Contains("Duplicate") => StatusCodes.Status409Conflict,
                var code when code.Contains("Unauthorized") => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status400BadRequest
            };

            // Create a ProblemDetails object and attach our custom error list via Extensions
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

        // Helper to generate a default RFC 7807 title for the given status code
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
}
