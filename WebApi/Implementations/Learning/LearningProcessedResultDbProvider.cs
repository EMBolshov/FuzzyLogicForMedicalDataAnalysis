using System.Collections.Generic;
using POCO.Domain;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class LearningProcessedResultDbProvider : IProcessedResultProvider
    {
        private readonly ILearningRepository _repo;

        public LearningProcessedResultDbProvider(ILearningRepository repo)
        {
            _repo = repo;
        }

        public void SaveProcessedResult(ProcessedResult result)
        {
            _repo.SaveProcessedResult(result);
        }

        public List<ProcessedResult> GetAllPositiveResults()
        {
            return _repo.GetAllPositiveResults();
        }

        public void DeleteAllResults()
        {
            _repo.DeleteAllProcessedResults();
        }
    }
}
