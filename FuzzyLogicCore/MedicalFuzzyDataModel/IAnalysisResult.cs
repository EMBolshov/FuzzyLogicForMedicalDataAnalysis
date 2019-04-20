using System;
using FuzzyLogicMedicalCore.FuzzyLogic;

namespace FuzzyLogicMedicalCore.MedicalFuzzyDataModel
{
    public interface IAnalysisResult
    {
        string AnalysisName { get; set; }
        LowResult LowResult { get; set; }
        MidResult MidResult { get; set; }
        HighResult HighResult { get; set; }
        Guid PatientGuid { get; set; }
    }
}
