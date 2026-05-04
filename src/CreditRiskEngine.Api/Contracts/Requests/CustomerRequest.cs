using System.Text.Json.Serialization;

namespace CreditRiskEngine.Api.Contracts.Requests;

public class CustomerRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("has_market_debt")]
    public bool HasMarketDebt { get; set; }

    [JsonPropertyName("market_debt_types")]
    public List<string> MarketDebtTypes { get; set; } = new();

    [JsonPropertyName("location")]
    public LocationRequest Location { get; set; } = new();

    [JsonPropertyName("job_title")]
    public string JobTitle { get; set; } = string.Empty;
}