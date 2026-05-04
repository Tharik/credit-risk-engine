using System.Text.Json.Serialization;

namespace CreditRiskEngine.Api.Contracts.Requests;

public class LocationRequest
{
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;
}