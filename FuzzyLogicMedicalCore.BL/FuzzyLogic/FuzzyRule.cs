using System.Collections.Generic;
using System.Linq;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class FuzzyRule
    {
        public int Id { get; set; }
        public List<InputTerm> InputTerms { get; set; } 
        public List<string> OutputTerms { get; set; } 
        public decimal Power { get; set; }

        public void GetPower(List<AnalysisResult> results)
        {
            var affiliations = new Dictionary<string, decimal>();

            foreach (var result in results)
            {
                affiliations.Add(result.AnalysisName, 0m);
            }

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
                                {
                                    affiliations[result.AnalysisName] += result.LowResult.Affiliation;
                                }
                                
                                break;
                            }
                            case "Mid":
                            {
                                {
                                    affiliations[result.AnalysisName] += result.MidResult.Affiliation;
                                }

                                break;
                            }
                            case "High":
                            {
                                {
                                    affiliations[result.AnalysisName] += result.HighResult.Affiliation;
                                }

                                break;
                            }
                        }
                    }
                }
            }

            Power = 0;

            if (affiliations.Count > 0)
            {
                Power = affiliations.Values.Min();
                affiliations.Clear();
            }
            
            if (Power > 100)
            {
                Power = 100;
            }
        }
    }
}
