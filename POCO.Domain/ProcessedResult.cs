using System;

namespace POCO.Domain
{
    public class ProcessedResult
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Guid PatientGuid { get; set; }
        public Guid DiagnosisGuid { get; set; }
        public decimal Value { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
