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
            if (Rules.Count > 0)
            {
                Affiliation = Rules.Max(x => x.Power);
            }
            else
            {
                Affiliation = 0;
            }
        }

        public Diagnosis()
        {
            Rules = new List<FuzzyRule>();
        }
    }
}
