using System;
using System.Collections.Generic;
using System.Linq;
using POCO.Domain;
using POCO.Domain.Dto;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.Learning;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
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
                var analysisToProcess = rules.Select(x => x.Test).ToList();
                var analysisResults = allAnalysisResults.Where(x => analysisToProcess.Contains(x.TestName)).ToList();

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

                fuzzyResults = fuzzyResults.Where(x => x.Confidence > 0m).ToList();

                foreach (var fuzzyResult in fuzzyResults)
                {
                    foreach (var rule in LearningRules)
                    {
                        if (fuzzyResult.TestName == rule.Test)
                        {
                            //For learning data all core rules power must be 1, non-core rules power must be 0
                            fuzzyResult.Confidence *= rule.Power;
                        }
                    }
                }

                if (fuzzyResults.Count > 0 && fuzzyResults.All(x => x.Confidence > 0))
                {
                    processedResults.Add(new ProcessedResult
                    {
                        PatientGuid = patient.Guid,
                        DiagnosisGuid = diagnosis.Guid,
                        Value = fuzzyResults.Select(x => x.Confidence).Average()
                    });
                }
            }

            foreach (var processedResult in processedResults)
            {
                _reportGenerator.GenerateReport(processedResult, patient,
                    allAnalysisResults, LearningDiagnoses.ToList(), "TestReports");
            }

            return processedResults;
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
