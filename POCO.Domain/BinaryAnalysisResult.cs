namespace POCO.Domain
{
    public class BinaryAnalysisResult
    {
        public string TestName { get; set; }
        public string InputTermName { get; set; }
        public int Confidence { get; set; }
    }
}
