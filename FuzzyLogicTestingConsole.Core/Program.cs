using System;
using System.Collections.Generic;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;
using FuzzyLogicMedicalCore.BL.ReportGeneration;
using Microsoft.Extensions.Configuration;

namespace FuzzyLogicTestingConsole.Core
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Console started");
            var medicalDataManager = new FakeMedicalDataManager();
            var patientList = medicalDataManager.GetFakePatientList();
            var fakeRules = medicalDataManager.GetAllFakeRules();
            var fakeDiagnoses = medicalDataManager.GetFakeDiagnoses();
            var diagnosesResultsForStatistic = new List<Diagnosis>();

            foreach (var patient in patientList)
            {
                Console.WriteLine("New generation \n");
                var reportGenerator = new ReportGenerator(medicalDataManager.PathToReports + $"patient_{patient.Guid}.txt");
                fakeDiagnoses.ForEach(x => x.PatientGuid = patient.Guid);
                var fakeResults = medicalDataManager.GetFakeAnalysisResults(patient.Guid);
                medicalDataManager.GetAnalysisResultsAffiliation(fakeResults);
                medicalDataManager.GetPowerOfRules(fakeRules, fakeResults);

                foreach (var result in fakeResults)
                {
                    Console.WriteLine($"AnalysisName: {result.AnalysisName}, " +
                                      $"Low Affiliation {result.LowResult.Affiliation}, " +
                                      $"Mid Affiliation {result.MidResult.Affiliation}, " +
                                      $"High Affiliation {result.HighResult.Affiliation}");
                }

                foreach (var rule in fakeRules)
                {
                    Console.WriteLine($"Rule: {rule.Id}, Power: {rule.Power}");
                    medicalDataManager.GetDiagnosisAffiliation(fakeDiagnoses, rule);
                }

                foreach (var fakeDiagnosis in fakeDiagnoses)
                {
                    fakeDiagnosis.GetAffiliation();
                    diagnosesResultsForStatistic.Add(CloneDiagnosis(fakeDiagnosis));
                    Console.WriteLine($"Diagnosis: {fakeDiagnosis.Name}, Probability: {fakeDiagnosis.Affiliation}");
                }

                reportGenerator.GenerateReport(patient, fakeResults, fakeDiagnoses);
            }

            var statisticGenerator = new ReportGenerator(medicalDataManager.PathToReports + "A_" + DateTime.Now.ToString("dd/MM/yyyy") + ".txt");
            statisticGenerator.GenerateStatistics(patientList, diagnosesResultsForStatistic);
            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        public static Diagnosis CloneDiagnosis(Diagnosis diagnosis)
        {
            return new Diagnosis()
            {
                Affiliation = diagnosis.Affiliation,
                Name = diagnosis.Name,
                PatientGuid = diagnosis.PatientGuid,
                Rules = diagnosis.Rules
            };
        }
    }
}
