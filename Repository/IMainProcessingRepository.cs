using System.Collections.Generic;
using POCO.Domain;
using WebApi.POCO;

namespace Repository
{
    public interface IMainProcessingRepository
    {
        void CreateNewDiagnosis(CreateDiagnosisDto diagnosisDto);
        List<Diagnosis> GetAllDiagnoses();
        void CreateNewPatient(CreatePatientDto patientDto);
        List<Patient> GetAllPatients();
    }
}
