using BooksRUs.Host.Controllers;
using BooksRUs.Host.Middleware;
using BooksRUs.Host.Seed;
using BooksRUs.Modules.Catalog.Infrastructure;
using BooksRUs.Modules.ReadingList.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel();

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration)
        .Enrich.WithExceptionDetails()
        .Enrich.FromLogContext();
});

builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddReadingListModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BooksRUs API", Version = "v1" })
);

const string SpaCors = "SpaCors";

builder.Services.AddCors(o => o.AddPolicy(SpaCors, p =>
    p.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
     .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

await app.Services.MigrateAndSeedAsync(app.Logger);

app.UseRouting();
app.UseCors(SpaCors);

app.UseErrorHandling();

app.UseSerilogRequestLogging(opts =>
{
    opts.GetLevel = (ctx, _, ex) =>
        ex != null || ctx.Response.StatusCode >= 500 ?
        Serilog.Events.LogEventLevel.Error :
        Serilog.Events.LogEventLevel.Information;

    opts.EnrichDiagnosticContext = (diag, ctx) =>
    {
        diag.Set("RequestPath", ctx.Request.Path);
        diag.Set("RequestMethod", ctx.Request.Method);
        diag.Set("UserAgent", ctx.Request.Headers.UserAgent.ToString());
        diag.Set("TraceId", ctx.TraceIdentifier);
    };
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapBooksEndpoints();
app.MapReadingListEndpoints();

app.Run();

public partial class Program { }