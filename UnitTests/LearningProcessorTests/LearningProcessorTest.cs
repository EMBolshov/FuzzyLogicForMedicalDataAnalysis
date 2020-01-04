using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POCO.Domain;
using WebApi.Implementations.Helpers;
using WebApi.Implementations.Learning;
using WebApi.Interfaces.MainProcessing;

namespace UnitTests.LearningProcessorTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LearningProcessorTest
    {
        [TestMethod]
        public void ProcessForPatientTestExpectedJDA()
        {
            //Arrange
            var patient = CreatePatient();
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });

            var analysisResults = CreateAnalysisResults(patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(patient.Guid))
                .Returns(analysisResults);

            var diagnoses = CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);

            var rules = CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);

            var mockReportGenerator = new TxtReportGenerator();
            
            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object, 
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);
            
            //Act
            var results = sut.ProcessForAllPatients();

            //Assert
            Assert.IsTrue(results.Count > 0);
            results = results.OrderBy(x => x.Value).ToList();
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == diagnoses.ElementAt(1).Guid));
            Assert.IsTrue(results.First(x => x.Value > 0).DiagnosisGuid == diagnoses.ElementAt(1).Guid);
        }

        private Patient CreatePatient()
        {
            return new Patient
            {
                Guid = Guid.NewGuid(),
                MiddleName = "MiddleName",
                FirstName = "FirstName",
                LastName = "LastName",
                Age = 23,
                Gender = "Male",
                InsertedDate = DateTime.Now,
                IsRemoved = false,
                Id = 1
            };
        }

        private List<AnalysisResult> CreateAnalysisResults(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    IsRemoved = false
                }
            };
        }

        private List<Diagnosis> CreateDiagnoses()
        {
            return new List<Diagnosis>
            {
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "1.1",
                    Name = "Анемия хронических заболеваний"
                },
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "2.1",
                    Name = "Железодефицитная анемия"
                }
            };
        }

        private List<Rule> CreateRules()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
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
    }
}
