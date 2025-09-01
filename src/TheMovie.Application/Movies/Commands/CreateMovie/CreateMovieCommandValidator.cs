using FluentValidation;
using Microsoft.Extensions.Localization;
using TheMovie.Application.Resources;
using TheMovie.Domain.Interfaces;

namespace TheMovie.Application.Movies.Commands.CreateMovie;

/// <summary>
/// Validator for the <see cref="CreateMovieCommand"/>.
/// Performs format validation and business rule checks with repository lookups.
/// All error messages are localized via <see cref="IStringLocalizer{T}"/>.
/// </summary>
/// <remarks>
/// <para>
/// Enforces basic invariants (required fields, max lengths, numeric ranges, valid enum values) and business rules
/// (unique movie title, existing genre) before the handler runs. Messages are sourced from
/// <see cref="IStringLocalizer{Validations}"/> so they can be translated per current UI culture.
/// </para>
/// <para>
/// Rules overview:
/// - Title: not empty; max length 200; unique (checked asynchronously).
/// - Synopsis: not empty.
/// - Price: greater than 0.
/// - Rating: valid enum value.
/// - ReleaseYear: not in the future.
/// - GenreId: not empty and must exist.
/// </para>
/// <para>
/// Example:
/// <code><![CDATA[
/// var validator = new CreateMovieCommandValidator(movieRepo, genreRepo, localizer);
/// var result = await validator.ValidateAsync(command);
/// if (!result.IsValid) { /* map to ProblemDetails with localized messages */ }
/// ]]></code>
/// </para>
/// </remarks>
public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    private readonly IMovieRepository movieRepository;
    private readonly IGenreRepository genreRepository;
    private readonly IStringLocalizer<Validations> localizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMovieCommandValidator"/> class.
    /// </summary>
    /// <param name="movieRepository">The movie repository for database access.</param>
    /// <param name="genreRepository">The genre repository for database access.</param>
    /// <param name="localizer">The string localizer for retrieving translated error messages.</param>
    public CreateMovieCommandValidator(
        IMovieRepository movieRepository,
        IGenreRepository genreRepository,
        IStringLocalizer<Validations> localizer)
    {
        this.movieRepository = movieRepository;
        this.genreRepository = genreRepository;
        this.localizer = localizer;
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(_ => localizer[nameof(Validations.Movie_Title_Required)])
            .MaximumLength(200)
            .WithMessage(_ => localizer[nameof(Validations.Movie_Title_MaxLength), 200]);

        RuleFor(x => x.Synopsis)
            .NotEmpty()
            .WithMessage(_ => localizer[nameof(Validations.Movie_Synopsis_Required)]);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage(_ => localizer[nameof(Validations.Movie_Price_GreaterThan)]);

        RuleFor(x => x.Rating)
            .IsInEnum()
            .WithMessage(_ => localizer[nameof(Validations.Movie_Rating_Invalid)]);


        RuleFor(x => x.ReleaseYear)
            .Must(year => year <= DateTime.UtcNow.Year)
            .WithMessage(_ => localizer[nameof(Validations.Movie_ReleaseYear_Future)]);

        RuleFor(x => x.Title)
            .MustAsync(BeAUniqueTitle)
            .WithMessage(_ => localizer[nameof(Validations.Movie_Title_Unique)]);

        RuleFor(x => x.GenreId)
            .NotEmpty()
            .WithMessage(_ => localizer[nameof(Validations.Movie_GenreId_Required)])
            .MustAsync(GenreMustExist)
            .WithMessage(_ => localizer[nameof(Validations.Movie_GenreId_Exists)]);
    }

    /// <summary>
    /// Checks asynchronously whether a movie title is unique in the data store.
    /// </summary>
    /// <param name="title">The movie title to verify.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns><c>true</c> if no existing movie uses the specified title; otherwise, <c>false</c>.</returns>
    private async Task<bool> BeAUniqueTitle(string title, CancellationToken cancellationToken)
    {
        Domain.Aggregates.MovieAggregate.Movie? movie = await movieRepository.GetByTitleAsync(title);
        return movie is null;
    }

    /// <summary>
    /// Checks asynchronously whether a genre with the specified identifier exists in the data store.
    /// </summary>
    /// <param name="genreId">The genre identifier to verify.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns><c>true</c> if the genre exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> GenreMustExist(Guid genreId, CancellationToken cancellationToken)
    {
        if (genreId == Guid.Empty)
        {
            return false;
        }

        Domain.Aggregates.GenreAggregate.Genre? genre = await genreRepository.GetByIdAsync(genreId);
        return genre is not null;
    }
}
