using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace Repository
{
    public interface IMainProcessingRepository
    {
        void CreateDiagnosis(CreateDiagnosisDto diagnosisDto);
        List<Diagnosis> GetAllDiagnoses();
        void RemoveDiagnosisByGuid(Guid diagnosisGuid);
        void CreatePatient(CreatePatientDto patientDto);
        List<Patient> GetAllPatients();
        void RemovePatientByGuid(Guid patientGuid);
        void CreateAnalysisResult(CreateAnalysisResultDto dto);
        List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid);
        void RemoveAnalysisResultByGuid(Guid analysisResultGuid);
        List<AnalysisResult> GetPositiveAnalysisResultsByDiagnosisGuid(Guid diagnosisGuid);
        void CreateRule(CreateRuleDto ruleDto);
        List<Rule> GetAllActiveRules();
        void RemoveRuleByGuid(Guid ruleGuid);
    }
}
