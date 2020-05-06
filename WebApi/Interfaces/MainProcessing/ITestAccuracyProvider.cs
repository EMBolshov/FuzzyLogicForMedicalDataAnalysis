using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Interfaces.MainProcessing
{
    public interface ITestAccuracyProvider
    {
        List<TestAccuracy> GetAllTestAccuracies();

        void CreateTestAccuracy(TestAccuracy testAccuracy);
    }
}
