using System;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;

namespace FuzzyLogicMedicalCore.BL.MedicalFuzzyDataModel
{
    public class AnalysisResult
    {
        public string AnalysisName { get; set; }
        public decimal CurrentValue { get; set; }
        public LowResult LowResult { get; set; }
        public MidResult MidResult { get; set; }
        public HighResult HighResult { get; set; }
        public Guid PatientGuid { get; set; }
    }
}
