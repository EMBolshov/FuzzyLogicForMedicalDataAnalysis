using System.Collections.Generic;
using POCO.Domain;
using Repository;
using WebApi.Interfaces;
using WebApi.POCO;

namespace WebApi.Implementations
{
    public class PatientDbProvider : IPatientProvider
    {
        private readonly IMainProcessingRepository _repo;

        public List<Patient> GetAllPatients()
        {
            return _repo.GetAllPatients();
        }

        public void CreateNewPatient(CreatePatientDto dto)
        {
            _repo.CreateNewPatient(dto);
        }
    }
}
