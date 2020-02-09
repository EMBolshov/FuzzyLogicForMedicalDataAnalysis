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
        private readonly IRuleProvider _learningRuleProvider;
        private readonly IFuzzyficator _fuzzyficator;

        //TODO: Cache
        private IEnumerable<Rule> LearningRules => _learningRuleProvider.GetAllActiveRules();
        private IEnumerable<Diagnosis> LearningDiagnoses => _diagnosisProvider.GetAllDiagnoses();
        
        public DiagnosisDecisionMaker(IAnalysisResultProvider analysisResultProvider, 
            IDiagnosisProvider diagnosisProvider, IRuleProvider ruleProvider)
        {
            _analysisResultProvider = analysisResultProvider;
            _diagnosisProvider = diagnosisProvider;
            _learningRuleProvider = ruleProvider;
            _fuzzyficator = new Fuzzyficator();
        }

        public List<ProcessedResult> ProcessForPatient(Patient patient, bool isOnlyFullData = false)
        {
            var processedResults = new List<ProcessedResult>();
            var allAnalysisResults = _analysisResultProvider.GetAnalysisResultsByPatientGuid(patient.Guid);
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
    }
}
