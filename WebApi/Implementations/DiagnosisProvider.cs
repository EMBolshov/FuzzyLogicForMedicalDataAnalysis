using System.Collections.Generic;
using POCO.Domain;
using Repository;
using WebApi.Interfaces;

namespace WebApi.Implementations
{
    public class DiagnosisProvider : IDiagnosisProvider
    {
        private readonly IMainProcessingRepository _repo;

        public DiagnosisProvider(IMainProcessingRepository repo)
        {
            _repo = repo;
        }

        public List<Diagnosis> GetAllDiagnoses()
        {
            var result = new List<Diagnosis>();
            //
            return result;
        }

        public void CreateNewDiagnosis(string diagnosisName)
        {
            _repo.CreateNewDiagnosis(diagnosisName);
        }
    }
}
