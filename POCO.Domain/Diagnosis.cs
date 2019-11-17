using System;

namespace POCO.Domain
{
    public class Diagnosis
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string MkbCode { get; set; }
        public string Loinc { get; set; }
        public bool IsRemoved { get; set; }
    }
}
