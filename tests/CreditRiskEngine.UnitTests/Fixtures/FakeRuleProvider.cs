using CreditRiskEngine.Domain.Enums;
using CreditRiskEngine.Domain.Interfaces;
using CreditRiskEngine.Domain.Rules;

namespace CreditRiskEngine.UnitTests.Fixtures;

public class FakeRuleProvider : IRuleProvider
{
    public CreditEngineRules GetRules()
    {
        return new CreditEngineRules
        {
            Clusters = new List<ClusterRule>
            {
                new()
                {
                    Priority = 1,
                    Cluster = ClusterType.ClusterA,
                    Name = "Diamond",
                    MinScore = 700,
                    MinAge = 25,
                    MaxAge = 60,
                    RequiresNoMarketDebt = true,
                    BaseLimit = 50000,
                    Cap = 100000
                },
                new()
                {
                    Priority = 2,
                    Cluster = ClusterType.ClusterB,
                    Name = "Gold",
                    MinScore = 500,
                    MinAge = 18,
                    MaxAge = 65,
                    DenyIfDefaultDebtExists = true,
                    BaseLimit = 20000,
                    Cap = 40000
                },
                new()
                {
                    Priority = 3,
                    Cluster = ClusterType.ClusterC,
                    Name = "Silver",
                    MinScore = 300,
                    BaseLimit = 5000,
                    Cap = 10000
                },
                new()
                {
                    Priority = 4,
                    Cluster = ClusterType.ClusterD,
                    Name = "Bronze",
                    BaseLimit = 0,
                    Cap = 0
                }
            },
            JobCategories = new List<JobCategoryRule>
            {
                new()
                {
                    Priority = 1,
                    Category = JobCategoryType.Executive,
                    Multiplier = 2.0m,
                    Keywords = new List<string> { "CEO", "CFO", "CTO", "COO", "CIO", "CMO", "Chief", "President", "Vice President", "VP", "Director" }
                },
                new()
                {
                    Priority = 2,
                    Category = JobCategoryType.SeniorProfessional,
                    Multiplier = 1.5m,
                    Keywords = new List<string> { "Senior", "Lead", "Manager", "Coordinator", "Supervisor", "Principal" }
                },
                new()
                {
                    Priority = 3,
                    Category = JobCategoryType.MidProfessional,
                    Multiplier = 1.0m,
                    Keywords = new List<string> { "Engineer", "Analyst", "Developer", "Specialist", "Designer", "Accountant", "Consultant", "Architect" }
                },
                new()
                {
                    Priority = 4,
                    Category = JobCategoryType.JuniorProfessional,
                    Multiplier = 0.7m,
                    Keywords = new List<string> { "Junior", "Trainee", "Intern", "Apprentice", "Assistant", "Associate" }
                },
                new()
                {
                    Priority = 5,
                    Category = JobCategoryType.Other,
                    Multiplier = 0.8m,
                    Keywords = new List<string>()
                }
            },
            Penalties = new List<PenaltyRule>
            {
                new()
                {
                    Priority = 1,
                    RuleId = "DEFAULT_DEBT_PENALTY",
                    TriggerDebtTypes = new List<string> { "credit_default", "loan_default" },
                    PenaltyFactor = 0.5m
                }
            }
        };
    }
}