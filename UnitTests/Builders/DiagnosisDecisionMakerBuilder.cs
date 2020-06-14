
using System.Collections.Generic;
using Moq;
using POCO.Domain;
using WebApi.Implementations.Helpers;
using WebApi.Implementations.Learning;
using WebApi.Implementations.MainProcessing;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.MainProcessing;

namespace UnitTests.Builders
{
    public class DiagnosisDecisionMakerBuilder
    {
        public Patient Patient { get; private set; }
        public IPatientProvider PatientProvider { get; private set; }
        public IAnalysisResultProvider AnalysisResultProvider { get; private set; }
        public IDiagnosisProvider DiagnosisProvider { get; private set; }
        public IRuleProvider RuleProvider { get; private set; }
        public IProcessedResultProvider ProcessedResultProvider { get; private set; }
        public ITestAccuracyProvider TestAccuracyProvider { get; private set; }

        private readonly StubObjectProvider _stubObjectProvider;

        public DiagnosisDecisionMakerBuilder()
        {
            _stubObjectProvider = new StubObjectProvider();
        }

        public DiagnosisDecisionMakerBuilder GetPatientProvider()
        {
            var patient = _stubObjectProvider.CreatePatient();
            Patient = patient;
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });
            PatientProvider = mockPatientDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Железодефицитная анемия
        /// </summary>
        /// <returns></returns>
        public DiagnosisDecisionMakerBuilder GetJDAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForJDA(Patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patient.Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Анемия хронических заболеваний
        /// </summary>
        /// <returns></returns>
        public DiagnosisDecisionMakerBuilder GetAHZAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForAHZ(Patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patient.Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Фолиеводефицитная анемия
        /// </summary>
        /// <returns></returns>
        public DiagnosisDecisionMakerBuilder GetFDAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForFDA(Patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patient.Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Б12 дефицитная анемия
        /// </summary>
        /// <returns></returns>
        public DiagnosisDecisionMakerBuilder GetB12DAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForB12DA(Patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patient.Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Гемоглобин в норме
        /// </summary>
        /// <returns></returns>
        public DiagnosisDecisionMakerBuilder GetNormalHGBAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsWithNormalHgb(Patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patient.Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Гемоглобин низкий, других результатов нет
        /// </summary>
        /// <returns></returns>
        public DiagnosisDecisionMakerBuilder GetLowHGBOnlyAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsWithLowHgb(Patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patient.Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        /// <summary>
        /// Анемия хронических заболеваний и железодефицитная анемия
        /// </summary>
        /// <returns></returns>
        public DiagnosisDecisionMakerBuilder GetAHZAndJDAAnalysisResultProvider()
        {
            var analysisResults = _stubObjectProvider.CreateAnalysisResultsForJDAAndAHZ(Patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(Patient.Guid))
                .Returns(analysisResults);
            AnalysisResultProvider = mockAnalysisResultDbProvider.Object;
            return this;
        }

        public DiagnosisDecisionMakerBuilder GetDiagnosisProvider()
        {
            var diagnoses = _stubObjectProvider.CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);
            DiagnosisProvider = mockDiagnosisDbProvider.Object;
            return this;
        }

        public DiagnosisDecisionMakerBuilder GetRulesProvider()
        {
            var rules = _stubObjectProvider.CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);
            RuleProvider = mockRuleDbProvider.Object;
            return this;
        }

        public DiagnosisDecisionMakerBuilder GetProcessedResultProvider()
        {
            var mockProcessedResultsDbProvider = new Mock<IProcessedResultProvider>();
            ProcessedResultProvider = mockProcessedResultsDbProvider.Object;
            return this;
        }

        public DiagnosisDecisionMakerBuilder GetTestAccuracyProvider()
        {
            var mockTestAccuracyProvider = new Mock<ITestAccuracyProvider>();
            TestAccuracyProvider = mockTestAccuracyProvider.Object;
            return this;
        }
        
        public DiagnosisDecisionMaker Build()
        {
            return new DiagnosisDecisionMaker(AnalysisResultProvider, DiagnosisProvider, RuleProvider, TestAccuracyProvider);
        }
    }
}
