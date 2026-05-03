using CreditRiskEngine.Domain.Models;

namespace CreditRiskEngine.Domain.Entities;

public class Customer
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public int Score { get; set; }

    public bool HasMarketDebt { get; set; }

    public List<string> MarketDebtTypes { get; set; } = new List<string>();

    public Location Location { get; set; } = new();

    public string JobTitle { get; set; } = string.Empty;
}