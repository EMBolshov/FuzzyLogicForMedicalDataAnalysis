using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.POCO;

namespace WebApi.Implementations.MainProcessing
{
    public class MainRepositoryWrapper : IMainProcessingRepository, IService
    {
        private readonly IMainProcessingRepository _repo;
        
        public MainRepositoryWrapper(IOptions<Config> config)
        {
            var mainRepoConnectionString = config.Value.MainProcessingConnectionString;
            _repo = new MainProcessingRepository(mainRepoConnectionString);
        }

        public void CreateDiagnosis(CreateDiagnosisDto diagnosisDto)
        {
            _repo.CreateDiagnosis(diagnosisDto);
        }

        public List<Diagnosis> GetAllDiagnoses()
        {
            return _repo.GetAllDiagnoses();
        }

        public void RemoveDiagnosisByGuid(Guid diagnosisGuid)
        {
            _repo.RemoveDiagnosisByGuid(diagnosisGuid);
        }

        public void CreatePatient(CreatePatientDto dto)
        {
            _repo.CreatePatient(dto);
        }

        public List<Patient> GetAllPatients()
        {
            return _repo.GetAllPatients();
        }

        public void RemovePatientByGuid(Guid patientGuid)
        {
            _repo.RemovePatientByGuid(patientGuid);
        }

        public void CreateAnalysisResult(CreateAnalysisResultDto dto)
        {
            _repo.CreateAnalysisResult(dto);
        }

        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            return _repo.GetAnalysisResultsByPatientGuid(patientGuid);
        }

        public void RemoveAnalysisResultByGuid(Guid analysisResultGuid)
        {
            _repo.RemoveAnalysisResultByGuid(analysisResultGuid);
        }

        public List<AnalysisResult> GetPositiveAnalysisResultsByDiagnosisGuid(Guid diagnosisGuid)
        {
            throw new NotImplementedException();
        }

        public void CreateRule(CreateRuleDto ruleDto)
        {
            _repo.CreateRule(ruleDto);
        }

        public List<Rule> GetAllActiveRules()
        {
            return _repo.GetAllActiveRules();
        }

        public void RemoveRuleByGuid(Guid ruleGuid)
        {
            _repo.RemoveRuleByGuid(ruleGuid);
        }

        public void SaveProcessedResult(ProcessedResult result)
        {
            _repo.SaveProcessedResult(result);
        }

        public List<ProcessedResult> GetAllPositiveResults()
        {
            return _repo.GetAllPositiveResults();
        }
    }
}
