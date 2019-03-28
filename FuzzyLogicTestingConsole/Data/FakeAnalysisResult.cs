using System;
using FuzzyLogicMedicalCore.FuzzyLogic;

namespace FuzzyLogicTestingConsole.Data
{
    public class FakeAnalysisResult
    {
        public string AnalysisName { get; set; }
        public LowResult LowResult { get; set; }
        public MidResult MidResult { get; set; }
        public HighResult HighResult { get; set; }
        public Guid PatientGuid { get; set; }
    }
}
