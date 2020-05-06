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

        public List<BinaryAnalysisResult> MakeBinaryResults(List<AnalysisResult> analysisResults)
        {
            var binaryResults = new List<BinaryAnalysisResult>();

            foreach (var analysisResult in analysisResults)
            {
                binaryResults.Add(new BinaryAnalysisResult()
                {
                    TestName = analysisResult.TestName,
                    InputTermName = "Low",
                    Confidence = GetBinaryLowResultConfidence(analysisResult)
                });

                binaryResults.Add(new BinaryAnalysisResult()
                {
                    TestName = analysisResult.TestName,
                    InputTermName = "Normal",
                    Confidence = GetBinaryNormalResultConfidence(analysisResult)
                });
            }

            return binaryResults;
        }

        private decimal GetLowResultConfidence(AnalysisResult analysisResult)
        {
            var delta = ((analysisResult.ReferenceHigh - analysisResult.ReferenceLow) * 0.05m / 0.95m) / 2m;

            if (analysisResult.Entry >= analysisResult.ReferenceLow + delta)
            {
                return 0m;
            }

            if (analysisResult.Entry <= analysisResult.ReferenceLow - delta)
            {
                return 1m;
            }

            var firstPoint = (analysisResult.ReferenceLow - delta, 1m);
            var secondPoint = (analysisResult.ReferenceLow + delta, 0m);

            var k = (secondPoint.Item2 - firstPoint.Item2) / (secondPoint.Item1 - firstPoint.Item1);
            var b = firstPoint.Item2 - (firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2) 
                                        / (secondPoint.Item1 - firstPoint.Item1));

            var affiliation = k * analysisResult.Entry + b;

            if (affiliation < 0m) { affiliation = 0m; }
            if (affiliation > 1m) { affiliation = 1m; }

            return Math.Round(affiliation, 4);
        }

        private decimal GetNormalResultConfidence(AnalysisResult analysisResult)
        {
            var delta = ((analysisResult.ReferenceHigh - analysisResult.ReferenceLow) * 0.05m / 0.95m) / 2m;
            var minValue = analysisResult.ReferenceLow - delta;
            var maxValue = analysisResult.ReferenceHigh + delta;
            
            if (analysisResult.Entry >= analysisResult.ReferenceLow + delta
                && analysisResult.Entry <= analysisResult.ReferenceHigh - delta)
            {
                return 1m;
            }
            
            if (analysisResult.Entry <= minValue || analysisResult.Entry >= maxValue)
            {
                return 0m;
            }

            if (analysisResult.Entry > minValue && analysisResult.Entry < analysisResult.ReferenceLow + delta)
            {
                var firstPoint = (minValue, 0m);
                var secondPoint = (analysisResult.ReferenceLow + delta, 100m);

                var k = (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);
                var b = firstPoint.Item2 - firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);

                var affiliation = k * analysisResult.Entry + b;

                if (affiliation < 0m) { affiliation = 0m; }
                if (affiliation > 1m) { affiliation = 1m; }

                return Math.Round(affiliation, 4);
            }

            if (analysisResult.Entry > analysisResult.ReferenceHigh - delta && analysisResult.Entry < maxValue)
            {
                var firstPoint = (analysisResult.ReferenceHigh - delta, 1m);
                var secondPoint = (maxValue, 0m);

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

        private decimal GetHighResultConfidence(AnalysisResult analysisResult)
        {
            var delta = ((analysisResult.ReferenceHigh - analysisResult.ReferenceLow) * 0.05m / 0.95m) / 2m;
            var maxValue = analysisResult.ReferenceHigh + delta;

            if (analysisResult.Entry < analysisResult.ReferenceHigh - delta)
            {
                return 0m;
            }

            if (analysisResult.Entry > maxValue)
            {
                return 1m;
            }

            var firstPoint = (analysisResult.ReferenceHigh - delta, 0m);
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

        private int GetBinaryLowResultConfidence(AnalysisResult analysisResult)
        {
            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
        }

        private int GetBinaryNormalResultConfidence(AnalysisResult analysisResult)
        {
            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
        }
    }
}
