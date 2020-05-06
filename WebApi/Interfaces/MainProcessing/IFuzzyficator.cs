using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Interfaces.MainProcessing
{
    public interface IFuzzyficator
    {
        List<FuzzyAnalysisResult> FuzzyficateResults(List<AnalysisResult> analysisResults);
        List<BinaryAnalysisResult> MakeBinaryResults(List<AnalysisResult> analysisResults);
    }
}
