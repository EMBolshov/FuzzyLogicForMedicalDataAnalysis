using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class DiagnosisLearningDbProvider : IDiagnosisProvider
    {
        private readonly ILearningRepository _repo;
        
        public DiagnosisLearningDbProvider(ILearningRepository repo)
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

        public void RemoveDiagnosis(Guid diagnosisGuid)
        {
            _repo.RemoveDiagnosisByGuid(diagnosisGuid);
        }
    }
}
