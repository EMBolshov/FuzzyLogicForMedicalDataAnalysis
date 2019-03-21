namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public abstract class AbstractResult : ITerm
    {
        public string Name { get; set; }
        public decimal Affiliation { get; set; }

        //todo use intervals
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal CurrentValue { get; set; }

        public virtual decimal CalculateAffiliation()
        {
            //todo select formula for each term type
            //сейчас рассчитывается насколько число соответствует середине интервала
            var middleOfRange = (MaxValue + MinValue) / 2;
            var result = CurrentValue;
            return result;
        }
    }
}
