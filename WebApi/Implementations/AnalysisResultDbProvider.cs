using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces;

namespace WebApi.Implementations
{
    public class AnalysisResultDbProvider : IAnalysisResultProvider
    {
        private readonly IMainProcessingRepository _repo;
        private readonly IFileParser _parser;

        public AnalysisResultDbProvider(IMainProcessingRepository repo, IFileParser parser)
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

        public void LoadAnalysisResultsFromFile(string path)
        {
            var analysisResults = _parser.GetAnalysisResultsFromCsv(path);
        }

        public void LoadPatientsFromFile(string path)
        {
            var patients = _parser.GetPatientsFromCsv(path);
        }
    }
}
