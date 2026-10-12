using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.Todos.ToggleTodo;

/// <summary>Slice: flip a todo between done and not done.</summary>
public sealed record ToggleTodoCommand(Guid Id) : IRequest<Unit>;

public sealed class ToggleTodoCommandHandler : IRequestHandler<ToggleTodoCommand, Unit>
{
    private readonly ITodoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleTodoCommandHandler(ITodoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ToggleTodoCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), request.Id);

        item.Toggle();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
