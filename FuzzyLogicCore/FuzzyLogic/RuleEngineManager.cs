using System.Collections.Generic;
using FuzzyLogicMedicalCore.FHIR;

namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public class RuleEngineManager
    {
        public List<Rule> GetAllRules()
        {
            //TODO get rules from db
            var allRules = new List<Rule>();
            return allRules;
        }

        public void GetOutputTerms(List<Observation> observationList)
        {

        }
    }
}
