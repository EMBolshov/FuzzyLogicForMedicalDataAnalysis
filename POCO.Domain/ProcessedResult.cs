using System;

namespace POCO.Domain
{
    public class ProcessedResult
    {
        public Guid PatientGuid { get; set; }
        public Guid DiagnosisGuid { get; set; }
        public decimal Value { get; set; }
    }
}
