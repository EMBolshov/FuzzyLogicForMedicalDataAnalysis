using System;
using Microsoft.AspNetCore.Mvc;
using POCO.Domain.Dto;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost("CreateRule")]
        public void CreateNewDiagnosis([FromBody] CreateRuleDto dto)
        {
            _ruleProvider.CreateRule(dto);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("GetAllRules")]
        public void GetAllRules()
        {
            _ruleProvider.GetAllRules();
        }
    }
}
