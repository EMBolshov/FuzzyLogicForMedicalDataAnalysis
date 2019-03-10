namespace FuzzyLogicMedicalCore
{
    public class LowResult : AbstractResult
    {
        public override decimal CalculateAffiliation(decimal value)
        {
            //todo select formula for each term type
            var result = value;
            return result;
        }
    }
}
