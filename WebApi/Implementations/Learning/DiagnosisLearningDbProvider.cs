using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class DiagnosisLearningDbProvider : IDiagnosisProvider, IService
    {
        private readonly IMainProcessingRepository _repo;
        
        public DiagnosisLearningDbProvider(Startup.ServiceResolver resolver)
        {
            _repo = resolver("LearningRepo") as IMainProcessingRepository;
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
