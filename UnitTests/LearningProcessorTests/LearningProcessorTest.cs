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

            var analysisResults = CreateAnalysisResultsForJDA(patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(patient.Guid))
                .Returns(analysisResults);

            var diagnoses = CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);

            var rules = CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);

            var mockReportGenerator = new HtmlReportGenerator();

            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object, 
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);
            
            //Act
            var results = sut.ProcessForAllPatients();

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
            var patient = CreatePatient();
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });

            var analysisResults = CreateAnalysisResultsForAHZ(patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(patient.Guid))
                .Returns(analysisResults);

            var diagnoses = CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);

            var rules = CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);

            var mockReportGenerator = new HtmlReportGenerator();

            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object,
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);

            //Act
            var results = sut.ProcessForAllPatients();

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
            var patient = CreatePatient();
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });

            var analysisResults = CreateAnalysisResultsForFDA(patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(patient.Guid))
                .Returns(analysisResults);

            var diagnoses = CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);

            var rules = CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);

            var mockReportGenerator = new HtmlReportGenerator();

            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object,
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);

            //Act
            var results = sut.ProcessForAllPatients();

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
            var patient = CreatePatient();
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });

            var analysisResults = CreateAnalysisResultsForB12DA(patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(patient.Guid))
                .Returns(analysisResults);

            var diagnoses = CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);

            var rules = CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);

            var mockReportGenerator = new HtmlReportGenerator();

            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object,
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);

            //Act
            var results = sut.ProcessForAllPatients();

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
            var patient = CreatePatient();
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });

            var analysisResults = CreateAnalysisResultsWithNormalHgb(patient.Guid);
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
            //var mockReportGenerator = new HtmlReportGenerator();

            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object,
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);

            //Act
            var results = sut.ProcessForAllPatients();

            //Assert
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            Assert.IsTrue(results.All(x => x.Value == 0));
        }

        [TestMethod]
        public void ProcessForPatientTestWithLowHgbOnly()
        {
            //Arrange
            var patient = CreatePatient();
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });

            var analysisResults = CreateAnalysisResultsWithLowHgb(patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(patient.Guid))
                .Returns(analysisResults);

            var diagnoses = CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);

            var rules = CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);

            var mockReportGenerator = new HtmlReportGenerator();

            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object,
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);

            //Act
            var results = sut.ProcessForAllPatients();

            //Assert
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            Assert.IsTrue(results.All(x => x.Value > 0));
        }

        [TestMethod]
        public void ProcessForPatientTestExpectedJDAAndAHZ()
        {
            //Arrange
            var patient = CreatePatient();
            var mockPatientDbProvider = new Mock<IPatientProvider>();
            mockPatientDbProvider.Setup(x => x.GetAllPatients()).Returns(new List<Patient> { patient });

            var analysisResults = CreateAnalysisResultsForJDAAndAHZ(patient.Guid);
            var mockAnalysisResultDbProvider = new Mock<IAnalysisResultProvider>();
            mockAnalysisResultDbProvider.Setup(x => x.GetAnalysisResultsByPatientGuid(patient.Guid))
                .Returns(analysisResults);

            var diagnoses = CreateDiagnoses();
            var mockDiagnosisDbProvider = new Mock<IDiagnosisProvider>();
            mockDiagnosisDbProvider.Setup(x => x.GetAllDiagnoses()).Returns(diagnoses);

            var rules = CreateRules();
            var mockRuleDbProvider = new Mock<IRuleProvider>();
            mockRuleDbProvider.Setup(x => x.GetAllActiveRules()).Returns(rules);

            var mockReportGenerator = new HtmlReportGenerator();

            var sut = new LearningProcessor(mockAnalysisResultDbProvider.Object, mockDiagnosisDbProvider.Object,
                mockPatientDbProvider.Object, mockRuleDbProvider.Object, mockReportGenerator);

            //Act
            var results = sut.ProcessForAllPatients();

            //Assert
            Assert.IsTrue(results.Count > 0);
            results = results.OrderBy(x => x.Value).ToList();
            Assert.IsTrue(results.All(x => x.PatientGuid == patient.Guid));
            var guidJDA = diagnoses.First(x => x.Name == "Железодефицитная анемия").Guid;
            var guidAHZ = diagnoses.First(x => x.Name == "Анемия хронических заболеваний").Guid;
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidJDA && x.Value > 0));
            Assert.IsTrue(results.Any(x => x.DiagnosisGuid == guidAHZ && x.Value > 0));
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

        private List<AnalysisResult> CreateAnalysisResultsForJDA(Guid patientGuid)
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
                    Loinc = "1.01",
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
                    Loinc = "1.02",
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
                    Loinc = "1.03",
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
                    Loinc = "1.04",
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
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        private List<AnalysisResult> CreateAnalysisResultsForAHZ(Guid patientGuid)
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
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 12m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
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
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        private List<AnalysisResult> CreateAnalysisResultsForFDA(Guid patientGuid)
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
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 21m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
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
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        private List<AnalysisResult> CreateAnalysisResultsForB12DA(Guid patientGuid)
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
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 21m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.04",
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
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        private List<AnalysisResult> CreateAnalysisResultsWithNormalHgb(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 12m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
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
                    Loinc = "1.03",
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
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        private List<AnalysisResult> CreateAnalysisResultsWithLowHgb(Guid patientGuid)
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
                    Loinc = "1.01",
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
                },
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "3.1",
                    Name = "Фолиеводефицитная анемия"
                },
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "4.1",
                    Name = "B12-дефицитная анемия"
                }
            };
        }

        private List<AnalysisResult> CreateAnalysisResultsForJDAAndAHZ(Guid patientGuid)
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
                    Loinc = "1.01",
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
                    Loinc = "1.02",
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
                    Loinc = "1.04",
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
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        private List<Rule> CreateRules()
        {
            var result = new List<Rule>();
            result.AddRange(CreateRulesForJDA());
            result.AddRange(CreateRulesForAHZ());
            result.AddRange(CreateRulesForFDA());
            result.AddRange(CreateRulesForB12DA());
            return result;
        }

        private List<Rule> CreateRulesForJDA()
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

        private List<Rule> CreateRulesForAHZ()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
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

        private List<Rule> CreateRulesForFDA()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
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

        private List<Rule> CreateRulesForB12DA()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
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
