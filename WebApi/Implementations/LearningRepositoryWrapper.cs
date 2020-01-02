using Microsoft.Extensions.Options;
using Repository;
using WebApi.POCO;

namespace WebApi.Implementations
{
    public class LearningRepositoryWrapper : ILearningRepository
    {
        private readonly ILearningRepository _repo;

        public LearningRepositoryWrapper(IOptions<Config> config)
        {
            var learningDbConnectionString = config.Value.LearningDbConnectionString;
            _repo = new LearningRepository(learningDbConnectionString);
        }
    }
}
