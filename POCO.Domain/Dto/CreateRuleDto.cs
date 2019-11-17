namespace POCO.Domain.Dto
{
    public class CreateRuleDto
    {
        public string DiagnosisName { get; set; }
        public string Analysis { get; set; }
        public decimal Power { get; set; }
        public string InputTermName { get; set; }
    }
}
