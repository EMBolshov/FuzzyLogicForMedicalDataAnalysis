using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class RuleLearningDbProvider : IRuleProvider, IService
    {
        private readonly IMainProcessingRepository _repo;

        public RuleLearningDbProvider(Startup.ServiceResolver resolver)
        {
            _repo = resolver("LearningRepo") as IMainProcessingRepository;
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
