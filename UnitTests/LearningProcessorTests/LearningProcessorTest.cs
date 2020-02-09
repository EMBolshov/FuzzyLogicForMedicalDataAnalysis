using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Builders;

namespace UnitTests.LearningProcessorTests
{
    //TODO: Проверять подготовленные правила по результатом обработки результатов нескольких пациентов
    [TestClass]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LearningProcessorTest
    {
        private readonly StubObjectProvider _stubObjectProvider = new StubObjectProvider();
        private readonly LearningProcessorBuilder _builder = new LearningProcessorBuilder();

        [TestMethod]
        public void ProcessForPatientTestExpectedJDA()
        {
            //Arrange
            var sut = _builder.GetPatientProvider()
                .GetJDAAnalysisResultProvider()
                .GetDiagnosisProvider()
                .GetRulesProvider()
                .GetProcessedResultProvider()
                .GetHtmlReportGenerator()
                .Build();
            
            //Act
            var results = sut.ProcessForAllPatients();

            //Assert
            
        }
    }
}
