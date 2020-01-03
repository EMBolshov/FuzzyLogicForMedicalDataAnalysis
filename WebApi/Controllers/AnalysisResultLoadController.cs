using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Controllers
{
    /// <summary>
    /// Load AnalysisResults and Patients from csv to learning db
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisResultLoadController : ControllerBase
    {
        private readonly IAnalysisResultProvider _analysisResultProvider;

        public AnalysisResultLoadController(Startup.AnalysisResultServiceResolver analysisResultServiceResolver)
        {
            _analysisResultProvider = analysisResultServiceResolver("Learning");
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path"></param>
        [HttpGet("LoadAnalysisResultsFromFileToLearningDb")]
        public void LoadAnalysisResultsFromFileToLearningDb(string path)
        {
            var analysisResults = _analysisResultProvider.LoadAnalysisResultsFromFile(path);
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path"></param>
        [HttpGet("LoadPatientsFromFileToLearningDb")]
        public void LoadPatientsFromFileToLearningDb(string path)
        {
            var patients = _analysisResultProvider.LoadPatientsFromFile(path);
        }
    }
}
