namespace FuzzyLogicMedicalCore
{
    public class MidResult : AbstractResult
    {
        public override decimal CalculateAffiliation(decimal value)
        {
            //todo select formula for each term type
            var result = value;
            return result;
        }
    }
}
