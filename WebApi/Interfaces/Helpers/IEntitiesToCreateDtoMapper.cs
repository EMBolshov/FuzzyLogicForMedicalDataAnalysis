using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces.Helpers
{
    public interface IEntitiesToCreateDtoMapper
    {
        CreateAnalysisResultDto AnalysisResultToDto(AnalysisResult source);
        CreateDiagnosisDto DiagnosisToDiagnosisDto(Diagnosis source);
        CreatePatientDto PatientToCreatePatientDto(Patient source);
        CreateRuleDto RuleToCreateRuleDto(Rule dto);
    }
}
