namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public class HighResult : AbstractResult
    {
        public override decimal CalculateAffiliation()
        {
            //todo select formula for each term type
            var result = CurrentValue;
            return result;
        }
    }
}
