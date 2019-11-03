using Microsoft.AspNetCore.Mvc;
using Repository;
using WebApi.Implementations;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private readonly IDiagnosisProvider _diagnosisProvider;
        private readonly IMainProcessingRepository _mainProcessingRepository;
        private readonly string _mainRepoConnectionString;
        private readonly IConfiguration _config;

        public DiagnosisController()
        {
            _mainRepoConnectionString = "";
            _mainProcessingRepository = new MainProcessingRepository(_mainRepoConnectionString);
            _diagnosisProvider = new DiagnosisProvider(_mainProcessingRepository);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void CreateNewDiagnosis([FromBody] string name)
        {
            _diagnosisProvider.CreateNewDiagnosis(name);
        }
    }
}
