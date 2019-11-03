using Microsoft.Extensions.Options;
using Repository;
using WebApi.POCO;

namespace WebApi.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class MainRepositoryWrapper : IMainProcessingRepository
    {
        private readonly IMainProcessingRepository _repo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public MainRepositoryWrapper(IOptions<Config> config)
        {
            var mainRepoConnectionString = config.Value.MainProcessingConnectionString;
            _repo = new MainProcessingRepository(mainRepoConnectionString);
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
