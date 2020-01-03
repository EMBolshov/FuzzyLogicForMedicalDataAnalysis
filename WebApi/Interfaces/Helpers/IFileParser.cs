using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Interfaces.Helpers
{
    public interface IFileParser
    {
        List<AnalysisResult> GetAnalysisResultsFromCsv(string path);
        List<Patient> GetPatientsFromCsv(string path);
    }
}
