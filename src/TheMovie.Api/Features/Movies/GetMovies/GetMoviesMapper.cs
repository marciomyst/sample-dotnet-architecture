using TheMovie.Application.Movies.Queries.GetMovies;
using TheMovie.Application.Shared;

namespace TheMovie.Api.Features.Movies.GetMovies;

/// <summary>
/// Maps between HTTP DTOs and application queries/responses for the GetMovies feature.
/// </summary>
internal static class GetMoviesMapper
{
    /// <summary>
    /// Converts a <see cref="GetMoviesRequest"/> to a <see cref="GetMoviesQuery"/>.
    /// </summary>
    public static GetMoviesQuery ToQuery(this GetMoviesRequest request)
        => new(request.Title, request.ReleaseYear, request.GenreId, request.PageNumber, request.PageSize);

    /// <summary>
    /// Converts an application-layer paged result to the HTTP-layer response DTO.
    /// </summary>
    public static GetMoviesResponse ToResponse(this PagedResult<MovieDto> page)
    {
        var items = page.Items
            .Select(i => new GetMoviesItem(i.Id, i.Title, i.ReleaseYear, i.GenreName))
            .ToList();

        return new GetMoviesResponse(items, page.PageNumber, page.PageSize, page.TotalCount);
    }
}

