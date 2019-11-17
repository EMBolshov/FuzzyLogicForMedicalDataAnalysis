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
    public class DiagnosisController : ControllerBase
    {
        private readonly IDiagnosisProvider _diagnosisProvider;
        
        public DiagnosisController(IDiagnosisProvider diagnosisProvider)
        {
            _diagnosisProvider = diagnosisProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost("CreateDiagnosis")]
        public void CreateNewDiagnosis([FromBody] CreateDiagnosisDto dto)
        {
            _diagnosisProvider.CreateNewDiagnosis(dto);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("GetAllDiagnoses")]
        public void GetAllDiagnoses()
        {
            _diagnosisProvider.GetAllDiagnoses();
        }
    }
}
