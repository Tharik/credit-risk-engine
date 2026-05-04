using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json.Serialization;
namespace CreditRiskEngine.IntegrationTests.Controllers;

public class CustomersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CustomersControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Classify_WhenPayloadIsValid_ShouldReturnOkWithClassification()
    {
        var request = new
        {
            id = "customer-1",
            name = "John Doe",
            age = 35,
            score = 750,
            has_market_debt = false,
            market_debt_types = Array.Empty<string>(),
            location = new
            {
                city = "São Paulo",
                state = "SP",
                region = "Sudeste"
            },
            job_title = "Senior Software Engineer"
        };

        var response = await _client.PostAsJsonAsync("/customers/classify", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<CustomerClassificationTestResponse>();

        Assert.NotNull(body);
        Assert.Equal("ClusterA", body!.ClusterId);
        Assert.Equal("Diamond", body.ClusterName);
        Assert.Equal("SeniorProfessional", body.JobCategory);
        Assert.Equal(75000, body.ApprovedLimit);
        Assert.True(body.Approved);
    }

    [Fact]
    public async Task Classify_WhenScoreIsInvalid_ShouldReturnBadRequest()
    {
        var request = new
        {
            id = "customer-1",
            name = "John Doe",
            age = 35,
            score = 1200,
            has_market_debt = false,
            market_debt_types = Array.Empty<string>(),
            location = new
            {
                city = "São Paulo",
                state = "SP",
                region = "Sudeste"
            },
            job_title = "Senior Software Engineer"
        };

        var response = await _client.PostAsJsonAsync("/customers/classify", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Classify_WhenRequiredFieldsAreMissing_ShouldReturnBadRequest()
    {
        var request = new
        {
            age = 35,
            score = 750,
            has_market_debt = false,
            market_debt_types = Array.Empty<string>(),
            location = new
            {
                city = "São Paulo",
                state = "SP",
                region = "Sudeste"
            }
        };

        var response = await _client.PostAsJsonAsync("/customers/classify", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private class CustomerClassificationTestResponse
    {
        [JsonPropertyName("cluster_id")]
        public string ClusterId { get; set; } = string.Empty;

        [JsonPropertyName("cluster_name")]
        public string ClusterName { get; set; } = string.Empty;

        [JsonPropertyName("job_category")]
        public string JobCategory { get; set; } = string.Empty;

        [JsonPropertyName("approved_limit")]
        public decimal ApprovedLimit { get; set; }

        [JsonPropertyName("approved")]
        public bool Approved { get; set; }
    }
}