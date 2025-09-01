using FluentValidation;
using Microsoft.Extensions.Localization;
using TheMovie.Application.Resources;
using TheMovie.Domain.Interfaces;

namespace TheMovie.Application.Movies.Commands.UpdateMovie;

/// <summary>
/// Validator for <see cref="UpdateMovieCommand"/>.
/// Performs format validation and business rules (unique title, existing genre) with localized messages.
/// </summary>
public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    private readonly IMovieRepository movieRepository;
    private readonly IGenreRepository genreRepository;
    private readonly IStringLocalizer<Validations> localizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMovieCommandValidator"/> class and defines rules.
    /// </summary>
    /// <param name="movieRepository">Repository for movie lookups.</param>
    /// <param name="genreRepository">Repository for genre lookups.</param>
    /// <param name="localizer">String localizer for validation messages.</param>
    public UpdateMovieCommandValidator(
        IMovieRepository movieRepository,
        IGenreRepository genreRepository,
        IStringLocalizer<Validations> localizer)
    {
        this.movieRepository = movieRepository;
        this.genreRepository = genreRepository;
        this.localizer = localizer;

        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage(_ => localizer["Movie_Title_Required"])
            .MaximumLength(200)
            .WithMessage(_ => localizer["Movie_Title_MaxLength", 200]);

        RuleFor(c => c.Synopsis)
            .NotEmpty()
            .WithMessage(_ => localizer["Movie_Synopsis_Required"]);

        RuleFor(c => c.Price)
            .GreaterThan(0)
            .WithMessage(_ => localizer["Movie_Price_GreaterThan"]);

        RuleFor(c => c.Rating)
            .IsInEnum()
            .WithMessage(_ => localizer["Movie_Rating_Invalid"]);

        RuleFor(c => c.ReleaseYear)
            .Must(year => year <= DateTime.UtcNow.Year)
            .WithMessage(_ => localizer["Movie_ReleaseYear_Future"]);

        RuleFor(c => c)
            .MustAsync(HaveAUniqueTitle)
            .WithMessage(_ => localizer["Movie_Title_Unique"]);

        RuleFor(c => c.GenreId)
            .NotEmpty().WithMessage(_ => localizer["Movie_GenreId_Required"])
            .MustAsync(GenreMustExist)
            .WithMessage(_ => localizer["Movie_GenreId_Exists"]);
    }

    private async Task<bool> HaveAUniqueTitle(UpdateMovieCommand command, CancellationToken cancellationToken)
    {
        var existing = await movieRepository.GetByTitleAsync(command.Title);
        return existing is null || existing.Id == command.Id;
    }

    private async Task<bool> GenreMustExist(Guid genreId, CancellationToken cancellationToken)
    {
        if (genreId == Guid.Empty)
        {
            return false;
        }

        var genre = await genreRepository.GetByIdAsync(genreId);
        return genre is not null;
    }
}
