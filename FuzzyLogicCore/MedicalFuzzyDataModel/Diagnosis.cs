using System;
using FuzzyLogicMedicalCore.FuzzyLogic;

namespace FuzzyLogicMedicalCore.MedicalFuzzyDataModel
{
    public class Diagnosis : ITerm
    {
        public string Name { get; set; }
        public decimal Affiliation { get; set; }
        public Guid PatientGuid { get; set; }

        public void GetAffiliation()
        {
            //todo select formula for each term type
            var result = 123;
            Affiliation = result;
        }
    }
}
