using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces;

/// <summary>
/// Persistence is expressed as an abstraction here so the application layer never
/// references EF Core. Infrastructure supplies the implementation.
/// </summary>
public interface ITodoRepository
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(TodoItem item, CancellationToken cancellationToken = default);

    void Remove(TodoItem item);
}
