using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces;

namespace WebApi.Implementations
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
    }
}
