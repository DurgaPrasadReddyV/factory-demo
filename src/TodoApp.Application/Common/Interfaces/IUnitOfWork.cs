namespace TodoApp.Application.Common.Interfaces;

/// <summary>
/// Lets a feature slice commit its own changes without knowing which provider is behind it.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
