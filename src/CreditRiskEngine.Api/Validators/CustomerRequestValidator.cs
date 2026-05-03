using CreditRiskEngine.Api.Contracts.Requests;
using FluentValidation;

namespace CreditRiskEngine.Api.Validators;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    private static readonly string[] ValidRegions =
    {
        "Norte",
        "Nordeste",
        "Centro-Oeste",
        "Sudeste",
        "Sul"
    };

    private static readonly string[] ValidDebtTypes =
    {
        "credit_card",
        "personal_loan",
        "mortgage",
        "credit_default",
        "loan_default"
    };

    public CustomerRequestValidator()
    {
        RuleFor(customer => customer.Id)
            .NotEmpty();

        RuleFor(customer => customer.Name)
            .NotEmpty();

        RuleFor(customer => customer.Age)
            .GreaterThanOrEqualTo(18);

        RuleFor(customer => customer.Score)
            .InclusiveBetween(0, 1000);

        RuleFor(customer => customer.JobTitle)
            .NotEmpty();

        RuleFor(customer => customer.Location)
            .NotNull();

        RuleFor(customer => customer.Location.City)
            .NotEmpty()
            .When(customer => customer.Location is not null);

        RuleFor(customer => customer.Location.State)
            .NotEmpty()
            .Length(2)
            .When(customer => customer.Location is not null);

        RuleFor(customer => customer.Location.Region)
            .Must(region => ValidRegions.Contains(region))
            .WithMessage("Region must be one of: Norte, Nordeste, Centro-Oeste, Sudeste, Sul.")
            .When(customer => customer.Location is not null);

        RuleForEach(customer => customer.MarketDebtTypes)
            .Must(debtType => ValidDebtTypes.Contains(debtType))
            .WithMessage("Invalid market debt type.");
    }
}