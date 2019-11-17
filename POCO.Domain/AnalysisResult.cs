using System;

namespace POCO.Domain
{
    public class AnalysisResult
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public Guid PatientGuid { get; set; }
        public DateTime InsertedDate { get; set; }
        public string AnalysisName { get; set; }
        public string TestName { get; set; }
        public string ReportedName { get; set; }
        public decimal Entry { get; set; }
        public string FormattedEntry { get; set; }
        //todo: Make reference provider
        public decimal ReferenceLow { get; set; }
        public decimal ReferenceHigh { get; set; }
        public bool IsRemoved { get; set; }
    }
}
