using POCO.Domain.Dto;

namespace WebApi.Interfaces.Learning
{
    public interface ILearningProcessor
    {
        void ProcessForAllPatients();
        void CreateRule(CreateRuleDto dto);
        void CreateNewDiagnosis(CreateDiagnosisDto dto);
    }
}
