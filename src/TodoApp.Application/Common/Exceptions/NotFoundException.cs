namespace TodoApp.Application.Common.Exceptions;

/// <summary>Raised by a slice when the thing it was asked about does not exist.</summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"{name} ({key}) was not found.")
    {
    }
}
