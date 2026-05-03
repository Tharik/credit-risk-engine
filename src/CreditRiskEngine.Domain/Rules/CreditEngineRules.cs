namespace CreditRiskEngine.Domain.Rules;

public class CreditEngineRules
{
    public List<ClusterRule> Clusters { get; set; } = new();

    public List<JobCategoryRule> JobCategories { get; set; } = new();

    public List<PenaltyRule> Penalties { get; set; } = new();
}