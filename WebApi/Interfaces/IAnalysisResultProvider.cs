using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAnalysisResultProvider
    {
        List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid);

        void CreateNewAnalysisResult(CreateAnalysisResultDto dto);

        void RemoveAnalysisResult(Guid analysisResultGuid);

        void LoadAnalysisResultsFromFile(string path);

        void LoadPatientsFromFile(string path);
    }
}
