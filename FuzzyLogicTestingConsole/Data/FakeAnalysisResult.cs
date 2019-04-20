﻿using System;
using FuzzyLogicMedicalCore.MedicalFuzzyDataModel;
using FuzzyLogicMedicalCore.FuzzyLogic;

namespace FuzzyLogicTestingConsole.Data
{
    public class FakeAnalysisResult : IAnalysisResult
    {
        public string AnalysisName { get; set; }
        public LowResult LowResult { get; set; }
        public MidResult MidResult { get; set; }
        public HighResult HighResult { get; set; }
        public Guid PatientGuid { get; set; }
    }
}