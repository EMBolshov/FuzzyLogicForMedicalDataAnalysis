using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Implementations.Helpers;
using WebApi.Implementations.Learning;
using WebApi.Implementations.MainProcessing;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Controllers
{
    /// <summary>
    /// CRUD for AnalysisResults
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisResultController : ControllerBase
    {
        private readonly IAnalysisResultProvider _analysisResultProvider;

        public AnalysisResultController(IMainProcessingRepository repo, IFileParser parser)
        {
            _analysisResultProvider = new AnalysisResultDbProvider(repo, parser);
        }
        
        /// <summary>
        /// Insert new AnalysisResult into DB
        /// </summary>
        /// <param name="dto">DTO with all info about new AnalysisResult</param>
        [HttpPost("CreateNewAnalysisResult")]
        public void CreateNewAnalysisResult([FromBody] CreateAnalysisResultDto dto)
        {
            _analysisResultProvider.CreateNewAnalysisResult(dto);
        }

        /// <summary>
        /// Return list of AnalysisResults for Patient by GUID
        /// </summary>
        /// <param name="patientGuid">Patient's GUID</param>
        [HttpGet("GetAnalysisResultsByPatientGuid")]
        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            return _analysisResultProvider.GetAnalysisResultsByPatientGuid(patientGuid);
        }

        /// <summary>
        /// Set IsRemoved = True for AnalysisResult into DB
        /// </summary>
        /// <param name="analysisResultGuid">AnalysisResult's GUID</param>
        [HttpPost("RemoveAnalysisResult")]
        public void RemoveAnalysisResult([FromBody] Guid analysisResultGuid)
        {
            _analysisResultProvider.RemoveAnalysisResult(analysisResultGuid);
        }
    }
}
