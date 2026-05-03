using System.Text.Json;
using CreditRiskEngine.Domain.Interfaces;
using CreditRiskEngine.Domain.Rules;

namespace CreditRiskEngine.Infrastructure.RuleProviders;

public class JsonRuleProvider : IRuleProvider
{
    private readonly string _rulesFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonRuleProvider(string rulesFilePath)
    {
        _rulesFilePath = rulesFilePath;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public CreditEngineRules GetRules()
    {
        if (!File.Exists(_rulesFilePath))
        {
            throw new FileNotFoundException("Rules configuration file was not found.", _rulesFilePath);
        }

        var json = File.ReadAllText(_rulesFilePath);

        var rules = JsonSerializer.Deserialize<CreditEngineRules>(json, _jsonOptions);

        if (rules is null)
        {
            throw new InvalidOperationException("Rules configuration file is invalid or empty.");
        }

        return rules;
    }
}