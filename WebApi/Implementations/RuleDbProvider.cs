using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces;
using WebApi.POCO;

namespace WebApi.Implementations
{
    public class RuleDbProvider : IRuleProvider
    {
        private readonly IMainProcessingRepository _repo;

        public RuleDbProvider(IMainProcessingRepository repo)
        {
            _repo = repo;
        }

        public List<Rule> GetAllRules()
        {
            return _repo.GetAllRules();
        }

        public void CreateRule(CreateRuleDto dto)
        {
            _repo.CreateRule(dto);
        }
    }
}
