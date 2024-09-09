using ERP.Server.Application;
using ERP.Server.Infrastructure;
using ERP.Server.WebAPI.BackgroundServices;
using ERP.Server.WebAPI.Middlewares;
using ERP.Server.WebAPI.Utilities;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();


builder.Services.AddApplication();
builder.Services.AddInfrasturcture(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<OutboxBackgroundService>();

builder.Services.AddServiceTool();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "mylogs",
        MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
    })
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.MapControllers();

app.Run();
