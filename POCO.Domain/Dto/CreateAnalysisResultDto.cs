using System;

namespace POCO.Domain.Dto
{
    public class CreateAnalysisResultDto
    {
        public Guid PatientGuid { get; set; }
        public string AnalysisName { get; set; }
        public decimal ResultValue { get; set; }
    }
}
