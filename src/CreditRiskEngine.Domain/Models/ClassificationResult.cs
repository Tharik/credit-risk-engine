using CreditRiskEngine.Domain.Enums;

namespace CreditRiskEngine.Domain.Models;

public class ClassificationResult
{
    public ClusterType Cluster { get; set; }

    public string ClusterName { get; set; } = string.Empty;

    public JobCategoryType JobCategory { get; set; }

    public decimal JobMultiplier { get; set; }

    public decimal MonthlyIncome { get; set; }

    public decimal BaseLimit { get; set; }

    public decimal PenaltyFactor { get; set; }

    public decimal ApprovedLimit { get; set; }

    public decimal ClusterCap { get; set; }

    public bool Approved { get; set; }
}