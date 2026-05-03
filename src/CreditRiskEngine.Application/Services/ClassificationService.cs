using CreditRiskEngine.Application.Interfaces;
using CreditRiskEngine.Domain.Entities;
using CreditRiskEngine.Domain.Enums;
using CreditRiskEngine.Domain.Interfaces;
using CreditRiskEngine.Domain.Models;
using CreditRiskEngine.Domain.Rules;

namespace CreditRiskEngine.Application.Services;

public class ClassificationService : IClassificationService
{
    private static readonly string[] DefaultDebtTypes = new[] { "credit_default", "loan_default" };

    private readonly IRuleProvider _ruleProvider;

    public ClassificationService(IRuleProvider ruleProvider)
    {
        _ruleProvider = ruleProvider;
    }

    public ClassificationResult Classify(Customer customer)
    {
        var rules = _ruleProvider.GetRules();

        var clusterRule = ResolveCluster(customer, rules.Clusters);
        var jobRule = ResolveJobCategory(customer.JobTitle, rules.JobCategories);
        var penaltyFactor = ResolvePenaltyFactor(customer, rules.Penalties);
        var monthlyIncome = ResolveMonthlyIncome(clusterRule.Cluster, jobRule.Category);

        var approvedLimit = CalculateApprovedLimit(
            clusterRule.BaseLimit,
            jobRule.Multiplier,
            penaltyFactor,
            clusterRule.Cap);

        var approved = clusterRule.Cluster != ClusterType.ClusterD;

        if (!approved)
        {
            approvedLimit = 0;
        }

        return new ClassificationResult
        {
            Cluster = clusterRule.Cluster,
            ClusterName = clusterRule.Name,
            JobCategory = jobRule.Category,
            JobMultiplier = jobRule.Multiplier,
            MonthlyIncome = monthlyIncome,
            BaseLimit = clusterRule.BaseLimit,
            PenaltyFactor = penaltyFactor,
            ApprovedLimit = approvedLimit,
            ClusterCap = clusterRule.Cap,
            Approved = approved
        };
    }

    private static ClusterRule ResolveCluster(Customer customer, List<ClusterRule> clusterRules)
    {
        return clusterRules
            .OrderBy(rule => rule.Priority)
            .First(rule => IsClusterMatch(customer, rule));
    }

    private static bool IsClusterMatch(Customer customer, ClusterRule rule)
    {
        if (rule.MinScore.HasValue && customer.Score < rule.MinScore.Value)
        {
            return false;
        }

        if (rule.MinAge.HasValue && customer.Age < rule.MinAge.Value)
        {
            return false;
        }

        if (rule.MaxAge.HasValue && customer.Age > rule.MaxAge.Value)
        {
            return false;
        }

        if (rule.RequiresNoMarketDebt && customer.HasMarketDebt)
        {
            return false;
        }

        if (rule.DenyIfDefaultDebtExists && HasDefaultDebt(customer))
        {
            return false;
        }

        return true;
    }

    private static JobCategoryRule ResolveJobCategory(string jobTitle, List<JobCategoryRule> jobRules)
    {
        return jobRules
            .OrderBy(rule => rule.Priority)
            .First(rule =>
                rule.Keywords.Count == 0 ||
                rule.Keywords.Any(keyword =>
                    jobTitle.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
    }

    private static decimal ResolvePenaltyFactor(Customer customer, List<PenaltyRule> penaltyRules)
    {
        var matchedPenalty = penaltyRules
            .OrderBy(rule => rule.Priority)
            .FirstOrDefault(rule =>
                rule.TriggerDebtTypes.Any(debtType =>
                    customer.MarketDebtTypes.Contains(debtType, StringComparer.OrdinalIgnoreCase)));

        return matchedPenalty?.PenaltyFactor ?? 1.0m;
    }

    private static bool HasDefaultDebt(Customer customer)
    {
        return customer.MarketDebtTypes.Any(debtType =>
            DefaultDebtTypes.Contains(debtType, StringComparer.OrdinalIgnoreCase));
    }

    private static decimal CalculateApprovedLimit(
        decimal baseLimit,
        decimal jobMultiplier,
        decimal penaltyFactor,
        decimal cap)
    {
        var calculatedLimit = baseLimit * jobMultiplier * penaltyFactor;
        var cappedLimit = Math.Min(calculatedLimit, cap);

        return RoundToNearestHundred(cappedLimit);
    }

    private static decimal RoundToNearestHundred(decimal value)
    {
        return Math.Round(value / 100m, MidpointRounding.AwayFromZero) * 100m;
    }

    private static decimal ResolveMonthlyIncome(ClusterType cluster, JobCategoryType jobCategory)
    {
        return (cluster, jobCategory) switch
        {
            (ClusterType.ClusterA, JobCategoryType.Executive) => 30000,
            (ClusterType.ClusterA, JobCategoryType.SeniorProfessional) => 20000,
            (ClusterType.ClusterA, JobCategoryType.MidProfessional) => 12000,
            (ClusterType.ClusterA, JobCategoryType.JuniorProfessional) => 8000,
            (ClusterType.ClusterA, JobCategoryType.Other) => 10000,

            (ClusterType.ClusterB, JobCategoryType.Executive) => 20000,
            (ClusterType.ClusterB, JobCategoryType.SeniorProfessional) => 15000,
            (ClusterType.ClusterB, JobCategoryType.MidProfessional) => 8000,
            (ClusterType.ClusterB, JobCategoryType.JuniorProfessional) => 5000,
            (ClusterType.ClusterB, JobCategoryType.Other) => 6500,

            (ClusterType.ClusterC, JobCategoryType.Executive) => 10000,
            (ClusterType.ClusterC, JobCategoryType.SeniorProfessional) => 7000,
            (ClusterType.ClusterC, JobCategoryType.MidProfessional) => 5000,
            (ClusterType.ClusterC, JobCategoryType.JuniorProfessional) => 3000,
            (ClusterType.ClusterC, JobCategoryType.Other) => 4000,

            _ => 0
        };
    }
}