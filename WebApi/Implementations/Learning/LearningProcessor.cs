using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
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
        private readonly IRuleProvider _mainProcessingRuleProvider;
        private readonly IProcessedResultProvider _learningProcessedResultProvider;
        private readonly IReportGenerator _reportGenerator;
        private readonly IDiagnosisDecisionMaker _decisionMaker;
        private readonly IFuzzyficator _fuzzyficator;
        private readonly IEntitiesToCreateDtoMapper _createDtoMapper;
        private readonly List<string> _coreTests = new List<string>
        {
            "Гемоглобин (HGB)",
            "Железо в сыворотке",
            "Витамин В12",
            "Фолат сыворотки",
            "Ферритин"
        };
        
        private readonly List<string> _OAKTests = new List<string>
        {
            "Гемоглобин (HGB)",
            "Средний объем эритроцита (MCV)",
            "Эритроциты (RBC)",
            "Средн. сод. гемоглобина в эр-те (MCH)",
            "Гематокрит (HCT)"
        };
        
        private IEnumerable<Diagnosis> LearningDiagnoses => _learningDiagnosisProvider.GetAllDiagnoses();

        public LearningProcessor(IMainProcessingRepository mainRepo, ILearningRepository learnRepo)
        {
            _learningProcessedResultProvider = new LearningProcessedResultDbProvider(learnRepo);
            _learningAnalysisResultProvider = new AnalysisResultLearningDbProvider(learnRepo, new FileParser(new AnalysisAndTestsNamingMapper()));
            _learningDiagnosisProvider = new DiagnosisLearningDbProvider(learnRepo);
            _learningPatientProvider = new PatientLearningDbProvider(learnRepo);
            _learningRuleProvider = new RuleLearningDbProvider(learnRepo);
            _mainProcessingRuleProvider = new RuleDbProvider(mainRepo);

            _reportGenerator = new HtmlReportGenerator();
            _decisionMaker = new DiagnosisDecisionMaker(_learningAnalysisResultProvider, _learningDiagnosisProvider, _learningRuleProvider);
            _fuzzyficator = new Fuzzyficator();
            _createDtoMapper = new EntitiesToCreateDtoMapper();
        }

        public LearningProcessor(IAnalysisResultProvider learningAnalysisResultProvider, IDiagnosisProvider learningDiagnosisProvider,
            IPatientProvider learningPatientProvider, IRuleProvider learningRuleProvider, IRuleProvider mainProcessingRuleProvider,
            IProcessedResultProvider learningProcessedResultProvider, IReportGenerator reportGenerator)
        {
            _learningAnalysisResultProvider = learningAnalysisResultProvider;
            _learningDiagnosisProvider = learningDiagnosisProvider;
            _learningPatientProvider = learningPatientProvider;
            _learningRuleProvider = learningRuleProvider;
            _mainProcessingRuleProvider = mainProcessingRuleProvider;
            _learningProcessedResultProvider = learningProcessedResultProvider;
            _reportGenerator = reportGenerator;
            _decisionMaker = new DiagnosisDecisionMaker(_learningAnalysisResultProvider, 
                _learningDiagnosisProvider, _learningRuleProvider);
            _fuzzyficator = new Fuzzyficator();
            _createDtoMapper = new EntitiesToCreateDtoMapper();
        }
        
        //TODO: нужно ли возвращать все результаты наверх, если тесты будут переписаны?
        public List<ProcessedResult> ProcessForAllPatients()
        {
            //TODO: разбить на методы

            //Предварительная очистка всех правил на проде
            _mainProcessingRuleProvider.DeleteAllRules();
            //Предварительная очистка всех результатов на лерне
            _learningProcessedResultProvider.DeleteAllResults();

            var patients = GetAllPatients();
            var results = new List<ProcessedResult>(); 
            //Поставить диагнозы по полным данным
            patients.ForEach(MakeDiagnosisDecisionAndGenerateReports);

            var allPositiveResults = _learningProcessedResultProvider.GetAllPositiveResults();

            var statistics = GetPositiveResultsStatistics(allPositiveResults);

            //TODO: Подготовить новые правила с учетом статистики 

            var newRules = MakeNewRulesBasedOnStatistics(statistics);

            //TODO: Подготовить статистический отчет

            var rules = _learningRuleProvider.GetAllActiveRules();
            rules.ForEach(x =>
            {
                var ruleDto = _createDtoMapper.RuleToCreateRuleDto(x);
                _mainProcessingRuleProvider.CreateRule(ruleDto);
            });

            foreach (var newRule in newRules)
            {
                var ruleDto = _createDtoMapper.RuleToCreateRuleDto(newRule);
                _mainProcessingRuleProvider.CreateRule(ruleDto);
            }

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

        public decimal GetErrorRatio()
        {
            //Поставить диагнозы по полному набору данных, убрать из результатов пациентов специфические тесты,
            //поставить диагнозы по неполному набору данных, сравнить количество положительных результатов

            _learningProcessedResultProvider.DeleteAllResults();

            var patients = _learningPatientProvider.GetAllPatients();
            var fullDataResults = new List<ProcessedResult>();
            var partialDataResults = new List<ProcessedResult>();
            var removedAnalysisResultsGuids = new List<Guid>();

            //Сбор статистики для подготовки новых правил
            foreach (var patient in patients)
            {
                var fullRes = _decisionMaker.ProcessForPatient(patient, true);
                if (fullRes.Any(x => x.Value > 0))
                {
                    fullRes.Where(x => x.Value > 0).ToList()
                        .ForEach(x => _learningProcessedResultProvider.SaveProcessedResult(x));
                }
            }

            //Добавить новые правила
            var allPositiveResults = _learningProcessedResultProvider.GetAllPositiveResults();
            var statistics = GetPositiveResultsStatistics(allPositiveResults);
            var newRules = MakeNewRulesBasedOnStatistics(statistics);
            foreach (var newRule in newRules)
            {
                var ruleDto = _createDtoMapper.RuleToCreateRuleDto(newRule);
                _learningRuleProvider.CreateRule(ruleDto);
            }

            //Полный набор данных
            foreach (var patient in patients)
            {
                var fullRes = _decisionMaker.ProcessForPatient(patient, true);
                if (fullRes.Any(x => x.Value > 0))
                {
                    fullRes.Where(x => x.Value > 0).ToList()
                        .ForEach(x => _learningProcessedResultProvider.SaveProcessedResult(x));

                    fullDataResults.AddRange(fullRes.Where(x => x.Value > 0));
                }
            }

            //Только ОАК
            foreach (var patient in patients)
            {
                var patientResults = _learningAnalysisResultProvider.GetAnalysisResultsByPatientGuid(patient.Guid);
                patientResults.ForEach(x =>
                {
                    if (!_OAKTests.Contains(x.TestName))
                    {
                        _learningAnalysisResultProvider.RemoveAnalysisResult(x.Guid);
                        removedAnalysisResultsGuids.Add(x.Guid);
                    }
                });

                var partRes = _decisionMaker.ProcessForPatient(patient, false);
                if (partRes.Any(x => x.Value > 0))
                {
                    partialDataResults.AddRange(partRes.Where(x => x.Value > 0));
                }
            }

            //Сравнение полного набора с неполным
            var maxCount = fullDataResults.Count;
            var filteredPartRes = new List<ProcessedResult>();

            partialDataResults.ForEach(x =>
            {
                if (fullDataResults.Any(y => y.PatientGuid == x.PatientGuid && y.DiagnosisGuid == x.DiagnosisGuid))
                {
                    filteredPartRes.Add(x);
                }
            });

            var difference = filteredPartRes.Where(x => x.Value == 0).ToList();
            
            removedAnalysisResultsGuids.ForEach(x => _learningAnalysisResultProvider.ReturnAnalysisResult(x));
            _learningRuleProvider.DeleteAllRules();
            CreateBaseRules();

            return difference.Count / maxCount * 100;
        }

        private void MakeDiagnosisDecisionAndGenerateReports(Patient patient)
        {
            var processedResults = _decisionMaker.ProcessForPatient(patient, true);
            //TODO: переписать тесты! Результаты возвращает только decisionMaker, тесты LearningProcessor-а должны быть
            //TODO: полностью другие
            //results.AddRange(processedResults);

            if (processedResults.Any(x => x.Value > 0))
            {
                processedResults.Where(x => x.Value > 0).ToList()
                    .ForEach(x => _learningProcessedResultProvider.SaveProcessedResult(x));

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
        }

        private Dictionary<Diagnosis, List<FuzzyAnalysisResult>> GetPositiveResultsStatistics(List<ProcessedResult> results)
        {
            var statistic = new Dictionary<Diagnosis, List<FuzzyAnalysisResult>>();
            //TODO: Среди проставленных диагнозов собрать статистику по другим тестам
            foreach (var diagnosis in LearningDiagnoses)
            {
                var fuzzyResults = new List<FuzzyAnalysisResult>();
                var sickPatientGuids = results.Where(x => x.DiagnosisGuid == diagnosis.Guid)
                    .Select(x => x.PatientGuid).ToList();
                
                foreach (var patientGuid in sickPatientGuids)
                {
                    var patientResults = _learningAnalysisResultProvider.GetAnalysisResultsByPatientGuid(patientGuid);
                    var nonCoreResults = patientResults.Where(x => !_coreTests.Contains(x.TestName)).ToList();

                    //TODO: Фаззификация результатов и занесение их в Dictionary
                    //(добавляем новые тесты или обновляем счетчик у старых)
                    fuzzyResults.AddRange(_fuzzyficator.FuzzyficateResults(nonCoreResults));
                }

                if (!statistic.ContainsKey(diagnosis))
                {
                    statistic.Add(diagnosis, fuzzyResults);
                }
            }

            return statistic;
        }

        private List<Rule> MakeNewRulesBasedOnStatistics(Dictionary<Diagnosis, List<FuzzyAnalysisResult>> statistics)
        {
            var result = new List<Rule>();

            foreach (var diagnosis in statistics.Keys)
            {
                var analysisResults = statistics[diagnosis];

                var tests = analysisResults.Select(x => x.TestName).Distinct().ToList();
                var terms = analysisResults.Select(x => x.InputTermName).Distinct().ToList();

                foreach (var test in tests)
                {
                    foreach (var term in terms)
                    {
                        var testResultByTerm = analysisResults
                            .Where(x => x.InputTermName == term && x.TestName == test).ToList();
                        decimal positiveResultsCount = testResultByTerm.Count(x => x.Confidence > 0);
                        var power = Math.Round(positiveResultsCount / testResultByTerm.Count, 4);

                        if (power > 0)
                        {
                            result.Add(CreateRule(diagnosis.Name, term, power, test));
                        }
                    }
                }
            }

            return result;
        }

        private Rule CreateRule(string diagnosis, string term, decimal power, string test)
        {
            return new Rule
            {
                Guid = Guid.NewGuid(),
                DiagnosisName = diagnosis,
                InputTermName = term,
                Power = power,
                IsRemoved = false,
                Test = test
            };
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
