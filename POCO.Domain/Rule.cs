using System;

namespace POCO.Domain
{
    public class Rule
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string DiagnosisName { get; set; }
        public string Test { get; set; }
        public decimal Power { get; set; }
        public string InputTermName { get; set; }
        public bool IsRemoved { get; set; }
    }
}
