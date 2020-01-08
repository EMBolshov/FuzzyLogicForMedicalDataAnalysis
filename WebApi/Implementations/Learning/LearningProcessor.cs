using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using POCO.Domain;
using POCO.Domain.Dto;
using WebApi.Implementations.Helpers;
using WebApi.Implementations.MainProcessing;
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
        private readonly IDiagnosisDecisionMaker _decisionMaker;
        private readonly List<string> _coreTests = new List<string>
        {
            "Гемоглобин (HGB)",
            "Железо в сыворотке",
            "Витамин В12",
            "Фолат сыворотки",
            "Ферритин"
        };

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
            _reportGenerator = reportGeneratorResolver("Html");
            _decisionMaker = new DiagnosisDecisionMaker(_learningAnalysisResultProvider, _learningDiagnosisProvider,
                _learningPatientProvider, _learningRuleProvider);
        }

        public LearningProcessor(IAnalysisResultProvider learningAnalysisResultProvider, IDiagnosisProvider learningDiagnosisProvider,
            IPatientProvider learningPatientProvider, IRuleProvider learningRuleProvider, IReportGenerator reportGenerator)
        {
            _learningAnalysisResultProvider = learningAnalysisResultProvider;
            _learningDiagnosisProvider = learningDiagnosisProvider;
            _learningPatientProvider = learningPatientProvider;
            _learningRuleProvider = learningRuleProvider;
            _reportGenerator = reportGenerator;
            _decisionMaker = new DiagnosisDecisionMaker(_learningAnalysisResultProvider, _learningDiagnosisProvider,
                _learningPatientProvider, _learningRuleProvider);
        }
        
        //TODO: нужно ли возвращать все результаты наверх, если тесты будут переписаны?
        public List<ProcessedResult> ProcessForAllPatients()
        {
            var patients = GetAllPatients();
            var results = new List<ProcessedResult>(); 
            //Поставить диагнозы по полным данным
            patients.ForEach(patient =>
            {
                var processedResults = _decisionMaker.ProcessForPatient(patient, true);
                results.AddRange(processedResults);

                if (processedResults.Any(x => x.Value > 0))
                {
                    //Результаты так же доставались в прыдыдущем шаге внутри ProcessForPatient. Передавать их внутрь не хочу,
                    //из-за того, что они потребуются только для обучения (оправдание избыточности обращения к бд).
                    var patientResults = _learningAnalysisResultProvider.GetAnalysisResultsByPatientGuid(patient.Guid);
                    
                    //Подготовка отчетов по результатам для целей тестирования
                    var reportModel = new ReportModel
                    {
                        ProcessedResults = processedResults,
                        Patient = patient,
                        AnalysisResults = patientResults,
                        Diagnoses = LearningDiagnoses.ToList(),
                        Path = "TestReports"
                    };

                    _reportGenerator.GenerateReport(reportModel);
                }
            });

            //TODO: Среди проставленных диагнозов собрать статистику по другим анализам этого пациента

            foreach (var diagnosis in LearningDiagnoses)
            {
                
            }

            //TODO: Подготовить новые правила с учетом статистики 

            //TODO: Подготовить статистический отчет
            //TODO: Загрузить новые правила в основную БД

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

        private List<CreateRuleDto> CreateDtoForBaseRules()
        {
            var result = new List<CreateRuleDto>();
            result.AddRange(CreateJDARules());
            result.AddRange(CreateAHZRules());
            result.AddRange(CreateFolDefAnemiaRules());
            result.AddRange(CreateB12DefAnemiaRules());

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

        private List<CreateRuleDto> CreateFolDefAnemiaRules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                }
            };
        }

        private List<CreateRuleDto> CreateB12DefAnemiaRules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                }
            };
        }
    }
}
