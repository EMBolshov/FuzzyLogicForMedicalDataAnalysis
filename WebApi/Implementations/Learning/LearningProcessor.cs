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
        private readonly ITestAccuracyProvider _mainTestAccuracyProvider;
        private readonly ITestAccuracyProvider _learningTestAccuracyProvider;
        private readonly string _normalDataSet = "full_ds.csv";
        private readonly string _negativeDataSet = "full_negative.csv";
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
            _learningTestAccuracyProvider = new TestAccuracyLearningDbProvider(learnRepo);
            _mainTestAccuracyProvider = new TestAccuracyDbProvider(mainRepo);

            _reportGenerator = new HtmlReportGenerator();
            _decisionMaker = new DiagnosisDecisionMaker(_learningAnalysisResultProvider, _learningDiagnosisProvider, _learningRuleProvider, _learningTestAccuracyProvider);
            _fuzzyficator = new Fuzzyficator();
            _createDtoMapper = new EntitiesToCreateDtoMapper();
        }

        public LearningProcessor(IAnalysisResultProvider learningAnalysisResultProvider, IDiagnosisProvider learningDiagnosisProvider,
            IPatientProvider learningPatientProvider, IRuleProvider learningRuleProvider, IRuleProvider mainProcessingRuleProvider,
            IProcessedResultProvider learningProcessedResultProvider, IReportGenerator reportGenerator, 
            ITestAccuracyProvider learningTestAccuracyProvider, ITestAccuracyProvider mainTestAccuracyProvider)
        {
            _learningAnalysisResultProvider = learningAnalysisResultProvider;
            _learningDiagnosisProvider = learningDiagnosisProvider;
            _learningPatientProvider = learningPatientProvider;
            _learningRuleProvider = learningRuleProvider;
            _mainProcessingRuleProvider = mainProcessingRuleProvider;
            _learningProcessedResultProvider = learningProcessedResultProvider;
            _learningTestAccuracyProvider = learningTestAccuracyProvider;
            _mainTestAccuracyProvider = mainTestAccuracyProvider;
            _reportGenerator = reportGenerator;
            _decisionMaker = new DiagnosisDecisionMaker(_learningAnalysisResultProvider, 
                _learningDiagnosisProvider, _learningRuleProvider, _learningTestAccuracyProvider);
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

        public void CreateInverseRules()
        {
            var dtoForRules = CreateDtoForInverseRules();
            dtoForRules.ForEach(x => _learningRuleProvider.CreateRule(x));
        }

        public decimal GetErrorRatio()
        {
            //Поставить диагнозы по полному набору данных, убрать из результатов пациентов специфические тесты,
            //поставить диагнозы по неполному набору данных, сравнить наиболее вероятный диагноз по неполным данным с поставленным по полным

            _learningProcessedResultProvider.DeleteAllResults();
            _learningRuleProvider.DeleteAllRules();
            CreateBaseRules();

            var patients = _learningPatientProvider.GetAllPatients();
            var fullDataResults = new List<ProcessedResult>();
            var partialDataResults = new List<ProcessedResult>();
            var removedAnalysisResultsGuids = new List<Guid>();

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

            //Добавить новые правила
            var allPositiveResults = _learningProcessedResultProvider.GetAllPositiveResults();
            var statistics = GetPositiveResultsStatistics(allPositiveResults);
            var newRules = MakeNewRulesBasedOnStatistics(statistics);
            foreach (var newRule in newRules)
            {
                var ruleDto = _createDtoMapper.RuleToCreateRuleDto(newRule);
                _learningRuleProvider.CreateRule(ruleDto);
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

            var patientsWithPositiveResultByFullData =
                patients.Where(x => fullDataResults.Any(y => y.PatientGuid == x.Guid)).ToList();

            var diffCount = 0;

            foreach (var patient in patientsWithPositiveResultByFullData)
            {
                var diagnosisGuid = fullDataResults.First(x => x.PatientGuid == patient.Guid).DiagnosisGuid;
                var diagnosis = LearningDiagnoses.FirstOrDefault(x => x.Guid == diagnosisGuid)?.Name;

                var allPartialDiagnosis = partialDataResults.Where(x => x.PatientGuid == patient.Guid)
                    .ToDictionary(x => LearningDiagnoses.First(y => y.Guid == x.DiagnosisGuid).Name, x => x.Value);

                var maxValue = allPartialDiagnosis.Values.Max();
                var mostProbableDiagnoses = allPartialDiagnosis.Where(x => x.Value >= maxValue).Select(x => x.Key).ToList();
                
                //Наимболее вероятный диагноз не равен поставленному - ошибка
                //if (!mostProbableDiagnoses.Contains(diagnosis))
                //{
                //    diffCount++;
                //}

                //Разница между наиболее вероятным диагнозом и поставленным диагнозов больше 90% - ошибка
                var diagnosisChance = allPartialDiagnosis[diagnosis];
                var mostProbableDiagnosisChance = allPartialDiagnosis[mostProbableDiagnoses.First()];
                if (diagnosisChance / mostProbableDiagnosisChance < 0.9m)
                {
                    diffCount++;
                }
            }

            removedAnalysisResultsGuids.ForEach(x => _learningAnalysisResultProvider.ReturnAnalysisResult(x));
            _learningRuleProvider.DeleteAllRules();
            CreateBaseRules();
            
            var result = diffCount / (decimal)patientsWithPositiveResultByFullData.Count * 100m;
            return result;
        }

        public void CalculateTestAccuracy(bool clearAccuracies = true)
        {
            ReCreateStandardRulesAndLoadResults();
            if (clearAccuracies)
            {
                _learningTestAccuracyProvider.DeleteAllTestAccuracies();
            }

            //Сначала по обычному набору правил и результатов
            //Проставить диагнозы по полному набору данных
            var patients = _learningPatientProvider.GetAllPatients();
            var fullDataResults = new List<ProcessedResult>();

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

            //Разделить показатели на две категории - ниже ReferenceLow и выше
            //Подсчитать коэфы (А, Б) => А = кол-во ниже RefLow, Б = 1 -А 

            var allPositiveResults = _learningProcessedResultProvider.GetAllPositiveResults();
            var statistics = GetPositiveResultsBinaryStatistics(allPositiveResults);

            List<TestAccuracy> previousTestAccuracies = null;

            if (!clearAccuracies)
            {
                previousTestAccuracies = _learningTestAccuracyProvider.GetAllTestAccuracies();
            }

            var testAccuracies = CalculateTrulyTestAccuracies(statistics, previousTestAccuracies);
            
            //Загрузить инверсный набор правил и второй комплект результатов
            ReCreateInverseRulesAndLoadResults();

            //Проставить диагнозы по полному набору данных

            patients = _learningPatientProvider.GetAllPatients();
            fullDataResults = new List<ProcessedResult>();

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


            //Разделить показатели на две категории
            //Подсчитать коэфы (В, Г)

            allPositiveResults = _learningProcessedResultProvider.GetAllPositiveResults();
            statistics = GetPositiveResultsBinaryStatistics(allPositiveResults);

            var testAccuraciesWithFalsely = CalculateFalselyTestAccuracies(statistics, testAccuracies);

            //По каждому диагнозу для каждого показателя посчитать OТ => (А + Г) / (А + Б + В + Г)

            if (!clearAccuracies)
            {
                _learningTestAccuracyProvider.DeleteAllTestAccuracies();
            }

            var completeTestAccuracies = CalculateOverallTestAccuracies(statistics, testAccuraciesWithFalsely);
            foreach (var testAccuracy in completeTestAccuracies)
            {
                _learningTestAccuracyProvider.CreateTestAccuracy(testAccuracy);
            }

            ReCreateStandardRulesAndLoadResults();
        }

        private void ReCreateStandardRulesAndLoadResults()
        {
            ClearDb();
            CreateBaseRules();

            var analysisResults = _learningAnalysisResultProvider.LoadAnalysisResultsFromFile(_normalDataSet);
            foreach (var analysisResult in analysisResults)
            {
                var dto = _createDtoMapper.AnalysisResultToDto(analysisResult);
                _learningAnalysisResultProvider.CreateNewAnalysisResult(dto);
            }

            var patients = _learningAnalysisResultProvider.LoadPatientsFromFile(_normalDataSet);
            foreach (var patient in patients)
            {
                var dto = _createDtoMapper.PatientToCreatePatientDto(patient);
                _learningPatientProvider.CreateNewPatient(dto);
            }
        }

        private void ReCreateInverseRulesAndLoadResults()
        {
            ClearDb();

            CreateInverseRules();

            var analysisResults = _learningAnalysisResultProvider.LoadAnalysisResultsFromFile(_negativeDataSet);
            foreach (var analysisResult in analysisResults)
            {
                var dto = _createDtoMapper.AnalysisResultToDto(analysisResult);
                _learningAnalysisResultProvider.CreateNewAnalysisResult(dto);
            }

            var patients = _learningAnalysisResultProvider.LoadPatientsFromFile(_negativeDataSet);
            foreach (var patient in patients)
            {
                var dto = _createDtoMapper.PatientToCreatePatientDto(patient);
                _learningPatientProvider.CreateNewPatient(dto);
            }
        }

        private void ClearDb()
        {
            _learningProcessedResultProvider.DeleteAllResults();
            _learningRuleProvider.DeleteAllRules();
            _learningPatientProvider.DeleteAllPatients();
            _learningAnalysisResultProvider.DeleteAllAnalysisResults();
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

        private Dictionary<Diagnosis, List<BinaryAnalysisResult>> GetPositiveResultsBinaryStatistics(List<ProcessedResult> results)
        {
            var statistic = new Dictionary<Diagnosis, List<BinaryAnalysisResult>>();
            
            foreach (var diagnosis in LearningDiagnoses)
            {
                var binaryResults = new List<BinaryAnalysisResult>();
                var sickPatientGuids = results.Where(x => x.DiagnosisGuid == diagnosis.Guid)
                    .Select(x => x.PatientGuid).ToList();

                foreach (var patientGuid in sickPatientGuids)
                {
                    var patientResults = _learningAnalysisResultProvider.GetAnalysisResultsByPatientGuid(patientGuid);
                    //var nonCoreResults = patientResults.Where(x => !_coreTests.Contains(x.TestName)).ToList();

                    //(добавляем новые тесты или обновляем счетчик у старых)
                    //binaryResults.AddRange(_fuzzyficator.MakeBinaryResults(nonCoreResults));
                    binaryResults.AddRange(_fuzzyficator.MakeBinaryResults(patientResults));
                }

                statistic.Add(diagnosis, binaryResults);
            }

            return statistic;
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

        private List<TestAccuracy> CalculateTrulyTestAccuracies(Dictionary<Diagnosis, List<BinaryAnalysisResult>> statistics, List<TestAccuracy> testAccuracies = null)
        {
            var result = new List<TestAccuracy>();
            if (testAccuracies != null)
            {
                result = testAccuracies;
            }

            foreach (var diagnosis in statistics.Keys)
            {
                var analysisResults = statistics[diagnosis];

                var tests = analysisResults.Select(x => x.TestName).Distinct().ToList();
                var terms = analysisResults.Select(x => x.InputTermName).Distinct().ToList();

                if (testAccuracies == null)
                {
                    tests.ForEach(x => result.Add(CreateEmptyTestAccuracy(diagnosis.Guid, x)));
                }

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
                            var testAccuracy = result.FirstOrDefault(x => x.DiagnosisGuid == diagnosis.Guid && x.TestName == test);
                            if (testAccuracy != null)
                            {
                                if (term == "Low")
                                {
                                    result.Remove(testAccuracy);
                                    testAccuracy.TrulyPositive = power;
                                    result.Add(testAccuracy);
                                }
                                if (term == "Normal")
                                {
                                    result.Remove(testAccuracy);
                                    testAccuracy.TrulyNegative = power;
                                    result.Add(testAccuracy);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private List<TestAccuracy> CalculateFalselyTestAccuracies(Dictionary<Diagnosis, List<BinaryAnalysisResult>> statistics, List<TestAccuracy> testAccuracies)
        {
            var result = testAccuracies;

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
                            var testAccuracy = result.FirstOrDefault(x => x.DiagnosisGuid == diagnosis.Guid && x.TestName == test);
                            if (testAccuracy != null)
                            {
                                if (term == "Low")
                                {
                                    result.Remove(testAccuracy);
                                    testAccuracy.FalselyPositive = power;
                                    result.Add(testAccuracy);
                                }
                                if (term == "Normal")
                                {
                                    result.Remove(testAccuracy);
                                    testAccuracy.FalselyNegative = power;
                                    result.Add(testAccuracy);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private List<TestAccuracy> CalculateOverallTestAccuracies(Dictionary<Diagnosis, List<BinaryAnalysisResult>> statistics, List<TestAccuracy> testAccuracies)
        {
            var result = testAccuracies;

            foreach (var diagnosis in statistics.Keys)
            {
                var analysisResults = statistics[diagnosis];
                var tests = analysisResults.Select(x => x.TestName).Distinct().ToList();

                foreach (var test in tests)
                {
                    var testAccuracy = result.FirstOrDefault(x => x.DiagnosisGuid == diagnosis.Guid && x.TestName == test);

                    if (testAccuracy != null)
                    {
                        result.Remove(testAccuracy);
                        testAccuracy.OverallAccuracy = (testAccuracy.TrulyPositive + testAccuracy.FalselyPositive) /
                                                       (testAccuracy.TrulyPositive + testAccuracy.TrulyNegative +
                                                        testAccuracy.FalselyPositive + testAccuracy.FalselyNegative);
                        result.Add(testAccuracy);
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

        private TestAccuracy CreateEmptyTestAccuracy(Guid diagnosisGuid, string test)
        {
            return new TestAccuracy
            {
                Guid = Guid.NewGuid(),
                IsRemoved = false,
                InsertedDate = DateTime.Now,
                TestName = test,
                DiagnosisGuid = diagnosisGuid,
                TrulyPositive = 0,
                TrulyNegative = 0,
                FalselyPositive = 0,
                FalselyNegative = 0,
                OverallAccuracy = 0
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

        private List<CreateRuleDto> CreateDtoForInverseRules()
        {
            var result = new List<CreateRuleDto>();
            result.AddRange(CreateInverseJDARules());
            result.AddRange(CreateInverseAHZRules());
            result.AddRange(CreateInverseFolDefAnemiaRules());
            result.AddRange(CreateInverseB12DefAnemiaRules());

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

        private List<CreateRuleDto> CreateInverseJDARules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                }
            };
        }

        private List<CreateRuleDto> CreateInverseAHZRules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                }
            };
        }

        private List<CreateRuleDto> CreateInverseFolDefAnemiaRules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                }
            };
        }

        private List<CreateRuleDto> CreateInverseB12DefAnemiaRules()
        {
            return new List<CreateRuleDto>
            {
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },

                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new CreateRuleDto
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                }
            };
        }
    }
}
