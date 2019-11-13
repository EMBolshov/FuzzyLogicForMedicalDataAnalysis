using System.Collections.Generic;
using System.Transactions;
using POCO.Domain;
using Repository;
using WebApi.POCO;

namespace UnitTests.MocksAndStubs
{
    public class FakeMainProcessingRepository : IMainProcessingRepository
    {
        //TODO static list?
        public void CreateNewDiagnosis(CreateDiagnosisDto diagnosisDto)
        {
            throw new System.NotImplementedException();
        }

        public List<Diagnosis> GetAllDiagnoses()
        {
            var result = new List<Diagnosis>
            {
                new Diagnosis() {Name = "Diagnosis1", IcdCode = "1-11", Loinc = "111"},
                new Diagnosis() {Name = "Diagnosis2", IcdCode = "2-22", Loinc = "222"}
            };
            //TODO: remove stub
            return result;
        }

        public void CreateNewPatient(CreatePatientDto patientDto)
        {
            throw new System.NotImplementedException();
        }

        public List<Patient> GetAllPatients()
        {
            throw new System.NotImplementedException();
        }
    }
}
