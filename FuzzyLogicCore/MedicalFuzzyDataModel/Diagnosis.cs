using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyLogicMedicalCore.FuzzyLogic;

namespace FuzzyLogicMedicalCore.MedicalFuzzyDataModel
{
    public class Diagnosis : ITerm
    {
        public string Name { get; set; }
        public decimal Affiliation { get; set; }
        public List<Rule> Rules { get; set; }
        public Guid PatientGuid { get; set; }

        public void GetAffiliation()
        {
            Affiliation = Rules.Max(x => x.Power);
        }

        public Diagnosis()
        {
            Rules = new List<Rule>();
        }
    }
}
