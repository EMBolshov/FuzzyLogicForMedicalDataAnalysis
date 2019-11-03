using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using POCO.Domain;
using Repository;
using WebApi.Interfaces;
using WebApi.POCO;

namespace WebApi.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class DiagnosisProvider : IDiagnosisProvider
    {
        private readonly IMainProcessingRepository _repo;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        public DiagnosisProvider(IOptions<Config> config, IMainProcessingRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Diagnosis> GetAllDiagnoses()
        {
            var result = new List<Diagnosis>();
            //
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosisName"></param>
        public void CreateNewDiagnosis(string diagnosisName)
        {
            _repo.CreateNewDiagnosis(diagnosisName);
        }
    }
}
