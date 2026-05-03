namespace CreditRiskEngine.Domain.Rules;

public class PenaltyRule
{
    public int Priority { get; set; }

    public string RuleId { get; set; } = string.Empty;

    public List<string> TriggerDebtTypes { get; set; } = new List<string>();

    public decimal PenaltyFactor { get; set; }
}