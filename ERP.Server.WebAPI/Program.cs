using ERP.Server.Application;
using ERP.Server.Infrastructure;
using ERP.Server.WebAPI.BackgroundServices;
using ERP.Server.WebAPI.Middlewares;
using ERP.Server.WebAPI.Utilities;

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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.MapControllers();

app.Run();
