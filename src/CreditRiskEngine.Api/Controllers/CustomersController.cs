using CreditRiskEngine.Api.Contracts.Requests;
using CreditRiskEngine.Api.Contracts.Responses;
using CreditRiskEngine.Application.Interfaces;
using CreditRiskEngine.Domain.Entities;
using CreditRiskEngine.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CreditRiskEngine.Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomersController : ControllerBase
{
    private readonly IClassificationService _classificationService;

    public CustomersController(IClassificationService classificationService)
    {
        _classificationService = classificationService;
    }

    /// <summary>
    /// Classifies a customer into a credit risk cluster,
    /// calculates approved credit limit,
    /// and estimates monthly income.
    /// </summary>
    /// <param name="request">Customer input payload.</param>
    /// <returns>Enriched customer classification result.</returns>
    [HttpPost("classify")]
    [ProducesResponseType(typeof(CustomerClassificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<CustomerClassificationResponse> Classify([FromBody] CustomerRequest request)
    {
        var customer = MapToDomain(request);

        var result = _classificationService.Classify(customer);

        return Ok(MapToResponse(request, result));
    }

    private static Customer MapToDomain(CustomerRequest request)
    {
        return new Customer
        {
            Id = request.Id,
            Name = request.Name,
            Age = request.Age,
            Score = request.Score,
            HasMarketDebt = request.HasMarketDebt,
            MarketDebtTypes = request.MarketDebtTypes,
            JobTitle = request.JobTitle,
            Location = new Location
            {
                City = request.Location.City,
                State = request.Location.State,
                Region = request.Location.Region
            }
        };
    }

    private static CustomerClassificationResponse MapToResponse(
        CustomerRequest request,
        CreditRiskEngine.Domain.Models.ClassificationResult result)
    {
        return new CustomerClassificationResponse
        {
            Customer = request,
            ClusterId = result.Cluster.ToString(),
            ClusterName = result.ClusterName,
            JobCategory = result.JobCategory.ToString(),
            JobMultiplier = result.JobMultiplier,
            MonthlyIncome = result.MonthlyIncome,
            BaseLimit = result.BaseLimit,
            PenaltyFactor = result.PenaltyFactor,
            ApprovedLimit = result.ApprovedLimit,
            ClusterCap = result.ClusterCap,
            Approved = result.Approved
        };
    }
}