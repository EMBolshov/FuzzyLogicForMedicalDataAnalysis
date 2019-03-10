namespace FuzzyLogicMedicalCore
{
    public interface ITerm
    {
        string Name { get; set; }
        decimal Affiliation { get; set; }

        decimal CalculateAffiliation(decimal currentValue);
    }
}
