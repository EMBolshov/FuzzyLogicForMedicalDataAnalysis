using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;
using Repository;
using WebApi.Interfaces;

namespace WebApi.Implementations
{
    public class AnalysisResultDbProvider : IAnalysisResultProvider
    {
        private readonly IMainProcessingRepository _repo;

        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            return _repo.GetAnalysisResultsByPatientGuid(patientGuid);
        }

        public void CreateNewAnalysisResult(CreateAnalysisResultDto dto)
        {
            _repo.CreateAnalysisResult(dto);
        }
    }
}
