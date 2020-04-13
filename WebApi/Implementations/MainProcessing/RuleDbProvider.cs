using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.MainProcessing
{
    public class RuleDbProvider : IRuleProvider
    {
        private readonly IMainProcessingRepository _repo;


        public RuleDbProvider(IMainProcessingRepository repo)
        {
            _repo = repo;
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

        public void DeleteAllRules()
        {
            _repo.DeleteAllRules();
        }
    }
}
