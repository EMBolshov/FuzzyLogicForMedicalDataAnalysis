using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces.Learning
{
    public interface ILearningProcessor
    {
        List<ProcessedResult> ProcessForAllPatients();
        void CreateRule(CreateRuleDto dto);
        void CreateNewDiagnosis(CreateDiagnosisDto dto);
    }
}
