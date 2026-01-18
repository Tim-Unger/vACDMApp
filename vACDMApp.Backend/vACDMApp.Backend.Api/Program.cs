using vACDMApp.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddJsonSerializer();

//Cache + EF Core
builder.Services.AddStorageServices();

//SignalR
builder.Services.AddSignalR();

//Logging and Caching
builder.Services.AddLogging();
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();

// Configure Serilog
builder.Host.ConfigureLogger();

var app = builder.Build();

app.MapControllers();

app.MapOpenApi("/openapi");

app.UseSwagger();
app.UseSwaggerUI();

//TODO
app.Use(async (context, next) =>
{
    //Redirect / to swagger
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }

    //Complete Preflight requests (WSS)
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.StatusCode = 204;
        await context.Response.CompleteAsync();
        return;
    }

    await next.Invoke();
});

app.UseResponseCaching();

//Log Response Times
app.UseResponseTimeLogging();

app.Run();