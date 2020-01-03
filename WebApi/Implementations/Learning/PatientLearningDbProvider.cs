using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class PatientLearningDbProvider : IPatientProvider
    {
        private readonly IMainProcessingRepository _repo;

        public PatientLearningDbProvider(Startup.RepositoryServiceResolver repositoryServiceResolver)
        {
            _repo = repositoryServiceResolver("Learning");
        }

        public List<Patient> GetAllPatients()
        {
            return _repo.GetAllPatients();
        }

        public void CreateNewPatient(CreatePatientDto dto)
        {
            _repo.CreatePatient(dto);
        }

        public void RemovePatient(Guid patientGuid)
        {
            _repo.RemovePatientByGuid(patientGuid);
        }
    }
}
