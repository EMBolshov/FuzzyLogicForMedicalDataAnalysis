using System;
using System.Collections.Generic;
using System.Linq;
using POCO.Domain;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.MainProcessing
{
    public class DiagnosisDecisionMaker : IDiagnosisDecisionMaker
    {
        private readonly IAnalysisResultProvider _analysisResultProvider;
        private readonly IDiagnosisProvider _diagnosisProvider;
        private readonly IRuleProvider _ruleProvider;
        private readonly IFuzzyficator _fuzzyficator;
        private readonly ITestAccuracyProvider _testAccuracyProvider;
        public List<TestAccuracy> TestAccuracies => _testAccuracyProvider.GetAllTestAccuracies();

        public DiagnosisDecisionMaker(IAnalysisResultProvider analysisResultProvider, 
            IDiagnosisProvider diagnosisProvider, IRuleProvider ruleProvider,
            ITestAccuracyProvider testAccuracyProvider)
        {
            _analysisResultProvider = analysisResultProvider;
            _diagnosisProvider = diagnosisProvider;
            _ruleProvider = ruleProvider;
            _testAccuracyProvider = testAccuracyProvider;
            _fuzzyficator = new Fuzzyficator();
        }

        public List<ProcessedResult> ProcessForPatient(Patient patient, bool isOnlyFullData = false)
        {
            var processedResults = new List<ProcessedResult>();
            var allAnalysisResults = _analysisResultProvider.GetAnalysisResultsByPatientGuid(patient.Guid);
            var allDiagnoses = _diagnosisProvider.GetAllDiagnoses();
            var allActiveRules = _ruleProvider.GetAllActiveRules();

            foreach (var diagnosis in allDiagnoses)
            {
                var rules = allActiveRules.Where(x => x.DiagnosisName == diagnosis.Name).ToList();
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

            var fuzzyResults = _fuzzyficator.FuzzyficateResults(analysisResults);
            var distinctRules = rules.GroupBy(x => x.Test).Select(x => x.First()).ToList();

            foreach (var rule in distinctRules)
            {
                if (rules.Count(x => x.Test == rule.Test) == 1)
                {
                    var anyResults = fuzzyResults.Any(x => x.TestName == rule.Test);
                    var currentRuleValue = 0m;
                    if (anyResults)
                    {
                        var testAccuracy = TestAccuracies
                            .FirstOrDefault(x => x.DiagnosisGuid == diagnosis.Guid && x.TestName == rule.Test);

                        var testAccuracyValue = testAccuracy?.OverallAccuracy ?? 1;

                        currentRuleValue = fuzzyResults
                                               .First(x => x.TestName == rule.Test && x.InputTermName == rule.InputTermName)
                                               .Confidence * rule.Power * testAccuracyValue;
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
                            var testAccuracy = TestAccuracies
                                .FirstOrDefault(x => x.DiagnosisGuid == diagnosis.Guid && x.TestName == rule.Test);

                            var testAccuracyValue = testAccuracy?.OverallAccuracy ?? 1;

                            anyResults = true;
                            currentRuleValue += fuzzyResults
                                                   .First(x => x.TestName == nonSingleRule.Test && x.InputTermName == nonSingleRule.InputTermName)
                                                   .Confidence * nonSingleRule.Power * testAccuracyValue;
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
    }
}
