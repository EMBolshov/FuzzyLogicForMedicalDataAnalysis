using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Interfaces.MainProcessing
{
    public interface IDiagnosisDecisionMaker
    {
        List<ProcessedResult> ProcessForPatient(Patient patient, bool onlyWithFullData = false);
    }
}
