namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public interface ITerm
    {
        string Name { get; set; }
        decimal Affiliation { get; set; }

        void GetAffiliation();
    }
}
