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

        public List<BinaryAnalysisResult> MakeBinaryResults(List<AnalysisResult> analysisResults, string diagnosis)
        {
            var binaryResults = new List<BinaryAnalysisResult>();

            foreach (var analysisResult in analysisResults)
            {
                binaryResults.Add(new BinaryAnalysisResult
                {
                    TestName = analysisResult.TestName,
                    InputTermName = "Low",
                    Confidence = GetBinaryLowResultConfidence(analysisResult, diagnosis)
                });

                binaryResults.Add(new BinaryAnalysisResult
                {
                    TestName = analysisResult.TestName,
                    InputTermName = "Normal",
                    Confidence = GetBinaryNormalResultConfidence(analysisResult, diagnosis)
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

            //return Math.Round(affiliation, 4);
            return affiliation;
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

                //return Math.Round(affiliation, 4);
                return affiliation;
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

                //return Math.Round(affiliation, 4);
                return affiliation;
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

            //return Math.Round(affiliation, 4);
            return affiliation;
        }

        //TODO: сделать по-нормальному вместо этого хардкода тут
        private int GetBinaryLowResultConfidence(AnalysisResult analysisResult, string diagnosis)
        {
            //Исходя из конкретного диагноза и конкретного показателя
            switch (diagnosis)
            {
                case "Железодефицитная анемия":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Витамин В12":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Железо в сыворотке":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Tрансферрин":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 1 : 0;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Насыщение трансферрина":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Ферритин":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                    }
                    break;
                case "Анемия хронических заболеваний":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry <  analysisResult.ReferenceHigh ? 1 : 0;
                        case "Витамин В12":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Железо в сыворотке":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry < analysisResult.ReferenceHigh ? 1 : 0;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Tрансферрин":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 1 : 0;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Насыщение трансферрина":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Ферритин":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                    }
                    break;
                case "Фолиеводефицитная анемия":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 1 : 0;
                        case "Витамин В12":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Железо в сыворотке":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 1 : 0;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        //по трансферрину инфы не было
                        case "Tрансферрин":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        //инфы не было
                        case "Насыщение трансферрина":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Ферритин":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                    }
                    break;
                case "B12-дефицитная анемия":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 1 : 0;
                        case "Витамин В12":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Железо в сыворотке":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 1 : 0;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        //no info
                        case "Tрансферрин":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        //no info
                        case "Насыщение трансферрина":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 1 : 0;
                        case "Ферритин":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 1 : 0;
                    }
                    break;
            }

            throw new ArgumentOutOfRangeException();
        }

        private int GetBinaryNormalResultConfidence(AnalysisResult analysisResult, string diagnosis)
        {
            //Исходя из конкретного диагноза и конкретного показателя
            switch (diagnosis)
            {
                case "Железодефицитная анемия":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Витамин В12":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Железо в сыворотке":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Tрансферрин":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 0 : 1;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Насыщение трансферрина":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Ферритин":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                    }
                    break;
                case "Анемия хронических заболеваний":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry < analysisResult.ReferenceHigh ? 0 : 1;
                        case "Витамин В12":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Железо в сыворотке":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry < analysisResult.ReferenceHigh ? 0 : 1;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Tрансферрин":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 0 : 1;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Насыщение трансферрина":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Ферритин":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                    }
                    break;
                case "Фолиеводефицитная анемия":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 0 : 1;
                        case "Витамин В12":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Железо в сыворотке":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 0 : 1;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        //по трансферрину инфы не было
                        case "Tрансферрин":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        //инфы не было
                        case "Насыщение трансферрина":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Ферритин":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                    }
                    break;
                case "B12-дефицитная анемия":
                    switch (analysisResult.TestName)
                    {
                        case "Фолат сыворотки":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Средний объем эритроцита (MCV)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 0 : 1;
                        case "Витамин В12":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Железо в сыворотке":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                        case "Эритроциты (RBC)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Средн. сод. гемоглобина в эр-те (MCH)":
                            return analysisResult.Entry >= analysisResult.ReferenceHigh ? 0 : 1;
                        case "Гематокрит (HCT)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        //no info
                        case "Tрансферрин":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Гемоглобин (HGB)":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        //no info
                        case "Насыщение трансферрина":
                            return analysisResult.Entry <= analysisResult.ReferenceLow ? 0 : 1;
                        case "Ферритин":
                            return analysisResult.Entry > analysisResult.ReferenceLow ? 0 : 1;
                    }
                    break;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
