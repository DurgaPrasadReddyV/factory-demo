using MediatR;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Common.Models;

namespace TodoApp.Application.Features.Todos.GetTodos;

/// <summary>Slice: list every todo, newest first.</summary>
public sealed record GetTodosQuery : IRequest<IReadOnlyList<TodoDto>>;

public sealed class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IReadOnlyList<TodoDto>>
{
    private readonly ITodoRepository _repository;

    public GetTodosQueryHandler(ITodoRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyList<TodoDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return items.Select(item => item.ToDto()).ToArray();
    }
}
