using System;
using FuzzyLogicMedicalCore;
using FuzzyLogicMedicalCore.FHIR;
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
            var fakeObservationList = medicalDataManager.GetFakeObservationList(patient.Guid);
            var fakeDiagnoses = medicalDataManager.GetFakeDiagnoses();
        }
    }
}
