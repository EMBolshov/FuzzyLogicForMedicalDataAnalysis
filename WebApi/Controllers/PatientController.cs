using Microsoft.AspNetCore.Mvc;
using POCO.Domain.Dto;
using WebApi.Interfaces;
using WebApi.POCO;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientProvider _patientProvider;
        
        public PatientController(IPatientProvider patientProvider)
        {
            _patientProvider = patientProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost("CreatePatient")]
        public void CreateNewDiagnosis([FromBody] CreatePatientDto dto)
        {
            _patientProvider.CreateNewPatient(dto);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("GetAllPatients")]
        public void GetAllDiagnoses()
        {
            _patientProvider.GetAllPatients();
        }
    }
}
