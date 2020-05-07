using System;
using System.Collections.Generic;
using POCO.Domain;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.MainProcessing
{
    public class TestAccuracyDbProvider : ITestAccuracyProvider
    {
        private readonly IMainProcessingRepository _repo;


        public TestAccuracyDbProvider(IMainProcessingRepository repo)
        {
            _repo = repo;
        }

        public void CreateTestAccuracy(TestAccuracy testAccuracy)
        {
            _repo.CreateTestAccuracy(testAccuracy);
        }

        public List<TestAccuracy> GetAllTestAccuracies()
        {
            return _repo.GetAllTestAccuracies();
        }

        public void DeleteAllTestAccuracies()
        {
            throw new NotImplementedException();
        }
    }
}
