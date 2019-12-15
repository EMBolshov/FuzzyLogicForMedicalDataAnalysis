using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces
{
    public interface IDiagnosisProvider
    {
        List<Diagnosis> GetAllDiagnoses();

        void CreateNewDiagnosis(CreateDiagnosisDto diagnosisDto);

        void RemoveDiagnosis(Guid diagnosisGuid);
    }
}
