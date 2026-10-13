using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

/// <summary>
/// The in-memory store starts empty on every launch, so a little sample data makes the
/// running app immediately useful.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Todos.AnyAsync(cancellationToken))
        {
            return;
        }

        context.Todos.AddRange(
            TodoItem.Create("Launch the .NET 10 todo app"),
            TodoItem.Create("Browse the vertical slices under Application/Features/Todos"),
            TodoItem.Create("Swap the in-memory provider for a real database"));

        await context.SaveChangesAsync(cancellationToken);
    }
}
