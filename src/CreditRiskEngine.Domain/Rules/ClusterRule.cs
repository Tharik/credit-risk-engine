using CreditRiskEngine.Domain.Enums;

namespace CreditRiskEngine.Domain.Rules;

public class ClusterRule
{
    public int Priority { get; set; }

    public ClusterType Cluster { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? MinScore { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public bool RequiresNoMarketDebt { get; set; }

    public bool DenyIfDefaultDebtExists { get; set; }

    public decimal BaseLimit { get; set; }

    public decimal Cap { get; set; }
}