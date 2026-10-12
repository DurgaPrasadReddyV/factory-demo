using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Models;
using TodoApp.Application.Features.Todos.CreateTodo;
using TodoApp.Application.Features.Todos.DeleteTodo;
using TodoApp.Application.Features.Todos.GetTodos;
using TodoApp.Application.Features.Todos.ToggleTodo;

namespace TodoApp.Web.Pages;

public sealed class IndexModel : PageModel
{
    private readonly ISender _sender;

    public IndexModel(ISender sender)
        => _sender = sender;

    public IReadOnlyList<TodoDto> Todos { get; private set; } = Array.Empty<TodoDto>();

    [BindProperty]
    public string NewTitle { get; set; } = string.Empty;

    public async Task OnGetAsync(CancellationToken cancellationToken)
        => Todos = await _sender.Send(new GetTodosQuery(), cancellationToken);

    public async Task<IActionResult> OnPostCreateAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(new CreateTodoCommand(NewTitle), cancellationToken);
        }
        catch (ValidationException exception)
        {
            // The slice's validator rejected the input; show it in the page rather than
            // letting the global handler turn it into a JSON problem response.
            foreach (var failure in exception.Failures)
            {
                foreach (var message in failure.Value)
                {
                    ModelState.AddModelError(failure.Key, message);
                }
            }

            Todos = await _sender.Send(new GetTodosQuery(), cancellationToken);
            return Page();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleAsync(Guid id, CancellationToken cancellationToken)
    {
        await _sender.Send(new ToggleTodoCommand(id), cancellationToken);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteTodoCommand(id), cancellationToken);
        return RedirectToPage();
    }
}
