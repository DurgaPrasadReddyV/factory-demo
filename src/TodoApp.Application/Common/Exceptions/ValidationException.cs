using FluentValidation.Results;

namespace TodoApp.Application.Common.Exceptions;

/// <summary>
/// The application's own validation failure type. Kept separate from
/// FluentValidation's so the web layer maps one thing to a 400 and does not have to
/// know which validation library a slice happens to use.
/// </summary>
public sealed class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Failures = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Failures = failures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());
    }

    public IDictionary<string, string[]> Failures { get; }
}
