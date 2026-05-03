using CreditRiskEngine.Api.Contracts.Requests;

namespace CreditRiskEngine.Api.Contracts.Responses;

public class CustomerClassificationResponse
{
    public CustomerRequest Customer { get; set; } = new CustomerRequest();

    public string ClusterId { get; set; } = string.Empty;

    public string ClusterName { get; set; } = string.Empty;

    public string JobCategory { get; set; } = string.Empty;

    public decimal JobMultiplier { get; set; }

    public decimal MonthlyIncome { get; set; }

    public decimal BaseLimit { get; set; }

    public decimal PenaltyFactor { get; set; }

    public decimal ApprovedLimit { get; set; }

    public decimal ClusterCap { get; set; }

    public bool Approved { get; set; }
}