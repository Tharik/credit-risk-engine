using CreditRiskEngine.Domain.Interfaces;
using CreditRiskEngine.Infrastructure.RuleProviders;
using Microsoft.Extensions.DependencyInjection;

namespace CreditRiskEngine.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string rulesFilePath)
    {
        services.AddSingleton<IRuleProvider>(_ => new JsonRuleProvider(rulesFilePath));

        return services;
    }
}