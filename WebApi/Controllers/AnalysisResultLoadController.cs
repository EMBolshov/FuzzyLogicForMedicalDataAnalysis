using Microsoft.AspNetCore.Mvc;
using Repository;
using WebApi.Implementations.Learning;
using WebApi.Implementations.MainProcessing;
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
        private readonly IAnalysisResultProvider _analysisResultProviderLearn;
        private readonly IPatientProvider _patientProviderLearn;
        private readonly IAnalysisResultProvider _analysisResultProviderMain;
        private readonly IPatientProvider _patientProviderMain;
        private readonly IEntitiesToCreateDtoMapper _dtoMapper;

        public AnalysisResultLoadController(ILearningRepository learnRepo, IMainProcessingRepository mainRepo,
            IFileParser parser, IEntitiesToCreateDtoMapper dtoMapper)
        {
            _analysisResultProviderLearn = new AnalysisResultLearningDbProvider(learnRepo, parser);
            _patientProviderLearn = new PatientLearningDbProvider(learnRepo);
            _analysisResultProviderMain = new AnalysisResultDbProvider(mainRepo, parser);
            _patientProviderMain = new PatientDbProvider(mainRepo);
            _dtoMapper = dtoMapper;
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path">C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv</param>
        [HttpGet("LoadAnalysisResultsFromFileToLearningDb")]
        public void LoadAnalysisResultsFromFileToLearningDb(string path)
        {
            var analysisResults = _analysisResultProviderLearn.LoadAnalysisResultsFromFile(path);
            foreach (var analysisResult in analysisResults)
            {
                var dto = _dtoMapper.AnalysisResultToDto(analysisResult);
                _analysisResultProviderLearn.CreateNewAnalysisResult(dto);
            }
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path">C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv</param>
        [HttpGet("LoadPatientsFromFileToLearningDb")]
        public void LoadPatientsFromFileToLearningDb(string path)
        {
            var patients = _analysisResultProviderLearn.LoadPatientsFromFile(path);
            foreach (var patient in patients)
            {
                var dto = _dtoMapper.PatientToCreatePatientDto(patient);
                _patientProviderLearn.CreateNewPatient(dto);
            }
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path">C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv</param>
        [HttpGet("LoadAnalysisResultsFromFileToMainDb")]
        public void LoadAnalysisResultsFromFileToMainDb(string path)
        {
            var analysisResults = _analysisResultProviderLearn.LoadAnalysisResultsFromFile(path);
            foreach (var analysisResult in analysisResults)
            {
                var dto = _dtoMapper.AnalysisResultToDto(analysisResult);
                _analysisResultProviderMain.CreateNewAnalysisResult(dto);
            }
        }

        /// <summary>
        /// C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv
        /// </summary>
        /// <param name="path">C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\data.csv</param>
        [HttpGet("LoadPatientsFromFileToMainDb")]
        public void LoadPatientsFromFileToMainDb(string path)
        {
            var patients = _analysisResultProviderLearn.LoadPatientsFromFile(path);
            foreach (var patient in patients)
            {
                var dto = _dtoMapper.PatientToCreatePatientDto(patient);
                _patientProviderMain.CreateNewPatient(dto);
            }
        }
    }
}
