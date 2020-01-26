using System.Collections.Generic;
using POCO.Domain;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.MainProcessing
{
    public class Fuzzyficator : IFuzzyficator
    {
        public List<FuzzyAnalysisResult> FuzzyficateResults(List<AnalysisResult> analysisResults)
        {
            var fuzzyResults = new List<FuzzyAnalysisResult>();

            foreach (var analysisResult in analysisResults)
            {
                fuzzyResults.Add(new FuzzyAnalysisResult
                {
                    TestName = analysisResult.TestName,
                    InputTermName = "Low",
                    Confidence = GetLowResultConfidence(analysisResult)
                });

                fuzzyResults.Add(new FuzzyAnalysisResult
                {
                    TestName = analysisResult.TestName,
                    InputTermName = "Normal",
                    Confidence = GetNormalResultConfidence(analysisResult)
                });

                fuzzyResults.Add(new FuzzyAnalysisResult
                {
                    TestName = analysisResult.TestName,
                    InputTermName = "High",
                    Confidence = GetHighResultConfidence(analysisResult)
                });
            }

            return fuzzyResults;
        }

        //TODO: Fuzzyfication
        private decimal GetLowResultConfidence(AnalysisResult analysisResult)
        {
            if (analysisResult.Entry < analysisResult.ReferenceLow)
            {
                return 1m;
            }

            return 0m;
        }

        //TODO: Fuzzyfication
        private decimal GetNormalResultConfidence(AnalysisResult analysisResult)
        {
            if (analysisResult.Entry >= analysisResult.ReferenceLow
                && analysisResult.Entry <= analysisResult.ReferenceHigh)
            {
                return 1m;
            }

            return 0m;
        }

        //TODO: Fuzzyfication
        private decimal GetHighResultConfidence(AnalysisResult analysisResult)
        {
            if (analysisResult.Entry > analysisResult.ReferenceHigh)
            {
                return 1m;
            }

            return 0m;
        }
    }
}
