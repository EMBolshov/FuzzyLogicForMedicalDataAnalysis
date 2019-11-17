using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyLogicMedicalCore.BL.FHIR;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;
using FuzzyLogicMedicalCore.BL.ReportGeneration;
using FuzzyLogicTestingConsole.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzyLogicMedicalCore.BL.Test
{
    [TestClass]
    public class IntegrationTestsForPartialAnalysisResults
    {
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void Woman64yoAHZ()
        {
            //Arrange
            var patient = CreatePatient(64, "Female");
            var resultList = new List<AnalysisResult>()
            {
                CreateAnalysisResult(patient.Guid, "Гемоглобин (HGB)", 117m, 160m, 105m),
                CreateAnalysisResult(patient.Guid, "Железо в сыворотке", 6.6m, 26m, 6.7m),
                CreateAnalysisResult(patient.Guid, "Ферритин", 10m, 120m, 90m)
            };
            var rules = CreateRules();
            var diagnoses = CreateDiagnoses();
            var reportGenerator = new ReportGenerator(
                "C:\\Users\\ПК\\source\\repos\\FuzzyLogicForMedicalDataAnalysis\\" +
                $"TestResults\\IntegrationTestPartialResults_AHZ_patient_{patient.Guid}.txt");

            //Act
            diagnoses.ForEach(x => x.PatientGuid = patient.Guid);
            foreach (var result in resultList)
            {
                result.LowResult.GetAffiliation();
                result.MidResult.GetAffiliation();
                result.HighResult.GetAffiliation();
            }

            foreach (var rule in rules)
            {
                rule.GetPower(resultList);
            }

            foreach (var rule in rules)
            {
                foreach (var diagnosis in diagnoses)
                {
                    foreach (var outputTerm in rule.OutputTerms)
                    {
                        if (diagnosis.Name == outputTerm)
                        {
                            diagnosis.Rules.Add(rule);
                        }
                    }
                }
            }

            foreach (var diagnosis in diagnoses)
            {
                diagnosis.GetAffiliation();
            }


            reportGenerator.GenerateReport(patient, resultList, diagnoses, true);

            //Assert
            var isSick = diagnoses.Any(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isSick);
            // ReSharper disable once InconsistentNaming
            var isAHZ = diagnoses.Where(diagnosis => diagnosis.Name == "Анемия хронических заболеваний (АХЗ)")
                                 .FirstOrDefault(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isAHZ?.Affiliation > 0);
        }

        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void Woman72yoB12()
        {
            //Arrange
            var patient = CreatePatient(72, "Female");
            var resultList = new List<AnalysisResult>()
            {
                CreateAnalysisResult(patient.Guid, "Гемоглобин (HGB)", 117m, 161m, 98m),
                CreateAnalysisResult(patient.Guid, "Витамин B12", 191m, 663m, 101.6m),
                CreateAnalysisResult(patient.Guid, "Фолат сыворотки", 7m, 39.7m, 7.5m)
            };
            var rules = CreateRules();
            var diagnoses = CreateDiagnoses();
            var reportGenerator = new ReportGenerator(
                "C:\\Users\\ПК\\source\\repos\\FuzzyLogicForMedicalDataAnalysis\\" +
                $"TestResults\\IntegrationTestPartialResults_B12_patient_{patient.Guid}.txt");

            //Act
            diagnoses.ForEach(x => x.PatientGuid = patient.Guid);
            foreach (var result in resultList)
            {
                result.LowResult.GetAffiliation();
                result.MidResult.GetAffiliation();
                result.HighResult.GetAffiliation();
            }

            foreach (var rule in rules)
            {
                rule.GetPower(resultList);
            }

            foreach (var rule in rules)
            {
                foreach (var diagnosis in diagnoses)
                {
                    foreach (var outputTerm in rule.OutputTerms)
                    {
                        if (diagnosis.Name == outputTerm)
                        {
                            diagnosis.Rules.Add(rule);
                        }
                    }
                }
            }

            foreach (var diagnosis in diagnoses)
            {
                diagnosis.GetAffiliation();
            }


            reportGenerator.GenerateReport(patient, resultList, diagnoses, true);

            //Assert
            var isSick = diagnoses.Any(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isSick);

            var isB12 = diagnoses.Where(diagnosis => diagnosis.Name == "B12-дефицитная анемия")
                                 .FirstOrDefault(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isB12?.Affiliation > 0);
        }

        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void Man40yoJDA()
        {
            //Arrange
            var patient = CreatePatient(40, "Male");
            var resultList = new List<AnalysisResult>()
            {
                CreateAnalysisResult(patient.Guid, "Гемоглобин (HGB)", 132m, 173m, 110m),
                CreateAnalysisResult(patient.Guid, "Железо в сыворотке", 11m, 28m, 7.1m),
                CreateAnalysisResult(patient.Guid, "Ферритин", 20m, 250m, 15m)
            };
            var rules = CreateRules();
            var diagnoses = CreateDiagnoses();
            var reportGenerator = new ReportGenerator(
                "C:\\Users\\ПК\\source\\repos\\FuzzyLogicForMedicalDataAnalysis\\" +
                $"TestResults\\IntegrationTestPartialResults_JDA_patient_{patient.Guid}.txt");

            //Act
            diagnoses.ForEach(x => x.PatientGuid = patient.Guid);
            foreach (var result in resultList)
            {
                result.LowResult.GetAffiliation();
                result.MidResult.GetAffiliation();
                result.HighResult.GetAffiliation();
            }

            foreach (var rule in rules)
            {
                rule.GetPower(resultList);
            }

            foreach (var rule in rules)
            {
                foreach (var diagnosis in diagnoses)
                {
                    foreach (var outputTerm in rule.OutputTerms)
                    {
                        if (diagnosis.Name == outputTerm)
                        {
                            diagnosis.Rules.Add(rule);
                        }
                    }
                }
            }

            foreach (var diagnosis in diagnoses)
            {
                diagnosis.GetAffiliation();
            }


            reportGenerator.GenerateReport(patient, resultList, diagnoses, true);

            //Assert
            var isSick = diagnoses.Any(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isSick);

            var isB12 = diagnoses.Where(diagnosis => diagnosis.Name == "Железодефицитная анемия (ЖДА)")
                                 .FirstOrDefault(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isB12?.Affiliation > 0);
        }

        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void Man42yoFol()
        {
            //Arrange
            var patient = CreatePatient(42, "Male");
            var resultList = new List<AnalysisResult>()
            {
                CreateAnalysisResult(patient.Guid, "Гемоглобин (HGB)", 132m, 173m, 103m),
                CreateAnalysisResult(patient.Guid, "Витамин B12", 191m, 663m, 314.8m),
                CreateAnalysisResult(patient.Guid, "Фолат сыворотки", 7m, 39.7m, 4m)
            };
            var rules = CreateRules();
            var diagnoses = CreateDiagnoses();
            var reportGenerator = new ReportGenerator(
                "C:\\Users\\ПК\\source\\repos\\FuzzyLogicForMedicalDataAnalysis\\" +
                $"TestResults\\IntegrationTestPartialResults_Fol_patient_{patient.Guid}.txt");

            //Act
            diagnoses.ForEach(x => x.PatientGuid = patient.Guid);
            foreach (var result in resultList)
            {
                result.LowResult.GetAffiliation();
                result.MidResult.GetAffiliation();
                result.HighResult.GetAffiliation();
            }

            foreach (var rule in rules)
            {
                rule.GetPower(resultList);
            }

            foreach (var rule in rules)
            {
                foreach (var diagnosis in diagnoses)
                {
                    foreach (var outputTerm in rule.OutputTerms)
                    {
                        if (diagnosis.Name == outputTerm)
                        {
                            diagnosis.Rules.Add(rule);
                        }
                    }
                }
            }

            foreach (var diagnosis in diagnoses)
            {
                diagnosis.GetAffiliation();
            }


            reportGenerator.GenerateReport(patient, resultList, diagnoses, true);

            //Assert
            var isSick = diagnoses.Any(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isSick);

            var isB12 = diagnoses.Where(diagnosis => diagnosis.Name == "Фолиеводефицитная анемия")
                                 .FirstOrDefault(diagnosis => diagnosis.Affiliation > 0);
            Assert.IsTrue(isB12?.Affiliation > 0);
        }

        public Patient CreatePatient(int age, string gender)
        {
            return new Patient()
            {
                Id = 1,
                Age = age,
                FirstName = "Test",
                LastName = "Test",
                MiddleName = "Test",
                Gender = gender,
                Guid = Guid.NewGuid()
            };
        }

        public AnalysisResult CreateAnalysisResult(Guid patientGuid, string analysisName,
            decimal refLo, decimal refHi, decimal current)
        {
            return new AnalysisResult
            {
                AnalysisName = analysisName,
                PatientGuid = patientGuid,
                CurrentValue = current,
                LowResult = new LowResult(current) { MinValue = refLo, MaxValue = (refHi + refLo) / 2 },
                MidResult = new MidResult(current) { MinValue = refLo, MaxValue = refHi },
                HighResult = new HighResult(current) { MinValue = (refHi + refLo) / 2, MaxValue = refHi }
            };
        }

        public List<FuzzyRule> CreateRules()
        {
            var medicalDataManager = new FakeMedicalDataManager();
            return medicalDataManager.GetAllFakeRules();
        }

        public List<Diagnosis> CreateDiagnoses()
        {
            var medicalDataManager = new FakeMedicalDataManager();
            return medicalDataManager.GetFakeDiagnoses();
        }
    }
}

