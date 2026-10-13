using TodoApp.Application;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Web.Endpoints;
using TodoApp.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Composition root: each layer registers its own services.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Cross-cutting error handling first, so every later failure becomes a ProblemDetails.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();

app.MapRazorPages();
app.MapTodoEndpoints();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

// The in-memory database is recreated per process, so seed it on startup.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

app.Run();

// Exposed so a test host can boot the app with WebApplicationFactory.
public partial class Program;
