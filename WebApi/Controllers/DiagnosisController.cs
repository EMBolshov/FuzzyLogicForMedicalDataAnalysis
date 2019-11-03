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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosisProvider"></param>
        public DiagnosisController(IDiagnosisProvider diagnosisProvider)
        {
            _diagnosisProvider = diagnosisProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        [HttpPost("CreateNewDiagnosis")]
        public void CreateNewDiagnosis([FromBody] CreateDiagnosisDto dto)
        {
            _diagnosisProvider.CreateNewDiagnosis(dto.DiagnosisName);
        }
    }
}
