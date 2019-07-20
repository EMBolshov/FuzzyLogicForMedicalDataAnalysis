using System;
using System.Collections.Generic;
using FuzzyLogicMedicalCore.BL.FHIR;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;
using FuzzyLogicMedicalCore.BL.ReportGeneration;
using FuzzyLogicTestingConsole.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzyLogicMedicalCore.BL.Test
{
    [TestClass]
    public class IntegrationTests
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
                CreateAnalysisResult(patient.Guid, "Ферритин", 10m, 120m, 90m),
                CreateAnalysisResult(patient.Guid, "Витамин B12", 191m, 663m, 258m),
                CreateAnalysisResult(patient.Guid, "Фолат сыворотки", 7m, 39m, 9m),
            };
            var rules = CreateRules();
            var diagnoses = CreateDiagnoses();
            var reportGenerator = new ReportGenerator($"C:\\Users\\ПК\\source\\repos\\" +
                                                      $"FuzzyLogicForMedicalDataAnalysis\\" +
                                                      $"TestResults\\IntegrationTest_patient_{patient.Guid}.txt");

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

            Assert.IsTrue(true);
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

        public List<Rule> CreateRules()
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
