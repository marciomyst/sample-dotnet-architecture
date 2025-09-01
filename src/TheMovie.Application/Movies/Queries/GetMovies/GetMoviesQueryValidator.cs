using FluentValidation;
using Microsoft.Extensions.Localization;
using TheMovie.Application.Resources;

namespace TheMovie.Application.Movies.Queries.GetMovies;

/// <summary>
/// Validates <see cref="GetMoviesQuery"/> inputs such as pagination and optional filters.
/// </summary>
/// <remarks>
/// <para>
/// Enforces simple invariants before dispatching the query:
/// - PageNumber &gt;= 1
/// - 1 &lt;= PageSize &lt;= 100
/// - Title (when provided) has a maximum length of 200
/// - ReleaseYear (when provided) is between 1889 and current year
/// - GenreId (when provided) is not Guid.Empty
/// </para>
/// </remarks>
public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
{
    private const int MaxPageSize = 100;

    /// <summary>
    /// Initializes validation rules for the GetMovies query using localized messages.
    /// </summary>
    public GetMoviesQueryValidator(IStringLocalizer<Validations> localizer)
    {
        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage(_ => localizer[nameof(Validations.Pagination_PageNumber_Min)]);

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, MaxPageSize)
            .WithMessage(_ => localizer[nameof(Validations.Pagination_PageSize_Range), 1, MaxPageSize]);

        RuleFor(q => q.Title)
            .MaximumLength(200)
            .WithMessage(_ => localizer[nameof(Validations.Movie_Title_MaxLength), 200])
            .When(q => !string.IsNullOrWhiteSpace(q.Title));

        RuleFor(q => q.ReleaseYear)
            .GreaterThan(1888)
            .WithMessage(_ => localizer[nameof(Validations.Movie_ReleaseYear_Min)]) 
            .LessThanOrEqualTo(_ => DateTime.UtcNow.Year)
            .WithMessage(_ => localizer[nameof(Validations.Movie_ReleaseYear_Future)]) 
            .When(q => q.ReleaseYear.HasValue);

        RuleFor(q => q.GenreId)
            .Must(id => id.HasValue ? id.Value != Guid.Empty : true)
            .WithMessage(_ => localizer[nameof(Validations.Movie_GenreId_Required)]);
    }
}
