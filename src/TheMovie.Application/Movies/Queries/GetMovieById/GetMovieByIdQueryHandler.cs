using Dapper;
using MediatR;
using System.Data;

namespace TheMovie.Application.Movies.Queries.GetMovieById;

/// <summary>
/// Handler for <see cref="GetMovieByIdQuery"/>.
/// </summary>
public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDetailDto?>
{
    private readonly IDbConnection _dbConnection;

    /// <summary>
    /// Initializes a new instance of <see cref="GetMovieByIdQueryHandler"/> with the specified database connection.
    /// </summary>
    /// <param name="dbConnection">IDbConnection configured for read operations.</param>
    public GetMovieByIdQueryHandler(IDbConnection dbConnection)
    {
        // Esta IDbConnection foi configurada no Program.cs
        // para apontar para o banco de dados de LEITURA.
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// Handles the <see cref="GetMovieByIdQuery"/> to retrieve movie details by ID.
    /// </summary>
    /// <param name="request">The query containing the movie ID.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="MovieDetailDto"/> with movie details or null if not found.</returns>
    public async Task<MovieDetailDto?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
                SELECT
                    m.""Id"",
                    m.""Title"",
                    m.""Synopsis"",
                    m.""ReleaseYear"",
                    m.""Price"",
                    g.""Name"" AS GenreName,
                    m.""Rating""
                FROM ""Movies"" m
                INNER JOIN ""Genres"" g ON m.""GenreId"" = g.""Id""
                WHERE m.""Id"" = @MovieId";

        MovieDetailDto? movie = await _dbConnection.QueryFirstOrDefaultAsync<MovieDetailDto>(
            sql,
            new { MovieId = request.Id });

        return movie;
    }
}
