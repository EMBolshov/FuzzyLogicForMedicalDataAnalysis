using FuzzyLogicMedicalCore.FuzzyLogic;

namespace FuzzyLogicMedicalCore.MedicalFuzzyDataModel
{
    public class Diagnosis : ITerm
    {
        public string Name { get; set; }
        public decimal Affiliation { get; set; }

        public decimal CalculateAffiliation()
        {
            //todo select formula for each term type
            var result = 123;
            return result;
        }
    }
}
