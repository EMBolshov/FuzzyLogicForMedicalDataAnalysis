using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using POCO.Domain;
using POCO.Domain.Dto;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.Learning;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class LearningProcessor : ILearningProcessor
    {
        private readonly IAnalysisResultProvider _learningAnalysisResultProvider;
        private readonly IDiagnosisProvider _learningDiagnosisProvider;
        private readonly IPatientProvider _learningPatientProvider;
        private readonly IRuleProvider _learningRuleProvider;
        private readonly IReportGenerator _reportGenerator;

        private IEnumerable<Rule> LearningRules => _learningRuleProvider.GetAllActiveRules();
        private IEnumerable<Diagnosis> LearningDiagnoses => _learningDiagnosisProvider.GetAllDiagnoses();

        public LearningProcessor(Startup.RuleServiceResolver ruleServiceResolver, 
            Startup.DiagnosisServiceResolver diagnosisServiceResolver, 
            Startup.AnalysisResultServiceResolver analysisResultServiceResolver, 
            Startup.PatientServiceResolver patientServiceResolver,
            Startup.ReportGeneratorResolver reportGeneratorResolver)
        {
            _learningAnalysisResultProvider = analysisResultServiceResolver("Learning");
            _learningDiagnosisProvider = diagnosisServiceResolver("Learning");
            _learningPatientProvider = patientServiceResolver("Learning");
            _learningRuleProvider = ruleServiceResolver("Learning");
            _reportGenerator = reportGeneratorResolver("Txt");
        }

        public LearningProcessor(IAnalysisResultProvider learningAnalysisResultProvider, IDiagnosisProvider learningDiagnosisProvider,
            IPatientProvider learningPatientProvider, IRuleProvider learningRuleProvider, IReportGenerator reportGenerator)
        {
            _learningAnalysisResultProvider = learningAnalysisResultProvider;
            _learningDiagnosisProvider = learningDiagnosisProvider;
            _learningPatientProvider = learningPatientProvider;
            _learningRuleProvider = learningRuleProvider;
            _reportGenerator = reportGenerator;
        }
        
        public List<ProcessedResult> ProcessForAllPatients()
        {
            var patients = GetAllPatients();
            var results = new List<ProcessedResult>(); 
            patients.ForEach(patient => results.AddRange(ProcessForPatient(patient)));
            return results;
        }

        public void CreateRule(CreateRuleDto dto)
        {
            _learningRuleProvider.CreateRule(dto);
        }

        public void CreateNewDiagnosis(CreateDiagnosisDto dto)
        {
            _learningDiagnosisProvider.CreateNewDiagnosis(dto);
        }

        public void CreateBaseRules()
        {
            var dtoForRules = CreateDtoForBaseRules();
            dtoForRules.ForEach(x => _learningRuleProvider.CreateRule(x));
        }

        private List<Patient> GetAllPatients()
        {
            return _learningPatientProvider.GetAllPatients();
        }

        //TODO: refactor
        private List<ProcessedResult> ProcessForPatient(Patient patient)
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
                    processedResults.Add(GetProcessedResultValue(analysisResults, diagnosis, rules));
                }
            }

            foreach (var processedResult in processedResults)
            {
                _reportGenerator.GenerateReport(processedResult, patient,
                    allAnalysisResults, LearningDiagnoses.ToList(), "TestReports");
            }

            return processedResults;
        }

        private ProcessedResult GetProcessedResultValue(List<AnalysisResult> analysisResults, Diagnosis diagnosis, List<Rule> rules)
        {
            var value = 0m;
            var countedTests = 0;

            var fuzzyResults = FuzzyficateResults(analysisResults);

            var distinctRules = rules.GroupBy(x => x.Test).Select(x => x.First()).ToList();

            foreach (var rule in distinctRules)
            {
                if (rules.Count(x => x.Test == rule.Test) == 1)
                {
                    var currentRuleValue = 0m;
                    if (fuzzyResults.Any(x => x.TestName == rule.Test))
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
                    else
                    {
                        return new ProcessedResult
                        {
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
                    foreach (var nonSingleRule in nonSingleRules)
                    {
                        if (fuzzyResults.Any(x => x.TestName == nonSingleRule.Test))
                        {
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
                    else
                    {
                        return new ProcessedResult
                        {
                            DiagnosisGuid = diagnosis.Guid,
                            PatientGuid = analysisResults.First().PatientGuid,
                            Value = 0m
                        };
                    }
                }
            }

            var result = new ProcessedResult
            {
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

        private List<CreateRuleDto> CreateDtoForBaseRules()
        {
            var result = new List<CreateRuleDto>();
            result.AddRange(CreateJDARules());
            result.AddRange(CreateAHZRules());

            return result;
        }

        private List<CreateRuleDto> CreateJDARules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                }
            };
        }

        private List<CreateRuleDto> CreateAHZRules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                }
            };
        }
    }
}
