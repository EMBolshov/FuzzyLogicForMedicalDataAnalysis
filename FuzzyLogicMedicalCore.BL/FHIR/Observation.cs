using System;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;

namespace FuzzyLogicMedicalCore.BL.FHIR
{
    public class Observation
    {
        //todo use FHIR resource
        public string IndicationName { get; set; }
        public decimal ReferenceLow { get; set; }
        public decimal ReferenceHigh { get; set; }
        public decimal Value { get; set; }
        public Guid PatientGuid { get; set; }
        public HighResult HighResult { get; set; }
        public MidResult MidResult { get; set; }
        public LowResult LowResult { get; set; }
    }
}
