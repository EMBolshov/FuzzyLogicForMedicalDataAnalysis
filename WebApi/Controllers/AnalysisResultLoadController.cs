using Microsoft.AspNetCore.Mvc;
using Repository;
using WebApi.Implementations.Learning;
using WebApi.Interfaces.Helpers;
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
        private readonly IPatientProvider _patientProvider;
        private readonly IEntitiesToCreateDtoMapper _dtoMapper;

        public AnalysisResultLoadController(ILearningRepository repo, IFileParser parser, IEntitiesToCreateDtoMapper dtoMapper)
        {
            _analysisResultProvider = new AnalysisResultLearningDbProvider(repo, parser);
            _patientProvider = new PatientLearningDbProvider(repo);
            _dtoMapper = dtoMapper;
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path">C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv</param>
        [HttpGet("LoadAnalysisResultsFromFileToLearningDb")]
        public void LoadAnalysisResultsFromFileToLearningDb(string path)
        {
            var analysisResults = _analysisResultProvider.LoadAnalysisResultsFromFile(path);
            foreach (var analysisResult in analysisResults)
            {
                var dto = _dtoMapper.AnalysisResultToDto(analysisResult);
                _analysisResultProvider.CreateNewAnalysisResult(dto);
            }
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path">C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv</param>
        [HttpGet("LoadPatientsFromFileToLearningDb")]
        public void LoadPatientsFromFileToLearningDb(string path)
        {
            var patients = _analysisResultProvider.LoadPatientsFromFile(path);
            foreach (var patient in patients)
            {
                var dto = _dtoMapper.PatientToCreatePatientDto(patient);
                _patientProvider.CreateNewPatient(dto);
            }
        }
    }
}
