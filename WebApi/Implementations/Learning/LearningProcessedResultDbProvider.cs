using System.Collections.Generic;
using POCO.Domain;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class LearningProcessedResultDbProvider : IProcessedResultProvider
    {
        private readonly IMainProcessingRepository _repo;

        public LearningProcessedResultDbProvider(Startup.RepositoryServiceResolver repositoryServiceResolver)
        {
            _repo = repositoryServiceResolver("Learning");
        }

        public void SaveProcessedResult(ProcessedResult result)
        {
            _repo.SaveProcessedResult(result);
        }

        public List<ProcessedResult> GetAllPositiveResults()
        {
            return _repo.GetAllPositiveResults();
        }
    }
}
