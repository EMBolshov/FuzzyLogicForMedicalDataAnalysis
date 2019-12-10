using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRuleProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Rule> GetAllActiveRules();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleDto"></param>
        void CreateRule(CreateRuleDto ruleDto);
    }
}
