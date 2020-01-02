using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces
{
    public interface IAnalysisResultProvider
    {
        List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid);

        void CreateNewAnalysisResult(CreateAnalysisResultDto dto);

        void RemoveAnalysisResult(Guid analysisResultGuid);

        List<AnalysisResult> LoadAnalysisResultsFromFile(string path);

        List<Patient> LoadPatientsFromFile(string path);
    }
}
