# factory-demo

Disposable scratch repo for the agent factory's first end-to-end proof (#37).

This repository is built by the agent factory.

## TodoApp — a .NET 10 clean-architecture + vertical-slice sample

The sample lives in `src/`. It is a small todo application with a server-rendered
UI and a JSON API, built to show how clean architecture and vertical slice
architecture fit together without fighting each other.

### Layout

| Project | Responsibility |
| --- | --- |
| `TodoApp.Domain` | The `TodoItem` aggregate. No dependencies. |
| `TodoApp.Application` | Use cases, organised as **vertical slices** under `Features/Todos/<Slice>`. Each slice holds its request, handler and validator together. Also owns the cross-cutting MediatR behaviors. |
| `TodoApp.Infrastructure` | EF Core (`AppDbContext`, in-memory provider), the repository and unit-of-work implementations. |
| `TodoApp.Web` | Composition root, minimal-API endpoints, Razor Pages UI, global exception handling. |

Clean architecture supplies the dependency direction (`Web -> Infrastructure ->
Application -> Domain`) and the layer boundaries. Vertical slices supply the shape
*inside* the application layer: one folder per use case instead of one folder per
technical concern. A slice may only reach the other layers through the interfaces in
`Application/Common/Interfaces`.

### Cross-cutting concerns

- **Validation** — FluentValidation validators run in a MediatR pipeline behavior
  before every handler; failures surface as a 400 `ProblemDetails`.
- **Logging** — a MediatR pipeline behavior logs each request and its duration.
- **Error handling** — one `IExceptionHandler` maps application exceptions to
  `ProblemDetails` (400 validation, 404 missing, 500 otherwise).
- **Composition** — each layer exposes a single `Add...` extension method.

### Run it

```bash
dotnet run --project src/TodoApp.Web
```

Then open the printed URL (for example <http://localhost:5073>). The app uses the
EF Core in-memory provider, so the three seeded todos are recreated on every launch.

### API

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/todos` | List todos |
| `POST` | `/api/todos` | Create a todo (`{ "title": "..." }`) |
| `POST` | `/api/todos/{id}/toggle` | Toggle a todo's completed state |
| `DELETE` | `/api/todos/{id}` | Delete a todo |
| `GET` | `/health` | Health probe |

OpenAPI is mapped in Development at `/openapi/v1.json`.
