using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces.MainProcessing
{
    public interface IRuleProvider
    {
        List<Rule> GetAllActiveRules();

        void CreateRule(CreateRuleDto ruleDto);

        void RemoveRule(Guid ruleGuid);
    }
}
