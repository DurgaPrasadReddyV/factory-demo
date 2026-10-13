using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

public sealed class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
        => _context = context;

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Todos
            .OrderByDescending(todo => todo.CreatedAt)
            .ToListAsync(cancellationToken);

    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Todos.FirstOrDefaultAsync(todo => todo.Id == id, cancellationToken);

    public async Task AddAsync(TodoItem item, CancellationToken cancellationToken = default)
        => await _context.Todos.AddAsync(item, cancellationToken);

    public void Remove(TodoItem item)
        => _context.Todos.Remove(item);
}
