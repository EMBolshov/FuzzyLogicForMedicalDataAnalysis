using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCO.Domain.Dto;
using Repository;
using UnitTests.MocksAndStubs;

namespace UnitTests.ProvidersTests
{
    [TestClass]
    public class DiagnosisDbProviderTest
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
            _repo.CreateDiagnosis(new CreateDiagnosisDto(){DiagnosisName = "Test"});
        }

        [TestMethod]
        public void GetAllDiagnosesTest()
        {
            var sut = _repo.GetAllDiagnoses();
            Assert.IsTrue(sut != null && sut.Count > 0);
        }
    }
}
