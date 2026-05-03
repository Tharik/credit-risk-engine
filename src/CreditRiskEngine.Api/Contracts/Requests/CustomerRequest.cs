namespace CreditRiskEngine.Api.Contracts.Requests;

public class CustomerRequest
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public int Score { get; set; }

    public bool HasMarketDebt { get; set; }

    public List<string> MarketDebtTypes { get; set; } = new List<string>();

    public LocationRequest Location { get; set; } = new LocationRequest();

    public string JobTitle { get; set; } = string.Empty;
}