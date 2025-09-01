using FluentValidation;
using Microsoft.Extensions.Localization;
using TheMovie.Application.Resources;

namespace TheMovie.Application.Movies.Commands.DeleteMovie;

/// <summary>
/// Validator for <see cref="DeleteMovieCommand"/> ensuring a valid identifier is supplied.
/// </summary>
/// <remarks>
/// <para>
/// Keeps the handler focused on business logic by pushing basic input validation into the pipeline.
/// </para>
/// </remarks>
public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    /// <summary>
    /// Initializes validation rules for deleting a movie.
    /// </summary>
    public DeleteMovieCommandValidator(IStringLocalizer<Validations> localizer)
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage(_ => localizer[nameof(Validations.Movie_Id_Required)]);
    }
}
