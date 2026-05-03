using CreditRiskEngine.Application.Services;
using CreditRiskEngine.Domain.Entities;
using CreditRiskEngine.Domain.Enums;
using CreditRiskEngine.Domain.Models;
using CreditRiskEngine.UnitTests.Fixtures;

namespace CreditRiskEngine.UnitTests.Services;

public class ClassificationServiceTests
{
    private readonly ClassificationService _service = new(new FakeRuleProvider());

    [Fact]
    public void Classify_WhenCustomerMeetsClusterAConditions_ShouldReturnDiamond()
    {
        var customer = CreateCustomer(age: 35, score: 700, hasMarketDebt: false, jobTitle: "Senior Engineer");

        var result = _service.Classify(customer);

        Assert.Equal(ClusterType.ClusterA, result.Cluster);
        Assert.Equal("Diamond", result.ClusterName);
        Assert.True(result.Approved);
    }

    [Fact]
    public void Classify_WhenCustomerHasMarketDebt_ShouldSkipClusterAAndReturnClusterB()
    {
        var customer = CreateCustomer(
            age: 35,
            score: 750,
            hasMarketDebt: true,
            marketDebtTypes: new List<string> { "credit_card" },
            jobTitle: "Engineer");

        var result = _service.Classify(customer);

        Assert.Equal(ClusterType.ClusterB, result.Cluster);
        Assert.Equal("Gold", result.ClusterName);
    }

    [Fact]
    public void Classify_WhenScoreIsExactlyClusterCThreshold_ShouldReturnClusterC()
    {
        var customer = CreateCustomer(age: 30, score: 300, hasMarketDebt: true, jobTitle: "Analyst");

        var result = _service.Classify(customer);

        Assert.Equal(ClusterType.ClusterC, result.Cluster);
    }

    [Fact]
    public void Classify_WhenCustomerDoesNotMeetAnyScoreThreshold_ShouldReturnClusterDAndDeny()
    {
        var customer = CreateCustomer(age: 30, score: 100, hasMarketDebt: true, jobTitle: "Unknown");

        var result = _service.Classify(customer);

        Assert.Equal(ClusterType.ClusterD, result.Cluster);
        Assert.False(result.Approved);
        Assert.Equal(0, result.ApprovedLimit);
    }

    [Fact]
    public void Classify_WhenJobTitleHasExecutiveAndSeniorKeywords_ShouldApplyExecutivePriority()
    {
        var customer = CreateCustomer(age: 40, score: 750, hasMarketDebt: false, jobTitle: "Senior Director");

        var result = _service.Classify(customer);

        Assert.Equal(JobCategoryType.Executive, result.JobCategory);
        Assert.Equal(2.0m, result.JobMultiplier);
    }

    [Fact]
    public void Classify_WhenJobTitleHasLowercaseKeyword_ShouldMatchCaseInsensitively()
    {
        var customer = CreateCustomer(age: 40, score: 750, hasMarketDebt: false, jobTitle: "software engineer");

        var result = _service.Classify(customer);

        Assert.Equal(JobCategoryType.MidProfessional, result.JobCategory);
    }

    [Fact]
    public void Classify_WhenDefaultDebtExists_ShouldApplyPenaltyFactor()
    {
        var customer = CreateCustomer(
            age: 40,
            score: 300,
            hasMarketDebt: true,
            marketDebtTypes: new List<string> { "credit_default" },
            jobTitle: "Engineer");

        var result = _service.Classify(customer);

        Assert.Equal(0.5m, result.PenaltyFactor);
    }

    [Fact]
    public void Classify_WhenCalculatedLimitExceedsCap_ShouldEnforceClusterCap()
    {
        var customer = CreateCustomer(age: 40, score: 750, hasMarketDebt: false, jobTitle: "CEO");

        var result = _service.Classify(customer);

        Assert.Equal(100000, result.ApprovedLimit);
    }

    [Fact]
    public void Classify_WhenClusterBMidProfessional_ShouldCalculateExpectedApprovedLimit()
    {
        var customer = CreateCustomer(
            age: 40,
            score: 600,
            hasMarketDebt: true,
            marketDebtTypes: new List<string> { "credit_card" },
            jobTitle: "Software Engineer");

        var result = _service.Classify(customer);

        Assert.Equal(20000, result.ApprovedLimit);
    }

    [Theory]
    [InlineData(ClusterType.ClusterA, JobCategoryType.Executive, 30000)]
    [InlineData(ClusterType.ClusterA, JobCategoryType.SeniorProfessional, 20000)]
    [InlineData(ClusterType.ClusterB, JobCategoryType.MidProfessional, 8000)]
    [InlineData(ClusterType.ClusterC, JobCategoryType.JuniorProfessional, 3000)]
    [InlineData(ClusterType.ClusterD, JobCategoryType.Other, 0)]
    public void Classify_ShouldResolveExpectedMonthlyIncome(
        ClusterType expectedCluster,
        JobCategoryType expectedCategory,
        decimal expectedIncome)
    {
        var customer = (expectedCluster, expectedCategory) switch
        {
            (ClusterType.ClusterA, JobCategoryType.Executive) => CreateCustomer(40, 750, false, "CEO"),
            (ClusterType.ClusterA, JobCategoryType.SeniorProfessional) => CreateCustomer(40, 750, false, "Senior Engineer"),
            (ClusterType.ClusterB, JobCategoryType.MidProfessional) => CreateCustomer(40, 600, true, "Engineer", new List<string> { "credit_card" }),
            (ClusterType.ClusterC, JobCategoryType.JuniorProfessional) => CreateCustomer(40, 300, true, "Junior Assistant"),
            _ => CreateCustomer(40, 100, true, "Unknown")
        };

        var result = _service.Classify(customer);

        Assert.Equal(expectedCluster, result.Cluster);
        Assert.Equal(expectedCategory, result.JobCategory);
        Assert.Equal(expectedIncome, result.MonthlyIncome);
    }

    private static Customer CreateCustomer(
        int age,
        int score,
        bool hasMarketDebt,
        string jobTitle,
        List<string>? marketDebtTypes = null)
    {
        return new Customer
        {
            Id = "customer-1",
            Name = "Test Customer",
            Age = age,
            Score = score,
            HasMarketDebt = hasMarketDebt,
            MarketDebtTypes = marketDebtTypes ?? new List<string>(),
            JobTitle = jobTitle,
            Location = new Location
            {
                City = "São Paulo",
                State = "SP",
                Region = "Sudeste"
            }
        };
    }
}