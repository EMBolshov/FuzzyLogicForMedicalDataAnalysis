using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Implementations.Helpers
{
    public class ReportModel
    {
        public List<ProcessedResult> ProcessedResults { get; set; }
        public Patient Patient { get; set; }
        public List<AnalysisResult> AnalysisResults { get; set; }
        public List<Diagnosis> Diagnoses { get; set; }
        public string Path { get; set; }
    }
}
