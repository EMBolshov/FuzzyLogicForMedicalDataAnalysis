using System;
using System.Collections.Generic;
using System.Linq;
using POCO.Domain;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.MainProcessing
{
    public class DiagnosisDecisionMaker : IDiagnosisDecisionMaker
    {
        private readonly IAnalysisResultProvider _learningAnalysisResultProvider;
        private readonly IDiagnosisProvider _learningDiagnosisProvider;
        private readonly IPatientProvider _learningPatientProvider;
        private readonly IRuleProvider _learningRuleProvider;

        //TODO: Cache
        private IEnumerable<Rule> LearningRules => _learningRuleProvider.GetAllActiveRules();
        private IEnumerable<Diagnosis> LearningDiagnoses => _learningDiagnosisProvider.GetAllDiagnoses();
        
        public DiagnosisDecisionMaker(IAnalysisResultProvider learningAnalysisResultProvider, IDiagnosisProvider learningDiagnosisProvider, IPatientProvider learningPatientProvider, IRuleProvider learningRuleProvider)
        {
            _learningAnalysisResultProvider = learningAnalysisResultProvider;
            _learningDiagnosisProvider = learningDiagnosisProvider;
            _learningPatientProvider = learningPatientProvider;
            _learningRuleProvider = learningRuleProvider;
        }

        //TODO: Тесты
        public List<ProcessedResult> ProcessForPatient(Patient patient, bool isOnlyFullData = false)
        {
            var processedResults = new List<ProcessedResult>();
            var allAnalysisResults = _learningAnalysisResultProvider.GetAnalysisResultsByPatientGuid(patient.Guid);
            foreach (var diagnosis in LearningDiagnoses)
            {
                var rules = LearningRules.Where(x => x.DiagnosisName == diagnosis.Name).ToList();
                var testsToProcess = rules.Select(x => x.Test).ToList();
                var analysisResults = allAnalysisResults.Where(x => testsToProcess.Contains(x.TestName)).ToList();

                if (analysisResults.Any())
                {
                    processedResults.Add(GetProcessedResultValue(analysisResults, diagnosis, rules, isOnlyFullData));
                }
            }

            return processedResults;
        }

        //TODO: Тесты, мб рефакторинг (пока я не придумал как)
        private ProcessedResult GetProcessedResultValue(List<AnalysisResult> analysisResults, Diagnosis diagnosis, List<Rule> rules, bool isOnlyFullData = false)
        {
            var value = 0m;
            var countedTests = 0;

            var fuzzyResults = FuzzyficateResults(analysisResults);

            var distinctRules = rules.GroupBy(x => x.Test).Select(x => x.First()).ToList();

            foreach (var rule in distinctRules)
            {
                if (rules.Count(x => x.Test == rule.Test) == 1)
                {
                    var anyResults = fuzzyResults.Any(x => x.TestName == rule.Test);
                    var currentRuleValue = 0m;
                    if (anyResults)
                    {
                        currentRuleValue = fuzzyResults
                                               .First(x => x.TestName == rule.Test && x.InputTermName == rule.InputTermName)
                                               .Confidence * rule.Power;
                    }

                    if (currentRuleValue > 0)
                    {
                        value += currentRuleValue;
                        countedTests++;
                    }
                    else if (anyResults || isOnlyFullData)
                    {
                        return new ProcessedResult
                        {
                            Guid = Guid.NewGuid(),
                            InsertedDate = DateTime.Now,
                            DiagnosisGuid = diagnosis.Guid,
                            PatientGuid = analysisResults.First().PatientGuid,
                            Value = 0m
                        };
                    }
                }
                else
                {
                    var nonSingleRules = rules.Where(x => x.Test == rule.Test).ToList();
                    var currentRuleValue = 0m;
                    var anyResults = false;
                    foreach (var nonSingleRule in nonSingleRules)
                    {
                        var anySingleResult = fuzzyResults.Any(x => x.TestName == nonSingleRule.Test);
                        if (anySingleResult)
                        {
                            anyResults = true;
                            currentRuleValue += fuzzyResults
                                                   .First(x => x.TestName == nonSingleRule.Test && x.InputTermName == nonSingleRule.InputTermName)
                                                   .Confidence * nonSingleRule.Power;
                        }
                    }

                    if (currentRuleValue > 0)
                    {
                        if (currentRuleValue > 1)
                        {
                            currentRuleValue = 1;
                        }

                        value += currentRuleValue;
                        countedTests++;
                    }
                    else if (anyResults || isOnlyFullData)
                    {
                        return new ProcessedResult
                        {
                            Guid = Guid.NewGuid(),
                            InsertedDate = DateTime.Now,
                            DiagnosisGuid = diagnosis.Guid,
                            PatientGuid = analysisResults.First().PatientGuid,
                            Value = 0m
                        };
                    }
                }
            }

            var result = new ProcessedResult
            {
                Guid = Guid.NewGuid(),
                InsertedDate = DateTime.Now,
                DiagnosisGuid = diagnosis.Guid,
                PatientGuid = analysisResults.First().PatientGuid,
                Value = value / countedTests
            };

            return result;
        }

        private List<FuzzyAnalysisResult> FuzzyficateResults(List<AnalysisResult> analysisResults)
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
