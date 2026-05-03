using CreditRiskEngine.Application.DependencyInjection;
using CreditRiskEngine.Infrastructure.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rulesFilePath = Path.Combine(builder.Environment.ContentRootPath, "rules.json");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(rulesFilePath);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program
{
}