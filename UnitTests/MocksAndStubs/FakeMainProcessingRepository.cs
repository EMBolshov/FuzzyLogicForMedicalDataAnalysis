using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;

namespace UnitTests.MocksAndStubs
{
    public class FakeMainProcessingRepository : IMainProcessingRepository
    {
        //TODO static list?
        public void CreateDiagnosis(CreateDiagnosisDto diagnosisDto)
        {
            throw new NotImplementedException();
        }

        public List<Diagnosis> GetAllDiagnoses()
        {
            var result = new List<Diagnosis>
            {
                new Diagnosis() {Name = "Diagnosis1", MkbCode = "1-11"},
                new Diagnosis() {Name = "Diagnosis2", MkbCode = "2-22"}
            };
            //TODO: remove stub
            return result;
        }

        public void RemoveDiagnosisByGuid(Guid diagnosisGuid)
        {
            throw new NotImplementedException();
        }

        public void CreatePatient(CreatePatientDto patientDto)
        {
            throw new NotImplementedException();
        }

        public List<Patient> GetAllPatients()
        {
            throw new NotImplementedException();
        }

        public void RemovePatientByGuid(Guid patientGuid)
        {
            throw new NotImplementedException();
        }

        public void CreateAnalysisResult(CreateAnalysisResultDto dto)
        {
            throw new NotImplementedException();
        }

        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            throw new NotImplementedException();
        }

        public void RemoveAnalysisResultByGuid(Guid analysisResultGuid)
        {
            throw new NotImplementedException();
        }

        public List<AnalysisResult> GetPositiveAnalysisResultsByDiagnosisGuid(Guid diagnosisGuid)
        {
            throw new NotImplementedException();
        }

        public void CreateRule(CreateRuleDto ruleDto)
        {
            throw new NotImplementedException();
        }

        public List<Rule> GetAllActiveRules()
        {
            throw new NotImplementedException();
        }

        public void RemoveRuleByGuid(Guid ruleGuid)
        {
            throw new NotImplementedException();
        }

        public void SaveProcessedResult(ProcessedResult result)
        {
            throw new NotImplementedException();
        }

        public List<ProcessedResult> GetAllPositiveResults()
        {
            throw new NotImplementedException();
        }

        public void CreateTestAccuracy(TestAccuracy testAccuracy)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllRules()
        {
            throw new NotImplementedException();
        }

        public List<TestAccuracy> GetAllTestAccuracies()
        {
            throw new NotImplementedException();
        }
    }
}
