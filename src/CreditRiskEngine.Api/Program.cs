using CreditRiskEngine.Api.Middleware;
using CreditRiskEngine.Application.DependencyInjection;
using CreditRiskEngine.Infrastructure.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var rulesFilePath = Path.Combine(builder.Environment.ContentRootPath, "rules.json");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(rulesFilePath);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

public partial class Program
{
}