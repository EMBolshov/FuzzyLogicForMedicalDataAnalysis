using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces;

namespace WebApi.Implementations
{
    public class DiagnosisDbProvider : IDiagnosisProvider
    {
        private readonly IMainProcessingRepository _repo;
        
        public DiagnosisDbProvider(IMainProcessingRepository repo)
        {
            _repo = repo;
        }
        
        public List<Diagnosis> GetAllDiagnoses()
        {
            return _repo.GetAllDiagnoses();
        }
        
        public void CreateNewDiagnosis(CreateDiagnosisDto createDiagnosisDto)
        {
            _repo.CreateDiagnosis(createDiagnosisDto);
        }
    }
}
