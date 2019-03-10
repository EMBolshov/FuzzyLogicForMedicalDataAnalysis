namespace FuzzyLogicMedicalCore
{
    public class Diagnosis : ITerm
    {
        public string Name { get; set; }
        public decimal Affiliation { get; set; }

        public decimal CalculateAffiliation(decimal value)
        {
            //todo select formula for each term type
            var result = value;
            return result;
        }
    }
}
