using CreditRiskEngine.Domain.Entities;
using CreditRiskEngine.Domain.Models;

namespace CreditRiskEngine.Application.Interfaces;

public interface IClassificationService
{
    ClassificationResult Classify(Customer customer);
}