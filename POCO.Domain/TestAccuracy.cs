using System;

namespace POCO.Domain
{
    public class TestAccuracy
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime InsertedDate { get; set; }
        public string TestName { get; set; }
        public Guid DiagnosisGuid { get; set; }
        public decimal TrulyPositive { get; set; }
        public decimal TrulyNegative { get; set; }
        public decimal FalselyPositive { get; set; }
        public decimal FalselyNegative { get; set; }
        public decimal OverallAccuracy { get; set; }
        public bool IsRemoved { get; set; }
    }
}
