using System.Collections.Generic;
using System.Linq;
using FuzzyLogicMedicalCore.BL.MedicalFuzzyDataModel;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class Rule
    {
        public int Id { get; set; }
        public string InputTerms { get; set; } 
        public string OutputTerms { get; set; } 
        public decimal Power { get; set; }

        public void GetPower(List<AnalysisResult> results)
        {
            var inputTerms = InputTerms.Split(';').ToList();
            var affiliations = new List<decimal>();

            foreach (var inputTerm in inputTerms)
            {
                var inputTermName = inputTerm.Split(':').First();
                var inputTermValue = inputTerm.Split(':').ElementAt(1);

                foreach (var result in results)
                {
                    if (result.AnalysisName == inputTermName)
                    {
                        switch (inputTermValue)
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
