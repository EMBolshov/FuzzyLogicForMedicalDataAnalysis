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
            var affiliations = new Dictionary<string, decimal>();

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
                                if (!affiliations.ContainsKey(result.AnalysisName) ||
                                    affiliations.FirstOrDefault(x => x.Key == result.AnalysisName).Value <
                                    result.LowResult.Affiliation)
                                {
                                    affiliations[result.AnalysisName] = result.LowResult.Affiliation;
                                }
                                
                                break;
                            }
                            case "Mid":
                            {
                                if (!affiliations.ContainsKey(result.AnalysisName) ||
                                    affiliations.FirstOrDefault(x => x.Key == result.AnalysisName).Value <
                                    result.MidResult.Affiliation)
                                {
                                    affiliations[result.AnalysisName] = result.MidResult.Affiliation;
                                }

                                break;
                            }
                            case "High":
                            {
                                if (!affiliations.ContainsKey(result.AnalysisName) ||
                                    affiliations.FirstOrDefault(x => x.Key == result.AnalysisName).Value <
                                    result.HighResult.Affiliation)
                                {
                                    affiliations[result.AnalysisName] = result.HighResult.Affiliation;
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
        }
    }
}
