using FluentValidation;
using MediatR;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Common.Models;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.Todos.CreateTodo;

/// <summary>Slice: add a todo. Request, handler and validator live together.</summary>
public sealed record CreateTodoCommand(string Title) : IRequest<TodoDto>;

public sealed class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, TodoDto>
{
    private readonly ITodoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTodoCommandHandler(ITodoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TodoDto> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var item = TodoItem.Create(request.Title);

        await _repository.AddAsync(item, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return item.ToDto();
    }
}

public sealed class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A title is required.")
            .MaximumLength(200).WithMessage("A title cannot be longer than 200 characters.");
    }
}
