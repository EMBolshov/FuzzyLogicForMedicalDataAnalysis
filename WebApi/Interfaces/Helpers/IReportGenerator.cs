using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Interfaces.Helpers
{
    //TODO: DTO for ReportGenerator
    public interface IReportGenerator
    {
        void GenerateReport(ProcessedResult processedResult, Patient patient, List<AnalysisResult> analysisResults, List<Diagnosis> diagnoses, string path);
    }
}
