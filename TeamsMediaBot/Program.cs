using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add basic services
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

// Simple health check endpoint for testing
app.MapGet("/health", () => Results.Ok(new { status = "Teams Media Bot Host is ready" }));

app.Run();