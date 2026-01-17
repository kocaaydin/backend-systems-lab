var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => 
{
    // Return a unique GUID as requested
    return Results.Ok(Guid.NewGuid().ToString());
});

app.Run();
