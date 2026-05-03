using CreditRiskEngine.Application.Interfaces;
using CreditRiskEngine.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CreditRiskEngine.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClassificationService, ClassificationService>();

        return services;
    }
}