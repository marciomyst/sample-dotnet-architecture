using TheMovie.Domain.SeedWork;

namespace TheMovie.Application.Shared;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail without returning a value.
/// </summary>
/// <remarks>
/// <para>
/// Use this type to model success/failure flows without exceptions in expected control paths.
/// For value-returning operations, use <see cref="Result{T}"/>.
/// </para>
/// <para>
/// Example:
/// <code>
/// Result result = Validate(input);
/// if (result.IsFailure)
/// {
///     // Inspect result.Errors for details (codes/messages)
///     return result;
/// }
/// // Continue on success
/// </code>
/// </para>
/// </remarks>
public class Result
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// The list of domain <see cref="Error"/> instances describing the failure; empty on success.
    /// </summary>
    public IReadOnlyCollection<Error> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="errors">Associated errors; should be empty on success.</param>
    protected Result(bool isSuccess, IReadOnlyCollection<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    /// <summary>
    /// Creates a failed result with a single <paramref name="error"/>.
    /// </summary>
    /// <param name="error">The error describing the failure.</param>
    public static Result Fail(Error error) => new(false, new[] { error });

    /// <summary>
    /// Creates a failed result with a collection of <paramref name="errors"/>.
    /// </summary>
    /// <param name="errors">The errors describing the failure.</param>
    public static Result Fail(IReadOnlyCollection<Error> errors) => new(false, errors);

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result Ok() => new(true, Array.Empty<Error>());
}

/// <summary>
/// Represents the outcome of an operation that may succeed and return a value of type <typeparamref name="T"/>,
/// or fail and return associated errors.
/// </summary>
/// <typeparam name="T">The value type returned on success.</typeparam>
/// <remarks>
/// <para>
/// Use <see cref="Ok(T)"/> to produce a successful result with a value and <see cref="Fail(Error)"/> or
/// <see cref="Fail(IReadOnlyCollection{Error})"/> to produce a failed result.
/// </para>
/// <para>
/// Example:
/// <code>
/// Result<Movie> result = await service.GetMovieAsync(id);
/// if (result.IsSuccess)
/// {
///     var movie = result.Value;
/// }
/// else
/// {
///     // Inspect result.Errors
/// }
/// </code>
/// </para>
/// </remarks>
public class Result<T> : Result
{
    /// <summary>
    /// The value returned on success; <c>null</c> when the result represents a failure.
    /// </summary>
    public T? Value { get; }

    private Result(T value) : base(true, Array.Empty<Error>())
    {
        Value = value;
    }

    private Result(IReadOnlyCollection<Error> errors) : base(false, errors)
    {
        Value = default;
    }

    /// <summary>
    /// Creates a successful result with the provided <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to carry.</param>
    public static Result<T> Ok(T value) => new(value);

    /// <summary>
    /// Creates a failed result with a single <paramref name="error"/>.
    /// </summary>
    /// <param name="error">The error describing the failure.</param>
    public static new Result<T> Fail(Error error) => new(new[] { error });

    /// <summary>
    /// Creates a failed result with a collection of <paramref name="errors"/>.
    /// </summary>
    /// <param name="errors">The errors describing the failure.</param>
    public static new Result<T> Fail(IReadOnlyCollection<Error> errors) => new(errors);
}
