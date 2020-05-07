using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.Learning
{
    public class TestAccuracyLearningDbProvider : ITestAccuracyProvider
    {
        private readonly ILearningRepository _repo;

        public TestAccuracyLearningDbProvider(ILearningRepository repo)
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
            _repo.DeleteAllTestAccuracies();
        }
    }
}
