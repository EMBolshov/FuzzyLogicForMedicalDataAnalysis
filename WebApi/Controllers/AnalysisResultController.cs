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
    public class AnalysisResultController : ControllerBase
    {
        private readonly IAnalysisResultProvider _analysisResultProvider;

        public AnalysisResultController(IAnalysisResultProvider analysisResultProvider)
        {
            _analysisResultProvider = analysisResultProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost("CreateAnalysisResult")]
        public void CreateNewDiagnosis([FromBody] CreateAnalysisResultDto dto)
        {
            _analysisResultProvider.CreateNewAnalysisResult(dto);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("GetAnalysisResultsByPatientGuid")]
        public void GetAllDiagnoses(Guid patientGuid)
        {
            _analysisResultProvider.GetAnalysisResultsByPatientGuid(patientGuid);
        }
    }
}
