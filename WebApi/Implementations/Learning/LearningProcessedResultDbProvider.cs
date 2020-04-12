using System.Collections.Generic;
using POCO.Domain;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class LearningProcessedResultDbProvider : IProcessedResultProvider, IService
    {
        private readonly IMainProcessingRepository _repo;

        public LearningProcessedResultDbProvider(Startup.ServiceResolver resolver)
        {
            _repo = resolver("LearningRepo") as IMainProcessingRepository;
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
