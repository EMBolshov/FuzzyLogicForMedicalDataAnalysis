using System.Collections.Generic;
using System.Linq;
using FuzzyLogicMedicalCore.FHIR;
using FuzzyLogicMedicalCore.MedicalFuzzyDataModel;

namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public class Rule
    {
        public int Id { get; set; }
        public string InputTerms { get; set; } //хранение в базе через разделитель, конвертация в список
        public string OutputTerms { get; set; } //todo - где конвертим?
        public decimal Power { get; set; }

        public void GetPower(List<IAnalysisResult> results)
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
                            default:
                            {
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
