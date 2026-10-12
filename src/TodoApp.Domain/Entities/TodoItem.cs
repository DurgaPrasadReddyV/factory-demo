namespace TodoApp.Domain.Entities;

/// <summary>
/// The single aggregate in the sample. It owns its own state transitions so that
/// callers cannot leave it in an invalid shape.
/// </summary>
public sealed class TodoItem
{
    // EF Core materialises through this constructor.
    private TodoItem()
    {
    }

    private TodoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsCompleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public bool IsCompleted { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public static TodoItem Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A todo needs a title.", nameof(title));
        }

        return new TodoItem(title.Trim());
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A todo needs a title.", nameof(title));
        }

        Title = title.Trim();
    }

    public void Toggle()
    {
        IsCompleted = !IsCompleted;
        CompletedAt = IsCompleted ? DateTimeOffset.UtcNow : null;
    }
}
