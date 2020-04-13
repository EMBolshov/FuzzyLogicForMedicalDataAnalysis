using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class AnalysisResultLearningDbProvider : IAnalysisResultProvider
    {
        private readonly ILearningRepository _repo;
        private readonly IFileParser _parser;

        public AnalysisResultLearningDbProvider(ILearningRepository repo, IFileParser parser)
        {
            _repo = repo;
            _parser = parser;
        }

        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            return _repo.GetAnalysisResultsByPatientGuid(patientGuid);
        }

        public void CreateNewAnalysisResult(CreateAnalysisResultDto dto)
        {
            _repo.CreateAnalysisResult(dto);
        }

        public void RemoveAnalysisResult(Guid analysisResultGuid)
        {
            _repo.RemoveAnalysisResultByGuid(analysisResultGuid);
        }

        public void ReturnAnalysisResult(Guid analysisResultGuid)
        {
            _repo.ReturnAnalysisResultByGuid(analysisResultGuid);
        }

        public List<AnalysisResult> LoadAnalysisResultsFromFile(string path)
        {
            return _parser.GetAnalysisResultsFromCsv(path);
        }

        public List<Patient> LoadPatientsFromFile(string path)
        {
            return _parser.GetPatientsFromCsv(path);
        }

        public List<AnalysisResult> GetPositiveAnalysisResultsByDiagnosisGuid(Guid diagnosisGuid)
        {
            return _repo.GetPositiveAnalysisResultsByDiagnosisGuid(diagnosisGuid);
        }
    }
}
