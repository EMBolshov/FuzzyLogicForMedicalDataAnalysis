using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using POCO.Domain;
using POCO.Domain.Dto;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Controllers
{
    /// <summary>
    /// CRUD for diagnosis
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private readonly IDiagnosisProvider _diagnosisProvider;
        
        public DiagnosisController(Startup.DiagnosisServiceResolver diagnosisServiceResolver)
        {
            _diagnosisProvider = diagnosisServiceResolver("Main");
        }

        /// <summary>
        /// Insert new diagnosis in DB
        /// </summary>
        /// <param name="dto">DTO with all diagnosis info</param>
        [HttpPost("CreateNewDiagnosis")]
        public void CreateNewDiagnosis([FromBody] CreateDiagnosisDto dto)
        {
            _diagnosisProvider.CreateNewDiagnosis(dto);
        }

        /// <summary>
        /// Get all diagnosis with IsRemoved = false
        /// </summary>
        [HttpGet("GetAllDiagnoses")]
        public List<Diagnosis> GetAllDiagnoses()
        {
            return _diagnosisProvider.GetAllDiagnoses();
        }

        /// <summary>
        /// Set IsRemoved = true for diagnosis in DB
        /// </summary>
        /// <param name="diagnosisGuid">Diagnosis's GUID</param>
        [HttpPost("RemoveDiagnosis")]
        public void RemoveDiagnosis([FromBody] Guid diagnosisGuid)
        {
            _diagnosisProvider.RemoveDiagnosis(diagnosisGuid);
        }
    }
}
