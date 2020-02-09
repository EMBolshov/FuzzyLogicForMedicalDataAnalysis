using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Builders;

namespace UnitTests.DiagnosisDecisionMakerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DiagnosisDecisionMakerPartialDataTest
    {
        private readonly StubObjectProvider _stubObjectProvider = new StubObjectProvider();
        private readonly DiagnosisDecisionMakerBuilder _builder = new DiagnosisDecisionMakerBuilder();

        [TestMethod]
        public void ProcessForPatientTestExpectedJDA()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetJDAAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .Build();

            var patient = _builder.Patient;
            var diagnoses = _builder.DiagnosisProvider.GetAllDiagnoses();

            //Act
            var results = sut.ProcessForPatient(patient);

            //Assert
            Assert.IsTrue(results.Count > 0);
            results = results.OrderBy(x => x.Value).ToList();
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            var guidJDA = diagnoses.First(x => x.Name == "Железодефицитная анемия").Guid;
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidJDA && x.Value > 0));
        }

        [TestMethod]
        public void ProcessForPatientTestExpectedAHZ()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetAHZAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .Build();

            var patient = _builder.Patient;
            var diagnoses = _builder.DiagnosisProvider.GetAllDiagnoses();

            //Act
            var results = sut.ProcessForPatient(patient);

            //Assert
            Assert.IsTrue(results.Count > 0);
            results = results.OrderBy(x => x.Value).ToList();
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            var guidAHZ = diagnoses.First(x => x.Name == "Анемия хронических заболеваний").Guid;
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidAHZ && x.Value > 0));
        }

        [TestMethod]
        public void ProcessForPatientTestExpectedFDA()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetFDAAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .Build();

            var patient = _builder.Patient;
            var diagnoses = _builder.DiagnosisProvider.GetAllDiagnoses();

            //Act
            var results = sut.ProcessForPatient(patient);

            //Assert
            Assert.IsTrue(results.Count > 0);
            results = results.OrderBy(x => x.Value).ToList();
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            var guidFDA = diagnoses.First(x => x.Name == "Фолиеводефицитная анемия").Guid;
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidFDA && x.Value > 0));
        }

        [TestMethod]
        public void ProcessForPatientTestExpectedB12DA()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetB12DAAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .Build();

            var patient = _builder.Patient;
            var diagnoses = _builder.DiagnosisProvider.GetAllDiagnoses();

            //Act
            var results = sut.ProcessForPatient(patient);

            //Assert
            Assert.IsTrue(results.Count > 0);
            results = results.OrderBy(x => x.Value).ToList();
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            var guidFDA = diagnoses.First(x => x.Name == "B12-дефицитная анемия").Guid;
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidFDA && x.Value > 0));
        }

        [TestMethod]
        public void ProcessForPatientTestExpectedHealthy()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetNormalHGBAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .Build();

            var patient = _builder.Patient;
            
            //Act
            var results = sut.ProcessForPatient(patient);

            //Assert
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            Assert.IsTrue(results.All(x => x.Value == 0));
        }

        [TestMethod]
        public void ProcessForPatientTestWithLowHgbOnly()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetLowHGBOnlyAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .Build();

            var patient = _builder.Patient;
            
            //Act
            var results = sut.ProcessForPatient(patient);

            //Assert
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            Assert.IsTrue(results.All(x => x.Value > 0));
        }

        [TestMethod]
        public void ProcessForPatientTestExpectedJDAAndAHZ()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetAHZAndJDAAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .Build();

            var patient = _builder.Patient;
            var diagnoses = _builder.DiagnosisProvider.GetAllDiagnoses();

            //Act
            var results = sut.ProcessForPatient(patient);

            //Assert
            Assert.IsTrue(results.Count > 0);
            results = results.OrderBy(x => x.Value).ToList();
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            var guidJDA = diagnoses.First(x => x.Name == "Железодефицитная анемия").Guid;
            var guidAHZ = diagnoses.First(x => x.Name == "Анемия хронических заболеваний").Guid;
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidJDA && x.Value > 0));
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidAHZ && x.Value > 0));
        }
    }
}
