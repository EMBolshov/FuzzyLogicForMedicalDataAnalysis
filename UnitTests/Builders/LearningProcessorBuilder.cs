using System.Collections.Generic;
using System.Linq;
using Moq;
using POCO.Domain;
using WebApi.Implementations.Helpers;
using WebApi.Implementations.Learning;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.MainProcessing;

namespace UnitTests.Builders
{
    //TODO: Убрать все, что относится к DiagnosisDecisionMaker-у
    //TODO: Подготовить список пациентов, по которому будут формироваться новые правила.
    public class LearningProcessorBuilder
    {
        public List<Patient> Patients { get; private set; }
        public IPatientProvider PatientProvider { get; private set; }
        public IAnalysisResultProvider AnalysisResultProvider { get; private set; }
        public IDiagnosisProvider DiagnosisProvider { get; private set; }
        public IRuleProvider RuleProvider { get; private set; }
        public IProcessedResultProvider ProcessedResultProvider { get; private set; }
        public IReportGenerator ReportGenerator { get; private set; }

        private readonly StubObjectProvider _stubObjectProvider;

        public LearningProcessorBuilder()
        {
            _stubObjectProvider = new StubObjectProvider();
        }

        public LearningProcessorBuilder GetPatientProvider()
        {
            var patient = _stubObjectProvider.CreatePatient();
            Patients.Add(patient);
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });
            PatientProvider = mockPatientDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Железодефицитная анемия
        /// </summary>
        /// <returns></returns>
        public LearningProcessorBuilder GetJDAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForJDA(Patients.First().Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patients.First().Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Анемия хронических заболеваний
        /// </summary>
        /// <returns></returns>
        public LearningProcessorBuilder GetAHZAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForAHZ(Patients.First().Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patients.First().Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Фолиеводефицитная анемия
        /// </summary>
        /// <returns></returns>
        public LearningProcessorBuilder GetFDAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForFDA(Patients.First().Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patients.First().Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Б12 дефицитная анемия
        /// </summary>
        /// <returns></returns>
        public LearningProcessorBuilder GetB12DAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForB12DA(Patients.First().Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patients.First().Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Гемоглобин в норме
        /// </summary>
        /// <returns></returns>
        public LearningProcessorBuilder GetNormalHGBAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsWithNormalHgb(Patients.First().Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patients.First().Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Гемоглобин низкий, других результатов нет
        /// </summary>
        /// <returns></returns>
        public LearningProcessorBuilder GetLowHGBOnlyAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsWithLowHgb(Patients.First().Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patients.First().Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Анемия хронических заболеваний и железодефицитная анемия
        /// </summary>
        /// <returns></returns>
        public LearningProcessorBuilder GetAHZAndJDAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForJDAAndAHZ(Patients.First().Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patients.First().Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        public LearningProcessorBuilder GetDiagnosisProvider()
        {
            var diagnoses = _stubObjectProvider.CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);
            DiagnosisProvider = mockDiagnosisDbProvider.Object;
            return this;
        }

        public LearningProcessorBuilder GetRulesProvider()
        {
            var rules = _stubObjectProvider.CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);
            RuleProvider = mockRuleDbProvider.Object;
            return this;
        }

        public LearningProcessorBuilder GetProcessedResultProvider()
        {
            var mockProcessedResultsDbProvider = new Mock<IProcessedResultProvider>();
            ProcessedResultProvider = mockProcessedResultsDbProvider.Object;
            return this;
        }

        public LearningProcessorBuilder GetHtmlReportGenerator()
        {
            ReportGenerator = new HtmlReportGenerator();
            return this;
        }

        public LearningProcessor Build()
        {
            return new LearningProcessor(AnalysisResultProvider, DiagnosisProvider,
                PatientProvider, RuleProvider, ProcessedResultProvider, ReportGenerator);
        }
    }
}
