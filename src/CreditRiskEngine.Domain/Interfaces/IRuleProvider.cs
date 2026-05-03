using CreditRiskEngine.Domain.Rules;

namespace CreditRiskEngine.Domain.Interfaces;

public interface IRuleProvider
{
    CreditEngineRules GetRules();
}