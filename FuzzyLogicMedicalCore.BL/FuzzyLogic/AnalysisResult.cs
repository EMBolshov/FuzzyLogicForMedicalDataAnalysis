﻿using System;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class AnalysisResult
    {
        public string AnalysisName { get; set; }
        public bool IsKey { get; set; }
        public decimal CurrentValue { get; set; }
        public LowResult LowResult { get; set; }
        public MidResult MidResult { get; set; }
        public HighResult HighResult { get; set; }
        public Guid PatientGuid { get; set; }
    }
}
