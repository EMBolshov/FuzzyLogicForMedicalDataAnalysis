using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.MainProcessing
{
    public class RuleDbProvider : IRuleProvider, IService
    {
        private readonly IMainProcessingRepository _repo;

        public RuleDbProvider(Startup.ServiceResolver resolver)
        {
            _repo = resolver("MainRepo") as IMainProcessingRepository;
        }

        public List<Rule> GetAllActiveRules()
        {
            return _repo.GetAllActiveRules();
        }

        public void CreateRule(CreateRuleDto dto)
        {
            _repo.CreateRule(dto);
        }

        public void RemoveRule(Guid ruleGuid)
        {
            _repo.RemoveRuleByGuid(ruleGuid);
        }
    }
}
