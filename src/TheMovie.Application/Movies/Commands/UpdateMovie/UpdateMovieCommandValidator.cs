using FluentValidation;

namespace TheMovie.Application.Movies.Commands.UpdateMovie;

/// <summary>
/// Validates instances of <see cref="UpdateMovieCommand"/> using FluentValidation.
/// </summary>
public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMovieCommandValidator"/> class
    /// and defines validation rules for updating a movie.
    /// </summary>
    public UpdateMovieCommandValidator()
    {
        /// <summary>
        /// Ensures the Id property is not empty.
        /// </summary>
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id is required.");

        /// <summary>
        /// Ensures the Title property is provided and does not exceed 200 characters.
        /// </summary>
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        /// <summary>
        /// Ensures the Synopsis property is provided and does not exceed 1000 characters.
        /// </summary>
        RuleFor(c => c.Synopsis)
            .NotEmpty().WithMessage("Synopsis is required.")
            .MaximumLength(1000).WithMessage("Synopsis cannot exceed 1000 characters.");

        /// <summary>
        /// Ensures the ReleaseYear property is greater than 1888.
        /// </summary>
        RuleFor(c => c.ReleaseYear)
            .GreaterThan(1888).WithMessage("Release year must be greater than 1888.");

        /// <summary>
        /// Ensures the Price property is greater than zero.
        /// </summary>
        RuleFor(c => c.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        /// <summary>
        /// Ensures the GenreId property is not empty.
        /// </summary>
        RuleFor(c => c.GenreId)
            .NotEmpty().WithMessage("Genre is required.");
    }
}
