using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using UnitTests.MocksAndStubs;

namespace UnitTests
{
    [TestClass]
    public class DiagnosisProviderTest
    {
        private IMainProcessingRepository _repo;

        [TestInitialize]
        public void TestInit()
        {
            _repo = new FakeMainProcessingRepository();
        }

        [TestMethod]
        public void CreateDiagnosisTest()
        {
            _repo.CreateNewDiagnosis("Test");
        }
    }
}
