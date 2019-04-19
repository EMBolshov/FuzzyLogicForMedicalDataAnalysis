namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public abstract class AbstractResult : ITerm
    {
        public string Name { get; set; }
        public decimal Affiliation { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal CurrentValue { get; set; }

        public virtual void GetAffiliation()
        {
            
        }
    }
}
