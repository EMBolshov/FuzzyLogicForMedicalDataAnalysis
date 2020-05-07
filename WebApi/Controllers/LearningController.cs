using Microsoft.AspNetCore.Mvc;
using POCO.Domain.Dto;
using WebApi.Interfaces.Learning;

namespace WebApi.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        private readonly ILearningProcessor _learningProcessor;

        public LearningController(ILearningProcessor learningProcessor)
        {
            _learningProcessor = learningProcessor;
        }

        /// <summary>
        /// Learn
        /// </summary>
        [HttpGet("Learn")]
        public void Learn()
        {
            //var results = _learningProcessor.ProcessForAllPatients();
            _learningProcessor.CalculateTestAccuracy();
            var errorRatio = _learningProcessor.GetErrorRatio();
            while (errorRatio >= 10m)
            {
                _learningProcessor.CalculateTestAccuracy(false);
                errorRatio = _learningProcessor.GetErrorRatio();
            }

            _learningProcessor.ProcessForAllPatients();
        }

        /// <summary>
        /// TODO
        /// </summary>
        [HttpGet("GetErrorRatio")]
        public decimal GetErrorRatio()
        {
            var results = _learningProcessor.GetErrorRatio();
            return results;
        }

        /// <summary>
        /// TODO
        /// </summary>
        [HttpGet("CalculateTestAccuracy")]
        public void CalculateTestAccuracy()
        {
            _learningProcessor.CalculateTestAccuracy();
        }

        /// <summary>
        /// TODO
        /// </summary>
        [HttpGet("CreateBaseRules")]
        public void CreateBaseRules()
        {
            _learningProcessor.CreateBaseRules();
        }

        /// <summary>
        /// Insert new rule in learning DB
        /// </summary>
        /// <param name="dto">DTO with all rule info</param>
        [HttpPost("CreateNewRule")]
        public void CreateNewRule([FromBody] CreateRuleDto dto)
        {
            _learningProcessor.CreateRule(dto);
        }

        /// <summary>
        /// Insert new diagnosis in learning DB
        /// </summary>
        /// <param name="dto">DTO with all diagnosis info</param>
        [HttpPost("CreateNewDiagnosis")]
        public void CreateNewDiagnosis([FromBody] CreateDiagnosisDto dto)
        {
            _learningProcessor.CreateNewDiagnosis(dto);
        }
    }
}
