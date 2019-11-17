using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.POCO;

namespace WebApi.Implementations
{
    public class MainRepositoryWrapper : IMainProcessingRepository
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

        public void CreatePatient(CreatePatientDto dto)
        {
            _repo.CreatePatient(dto);
        }

        public List<Patient> GetAllPatients()
        {
            return _repo.GetAllPatients();
        }

        public void CreateAnalysisResult(CreateAnalysisResultDto dto)
        {
            _repo.CreateAnalysisResult(dto);
        }

        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            return _repo.GetAnalysisResultsByPatientGuid(patientGuid);
        }

        public void CreateRule(CreateRuleDto ruleDto)
        {
            _repo.CreateRule(ruleDto);
        }

        public List<Rule> GetAllRules()
        {
            return _repo.GetAllRules();
        }
    }
}
