using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class Diagnosis : ITerm
    {
        public string Name { get; set; }
        public decimal Affiliation { get; set; }
        public List<FuzzyRule> Rules { get; set; }
        public Guid PatientGuid { get; set; }

        public void GetAffiliation()
        {
            Affiliation = Rules.Max(x => x.Power);
        }

        public Diagnosis()
        {
            Rules = new List<FuzzyRule>();
        }
    }
}
