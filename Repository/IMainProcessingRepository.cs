﻿using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace Repository
{
    public interface IMainProcessingRepository
    {
        void CreateDiagnosis(CreateDiagnosisDto diagnosisDto);
        List<Diagnosis> GetAllDiagnoses();
        void CreatePatient(CreatePatientDto patientDto);
        List<Patient> GetAllPatients();
        void CreateAnalysisResult(CreateAnalysisResultDto dto);
        List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid);
        void CreateRule(CreateRuleDto ruleDto);
        List<Rule> GetAllRules();
    }
}
