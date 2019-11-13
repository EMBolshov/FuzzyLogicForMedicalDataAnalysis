using System.Collections.Generic;
using POCO.Domain;
using Repository;
using WebApi.Interfaces;
using WebApi.POCO;

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
            _repo.CreateNewDiagnosis(createDiagnosisDto);
        }
    }
}
