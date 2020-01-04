namespace POCO.Domain
{
    public class FuzzyAnalysisResult
    {
        public string TestName { get; set; }
        public string InputTermName { get; set; }
        public decimal Confidence { get; set; }
    }
}
