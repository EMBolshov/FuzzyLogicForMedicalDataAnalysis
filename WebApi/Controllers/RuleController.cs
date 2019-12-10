using Microsoft.AspNetCore.Mvc;
using POCO.Domain.Dto;
using WebApi.Interfaces;

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

        public RuleController(IRuleProvider ruleProvider)
        {
            _ruleProvider = ruleProvider;
        }

        /// <summary>
        /// Create new rule
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost("CreateRule")]
        public void CreateNewRule([FromBody] CreateRuleDto dto)
        {
            _ruleProvider.CreateRule(dto);
        }

        [HttpGet("GetAllActiveRules")]
        public void GetAllActiveRules()
        {
            _ruleProvider.GetAllActiveRules();
        }
    }
}
