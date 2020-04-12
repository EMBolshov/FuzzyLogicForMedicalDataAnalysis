using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (analysisResult.Entry >= analysisResult.ReferenceLow)
            {
                return 0m;
            }

            var delta = ((analysisResult.ReferenceHigh - analysisResult.ReferenceLow) * 0.05m / 0.95m) / 2m;

            if (analysisResult.Entry <= analysisResult.ReferenceLow - delta)
            {
                return 1m;
            }

            var firstPoint = (analysisResult.ReferenceLow - delta, 1m);
            var secondPoint = (analysisResult.ReferenceLow, 0m);

            var k = (secondPoint.Item2 - firstPoint.Item2) / (secondPoint.Item1 - firstPoint.Item1);
            var b = firstPoint.Item2 - (firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2) 
                                        / (secondPoint.Item1 - firstPoint.Item1));

            var affiliation = k * analysisResult.Entry + b;

            if (affiliation < 0m) { affiliation = 0m; }
            if (affiliation > 1m) { affiliation = 1m; }

            return Math.Round(affiliation, 4);
        }

        //TODO: Fuzzyfication
        private decimal GetNormalResultConfidence(AnalysisResult analysisResult)
        {
            if (analysisResult.Entry >= analysisResult.ReferenceLow
                && analysisResult.Entry <= analysisResult.ReferenceHigh)
            {
                return 1m;
            }

            var delta = ((analysisResult.ReferenceHigh - analysisResult.ReferenceLow) * 0.05m / 0.95m) / 2m;
            var minValue = analysisResult.ReferenceLow - delta;
            var maxValue = analysisResult.ReferenceHigh + delta;

            if (analysisResult.Entry <= minValue || analysisResult.Entry >= maxValue)
            {
                return 0m;
            }

            if (analysisResult.Entry > minValue && analysisResult.Entry < analysisResult.ReferenceLow)
            {
                var firstPoint = (analysisResult.ReferenceHigh, 0m);
                var secondPoint = (maxValue, 100m);

                var k = (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);
                var b = firstPoint.Item2 - firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);

                var affiliation = k * analysisResult.Entry + b;

                if (affiliation < 0m) { affiliation = 0m; }
                if (affiliation > 1m) { affiliation = 1m; }

                return Math.Round(affiliation, 4);
            }

            if (analysisResult.Entry > analysisResult.ReferenceHigh && analysisResult.Entry < maxValue)
            {
                var firstPoint = (analysisResult.ReferenceLow - delta, 1m);
                var secondPoint = (analysisResult.ReferenceLow, 0m);

                var k = (secondPoint.Item2 - firstPoint.Item2) / (secondPoint.Item1 - firstPoint.Item1);
                var b = firstPoint.Item2 - (firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                                            / (secondPoint.Item1 - firstPoint.Item1));

                var affiliation = k * analysisResult.Entry + b;

                if (affiliation < 0m) { affiliation = 0m; }
                if (affiliation > 1m) { affiliation = 1m; }

                return Math.Round(affiliation, 4);
            }

            throw new ArgumentOutOfRangeException(analysisResult.Entry.ToString(CultureInfo.CurrentCulture),
                $"Entry of analysisResult with GUID {analysisResult.Guid} is {analysisResult.Entry} and it is out of range");
        }

        //TODO: Fuzzyfication
        private decimal GetHighResultConfidence(AnalysisResult analysisResult)
        {
            if (analysisResult.Entry < analysisResult.ReferenceHigh)
            {
                return 0m;
            }

            var delta = ((analysisResult.ReferenceHigh - analysisResult.ReferenceLow) * 0.05m / 0.95m) / 2m;
            var maxValue = analysisResult.ReferenceHigh + delta;

            if (analysisResult.Entry > maxValue)
            {
                return 1m;
            }

            var firstPoint = (analysisResult.ReferenceHigh, 0m);
            var secondPoint = (maxValue, 100m);

            var k = (secondPoint.Item2 - firstPoint.Item2)
                    / (secondPoint.Item1 - firstPoint.Item1);
            var b = firstPoint.Item2 - firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                    / (secondPoint.Item1 - firstPoint.Item1);

            var affiliation = k * analysisResult.Entry + b;
            
            if (affiliation < 0m) { affiliation = 0m; }
            if (affiliation > 1m) { affiliation = 1m; }

            return Math.Round(affiliation, 4);
        }
    }
}
