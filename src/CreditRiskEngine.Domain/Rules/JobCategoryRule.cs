using CreditRiskEngine.Domain.Enums;

namespace CreditRiskEngine.Domain.Rules;

public class JobCategoryRule
{
    public int Priority { get; set; }

    public JobCategoryType Category { get; set; }

    public decimal Multiplier { get; set; }

    public List<string> Keywords { get; set; } = new List<string>();
}