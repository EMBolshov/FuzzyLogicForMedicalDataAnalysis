using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Implementations.MainProcessing;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Controllers
{
    /// <summary>
    /// CRUD for rules
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IRuleProvider _ruleProvider;

        public RuleController(IMainProcessingRepository repo)
        {
            _ruleProvider = new RuleDbProvider(repo);
        }

        /// <summary>
        /// Insert new rule in DB
        /// </summary>
        /// <param name="dto">DTO with all rule info</param>
        [HttpPost("CreateNewRule")]
        public void CreateNewRule([FromBody] CreateRuleDto dto)
        {
            _ruleProvider.CreateRule(dto);
        }

        /// <summary>
        /// Returns list with all rules where IsRemoved = false
        /// </summary>
        [HttpGet("GetAllActiveRules")]
        public List<Rule> GetAllActiveRules()
        {
            return _ruleProvider.GetAllActiveRules();
        }

        /// <summary>
        /// Set IsRemoved = true for rule by GUID
        /// </summary>
        /// <param name="ruleGuid">Rule's GUID</param>
        [HttpPost("RemoveRule")]
        public void RemoveRule([FromBody] Guid ruleGuid)
        {
            _ruleProvider.RemoveRule(ruleGuid);
        }
    }
}
