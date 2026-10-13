namespace TodoApp.Application.Common.Models;

/// <summary>The shape the outside world sees; the entity itself never leaves the application.</summary>
public sealed record TodoDto(
    Guid Id,
    string Title,
    bool IsCompleted,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt);
