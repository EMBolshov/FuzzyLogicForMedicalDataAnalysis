using System;
using FuzzyLogicMedicalCore.MedicalFuzzyDataModel;
using FuzzyLogicTestingConsole.Manager;

namespace FuzzyLogicTestingConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //TODO make this test
            var medicalDataManager = new MedicalDataFakeManager();
            var patient = medicalDataManager.GetFakePatient();
            var fakeResults = medicalDataManager.GetFakeAnalysisResults(patient.Guid);
            var fakeDiagnoses = medicalDataManager.GetFakeDiagnoses();
            var fakeRules = medicalDataManager.GetAllFakeRules();
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
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
