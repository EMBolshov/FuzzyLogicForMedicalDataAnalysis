using System.Collections.Generic;
using System.Linq;
using POCO.Domain;
using Repository;
using WebApi.Implementations.Helpers;
using WebApi.Implementations.Learning;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Implementations.MainProcessing
{
    public class MainProcessor : IMainProcessor
    {
        private readonly IAnalysisResultProvider _analysisResultProvider;
        private readonly IDiagnosisProvider _diagnosisProvider;
        private readonly IPatientProvider _patientProvider;
        private readonly IReportGenerator _reportGenerator;
        private readonly IDiagnosisDecisionMaker _decisionMaker;
        private readonly ITestAccuracyProvider _testAccuracyProvider;

        private IEnumerable<Diagnosis> Diagnoses => _diagnosisProvider.GetAllDiagnoses();

        public MainProcessor(IMainProcessingRepository mainRepo)
        {
            _analysisResultProvider = new AnalysisResultDbProvider(mainRepo, new FileParser(new AnalysisAndTestsNamingMapper()));
            _diagnosisProvider = new DiagnosisDbProvider(mainRepo);
            _patientProvider = new PatientDbProvider(mainRepo);
            _testAccuracyProvider = new TestAccuracyDbProvider(mainRepo);
            IRuleProvider ruleProvider = new RuleDbProvider(mainRepo);

            _reportGenerator = new HtmlReportGenerator();
            _decisionMaker = new DiagnosisDecisionMaker(_analysisResultProvider, _diagnosisProvider, ruleProvider, _testAccuracyProvider);
        }

        public void ProcessForAllPatients()
        {
            var patients = _patientProvider.GetAllPatients();
            
            patients.ForEach(MakeDiagnosisDecisionAndGenerateReports);
        }

        private void MakeDiagnosisDecisionAndGenerateReports(Patient patient)
        {
            var processedResults = _decisionMaker.ProcessForPatient(patient);

            if (processedResults.Any(x => x.Value > 0))
            {
                var patientResults = _analysisResultProvider.GetAnalysisResultsByPatientGuid(patient.Guid);

                var reportModel = new ReportModel
                {
                    ProcessedResults = processedResults,
                    Patient = patient,
                    AnalysisResults = patientResults,
                    Diagnoses = Diagnoses.ToList(),
                    Path = "TestReports"
                };

                _reportGenerator.GenerateReport(reportModel);
            }
        }
    }
}
