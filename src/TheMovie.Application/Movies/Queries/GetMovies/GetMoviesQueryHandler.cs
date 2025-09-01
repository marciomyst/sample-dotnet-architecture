using Dapper;
using MediatR;
using System.Data;
using System.Text;
using TheMovie.Application.Shared;

namespace TheMovie.Application.Movies.Queries.GetMovies;

/// <summary>
/// Handler for <see cref="GetMoviesQuery"/> that retrieves movies from the database with pagination and filtering.
/// </summary>
/// <remarks>
/// Relies on <see cref="GetMoviesQueryValidator"/> to validate pagination bounds and optional filters.
/// </remarks>
/// <remarks>
/// Initializes a new instance of <see cref="GetMoviesQueryHandler"/> with the specified database connection.
/// </remarks>
/// <param name="dbConnection">IDbConnection configured for database operations.</param>
public class GetMoviesQueryHandler(IDbConnection dbConnection) : IRequestHandler<GetMoviesQuery, PagedResult<MovieDto>>
{
    private readonly IDbConnection _dbConnection = dbConnection;

    /// <summary>
    /// Handles the <see cref="GetMoviesQuery"/> to retrieve a paginated list of movies applying optional filters.
    /// </summary>
    /// <param name="request">The query containing filter and pagination parameters.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="PagedResult{MovieDto}"/> containing the list of movies, current page, page size, and total count.
    /// </returns>
    public async Task<PagedResult<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        var whereClause = new StringBuilder(" WHERE 1=1 ");

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            whereClause.Append(@" AND m.""Title"" ILIKE @Title ");
            parameters.Add("Title", $"%{request.Title}%");
        }
        if (request.ReleaseYear.HasValue)
        {
            whereClause.Append(@" AND m.""ReleaseYear"" = @ReleaseYear ");
            parameters.Add("ReleaseYear", request.ReleaseYear.Value);
        }
        if (request.GenreId.HasValue)
        {
            whereClause.Append(@" AND m.""GenreId"" = @GenreId ");
            parameters.Add("GenreId", request.GenreId.Value);
        }

        string countSql = $@"SELECT COUNT(m.""Id"") FROM ""Movies"" m {whereClause}";
        int totalCount = await _dbConnection.ExecuteScalarAsync<int>(countSql, parameters);

        string selectSql = $@"
                SELECT m.""Id"", m.""Title"", m.""ReleaseYear"", g.""Name"" AS GenreName
                FROM ""Movies"" m
                INNER JOIN ""Genres"" g ON m.""GenreId"" = g.""Id""
                {whereClause}
                ORDER BY m.""Title""
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
        parameters.Add("PageSize", request.PageSize);

        var movies = (await _dbConnection.QueryAsync<MovieDto>(selectSql, parameters)).ToList();

        return new PagedResult<MovieDto>(movies, request.PageNumber, request.PageSize, totalCount);
    }
}
