using System.Text.Json.Serialization;
using CreditRiskEngine.Api.Contracts.Requests;

namespace CreditRiskEngine.Api.Contracts.Responses;

public class CustomerClassificationResponse
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

    [JsonPropertyName("cluster_id")]
    public string ClusterId { get; set; } = string.Empty;

    [JsonPropertyName("cluster_name")]
    public string ClusterName { get; set; } = string.Empty;

    [JsonPropertyName("job_category")]
    public string JobCategory { get; set; } = string.Empty;

    [JsonPropertyName("job_multiplier")]
    public decimal JobMultiplier { get; set; }

    [JsonPropertyName("monthly_income")]
    public decimal MonthlyIncome { get; set; }

    [JsonPropertyName("base_limit")]
    public decimal BaseLimit { get; set; }

    [JsonPropertyName("penalty_factor")]
    public decimal PenaltyFactor { get; set; }

    [JsonPropertyName("approved_limit")]
    public decimal ApprovedLimit { get; set; }

    [JsonPropertyName("cluster_cap")]
    public decimal ClusterCap { get; set; }

    [JsonPropertyName("approved")]
    public bool Approved { get; set; }
}