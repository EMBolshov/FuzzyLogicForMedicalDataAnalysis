using POCO.Domain;
using POCO.Domain.Dto;
using WebApi.Interfaces.Helpers;

namespace WebApi.Implementations.Helpers
{
    public class EntitiesToCreateDtoMapper : IEntitiesToCreateDtoMapper
    {
        public CreateAnalysisResultDto AnalysisResultToDto(AnalysisResult source)
        {
            return new CreateAnalysisResultDto
            {
                Guid = source.Guid,
                AnalysisName = source.AnalysisName,
                Entry = source.Entry,
                PatientGuid = source.PatientGuid,
                IsRemoved = source.IsRemoved,
                Loinc = source.Loinc,
                ReportedName = source.ReportedName,
                ReferenceHigh = source.ReferenceHigh,
                ReferenceLow = source.ReferenceLow,
                Confidence = source.Confidence,
                TestName = source.TestName,
                FormattedEntry = source.FormattedEntry,
                InsertedDate = source.InsertedDate
            };
        }

        public CreateDiagnosisDto DiagnosisToDiagnosisDto(Diagnosis source)
        {
            return new CreateDiagnosisDto
            {
                Guid = source.Guid,
                DiagnosisName = source.Name,
                IsRemoved = source.IsRemoved,
                MkbCode = source.MkbCode
            };
        }

        public CreatePatientDto PatientToCreatePatientDto(Patient source)
        {
            return new CreatePatientDto
            {
                Guid = source.Guid,
                IsRemoved = source.IsRemoved,
                MiddleName = source.MiddleName,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Age = source.Age,
                InsertedDate = source.InsertedDate,
                Gender = source.Gender
            };
        }

        public CreateRuleDto RuleToCreateRuleDto(Rule source)
        {
            return new CreateRuleDto
            {
                Guid = source.Guid,
                IsRemoved = source.IsRemoved,
                Analysis = source.Analysis,
                DiagnosisName = source.DiagnosisName,
                InputTermName = source.InputTermName,
                Power = source.Power,
                Id = source.Id
            };
        }
    }
}
