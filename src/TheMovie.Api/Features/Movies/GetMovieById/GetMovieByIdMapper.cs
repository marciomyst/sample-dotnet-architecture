using TheMovie.Application.Movies.Queries.GetMovieById;

namespace TheMovie.Api.Features.Movies.GetMovieById;

/// <summary>
/// Maps between HTTP DTOs and application queries/responses for GetMovieById.
/// </summary>
internal static class GetMovieByIdMapper
{
    /// <summary>
    /// Converts a <see cref="GetMovieByIdRequest"/> to a <see cref="GetMovieByIdQuery"/>.
    /// </summary>
    public static GetMovieByIdQuery ToQuery(this GetMovieByIdRequest request) => new(request.Id);

    /// <summary>
    /// Maps an application <see cref="MovieDetailDto"/> to the HTTP response DTO.
    /// </summary>
    public static GetMovieByIdResponse ToResponse(this MovieDetailDto dto)
        => new(
            Id: dto.Id,
            Title: dto.Title,
            Synopsis: dto.Synopsis,
            ReleaseYear: dto.ReleaseYear,
            Price: dto.TicketPrice,
            GenreName: dto.GenreName,
            Rating: dto.Rating
        );
}

