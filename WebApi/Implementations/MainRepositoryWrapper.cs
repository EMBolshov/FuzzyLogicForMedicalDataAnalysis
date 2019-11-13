using System.Collections.Generic;
using Microsoft.Extensions.Options;
using POCO.Domain;
using Repository;
using WebApi.POCO;

namespace WebApi.Implementations
{
    public class MainRepositoryWrapper : IMainProcessingRepository
    {
        private readonly IMainProcessingRepository _repo;
        
        public MainRepositoryWrapper(IOptions<Config> config)
        {
            var mainRepoConnectionString = config.Value.MainProcessingConnectionString;
            _repo = new MainProcessingRepository(mainRepoConnectionString);
        }

        public void CreateNewDiagnosis(CreateDiagnosisDto diagnosisDto)
        {
            _repo.CreateNewDiagnosis(diagnosisDto);
        }

        public List<Diagnosis> GetAllDiagnoses()
        {
            return _repo.GetAllDiagnoses();
        }

        public void CreateNewPatient(CreatePatientDto dto)
        {
            _repo.CreateNewPatient(dto);
        }

        public List<Patient> GetAllPatients()
        {
            return _repo.GetAllPatients();
        }
    }
}
