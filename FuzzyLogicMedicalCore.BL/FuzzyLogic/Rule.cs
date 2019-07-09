using System.Collections.Generic;
using System.Linq;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class Rule
    {
        public int Id { get; set; }
        public List<InputTerm> InputTerms { get; set; } 
        public List<string> OutputTerms { get; set; } 
        public decimal Power { get; set; }

        public void GetPower(List<AnalysisResult> results)
        {
            var affiliations = new List<decimal>();

            foreach (var inputTerm in InputTerms)
            {
                foreach (var result in results)
                {
                    if (result.AnalysisName == inputTerm.AnalysisName)
                    {
                        switch (inputTerm.AnalysisTerm)
                        {
                            case "Low":
                            {
                                affiliations.Add(result.LowResult.Affiliation);
                                break;
                            }
                            case "Mid":
                            {
                                affiliations.Add(result.MidResult.Affiliation);
                                break;
                            }
                            case "High":
                            {
                                affiliations.Add(result.HighResult.Affiliation);
                                break;
                            }
                        }
                    }
                }
            }

            Power = 0;

            if (affiliations.Count > 0)
            {
                Power = affiliations.Min();
                affiliations.Clear();
            }
        }
    }
}
