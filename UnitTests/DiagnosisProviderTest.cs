using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using UnitTests.MocksAndStubs;
using WebApi.POCO;

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
            //todo static list?
            _repo.CreateNewDiagnosis(new CreateDiagnosisDto(){DiagnosisName = "Test"});
        }

        [TestMethod]
        public void GetAllDiagnosesTest()
        {
            var sut = _repo.GetAllDiagnoses();
            Assert.IsTrue(sut != null && sut.Count > 0);
        }
    }
}
