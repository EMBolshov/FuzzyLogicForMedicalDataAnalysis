using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repository;
using WebApi.Implementations;
using WebApi.Interfaces;
using WebApi.POCO;

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
        [HttpPost("CreateNewDiagnosis")]
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
