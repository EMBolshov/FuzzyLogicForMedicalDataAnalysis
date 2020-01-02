namespace WebApi.Interfaces
{
    public interface INamingMapper
    {
        string MapTestNameByAnalysisName(string analysisName);
        string ChangeTestName(string testName);
        string MapTestNameWithLoinc(string testName);
    }
}
