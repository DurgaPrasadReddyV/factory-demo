using MediatR;
using TodoApp.Application.Features.Todos.CreateTodo;
using TodoApp.Application.Features.Todos.DeleteTodo;
using TodoApp.Application.Features.Todos.GetTodos;
using TodoApp.Application.Features.Todos.ToggleTodo;

namespace TodoApp.Web.Endpoints;

/// <summary>
/// The HTTP edge. Each route only forwards to the slice that owns the behaviour; no
/// business logic lives here.
/// </summary>
public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/todos").WithTags("Todos");

        group.MapGet("/", async (ISender sender, CancellationToken cancellationToken)
            => Results.Ok(await sender.Send(new GetTodosQuery(), cancellationToken)));

        group.MapPost("/", async (CreateTodoRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var todo = await sender.Send(new CreateTodoCommand(request.Title), cancellationToken);
            return Results.Created($"/api/todos/{todo.Id}", todo);
        });

        group.MapPost("/{id:guid}/toggle", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new ToggleTodoCommand(id), cancellationToken);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteTodoCommand(id), cancellationToken);
            return Results.NoContent();
        });

        return endpoints;
    }

    public sealed record CreateTodoRequest(string Title);
}
