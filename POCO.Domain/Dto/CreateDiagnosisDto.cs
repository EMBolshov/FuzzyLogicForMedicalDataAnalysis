using System;

namespace POCO.Domain.Dto
{
    public class CreateDiagnosisDto
    {
        public string DiagnosisName { get; set; }
        public Guid Guid { get; set; }
        public string MkbCode { get; set; }
        public bool IsRemoved { get; set; }
    }
}
