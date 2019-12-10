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
                new Diagnosis() {Name = "Diagnosis1", MkbCode = "1-11", Loinc = "111"},
                new Diagnosis() {Name = "Diagnosis2", MkbCode = "2-22", Loinc = "222"}
            };
            //TODO: remove stub
            return result;
        }

        public void CreatePatient(CreatePatientDto patientDto)
        {
            throw new NotImplementedException();
        }

        public List<Patient> GetAllPatients()
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

        public void CreateRule(CreateRuleDto ruleDto)
        {
            throw new NotImplementedException();
        }

        public List<Rule> GetAllActiveRules()
        {
            throw new NotImplementedException();
        }
    }
}
