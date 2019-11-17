using System;
using FileHelpers;

namespace WebApi.POCO
{
    [DelimitedRecord(",")]
    public class PatientAnalysisResultCsvFormat
    {
        public Guid PatientGuid { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AnalysisName { get; set; }
        public string ReportedName { get; set; }
        public decimal ReferenceLow { get; set; }
        public decimal Entry { get; set; }
        public decimal ReferenceHigh { get; set; }
        public string FormattedEntry { get; set; }
    }
}
