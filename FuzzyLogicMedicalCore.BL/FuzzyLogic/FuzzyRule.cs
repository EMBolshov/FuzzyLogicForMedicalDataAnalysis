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
            var affiliations = new Dictionary<AnalysisResult, decimal>();

            foreach (var result in results)
            {
                affiliations.Add(result, 0m);
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
                                    if (result.IsKey && result.LowResult.Affiliation > 0)
                                    {
                                        affiliations[result] += 100;
                                    }
                                    else if (result.LowResult.Affiliation > 0)
                                    {
                                        affiliations[result] += result.LowResult.Affiliation;
                                    }
                                }
                                
                                break;
                            }
                            case "Mid":
                            {
                                {
                                    if (result.IsKey && result.MidResult.Affiliation > 0)
                                    {
                                        affiliations[result] += 100;
                                    }
                                    else if(result.MidResult.Affiliation > 0)
                                    {
                                        affiliations[result] += result.MidResult.Affiliation;
                                    }
                                }

                                break;
                            }
                            case "High":
                            {
                                {
                                    if (result.IsKey && result.HighResult.Affiliation > 0)
                                    {
                                        affiliations[result] += 100;
                                    }
                                    else if (result.HighResult.Affiliation > 0)
                                    {
                                        affiliations[result] += result.HighResult.Affiliation;
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }

            Power = 0;


            var positiveAffiliations = affiliations.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
            if (positiveAffiliations.Count > 0)
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
