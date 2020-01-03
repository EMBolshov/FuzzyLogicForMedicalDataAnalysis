namespace WebApi.Interfaces.Helpers
{
    public interface INamingMapper
    {
        string MapTestNameByAnalysisName(string analysisName);
        string ChangeTestName(string testName);
        string MapTestNameWithLoinc(string testName);
    }
}
