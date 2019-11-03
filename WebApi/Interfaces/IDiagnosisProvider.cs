using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Interfaces
{
    public interface IDiagnosisProvider
    {
        List<Diagnosis> GetAllDiagnoses();
        void CreateNewDiagnosis(string diagnosisName);
    }
}
