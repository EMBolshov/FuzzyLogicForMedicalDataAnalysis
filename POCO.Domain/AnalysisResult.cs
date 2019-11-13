using System;

namespace POCO.Domain
{
    public class AnalysisResult
    {
        public Guid Guid { get; set; }
        public Guid PatientGuid { get; set; }
        public string AnalysisName { get; set; }
        public decimal ResultValue { get; set; }
    }
}
