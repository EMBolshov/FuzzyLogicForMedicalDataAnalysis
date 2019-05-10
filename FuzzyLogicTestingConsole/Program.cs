using System;
using FuzzyLogicMedicalCore.ReportGeneration;

namespace FuzzyLogicTestingConsole
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

            foreach (var patient in patientList)
            {
                var reportGenerator = new ReportGenerator(medicalDataManager.PathToReports + $"_{patient.Guid}.txt");
                fakeDiagnoses.ForEach(x => x.PatientGuid = patient.Guid);
                var fakeResults = medicalDataManager.GetFakeAnalysisResults(patient.Guid);
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
                    Console.WriteLine($"Diagnosis: {fakeDiagnosis.Name}, Probability: {fakeDiagnosis.Affiliation}");
                }

                reportGenerator.GenerateReport(patient, fakeResults, fakeDiagnoses);

                Console.WriteLine("New generation \n");
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
