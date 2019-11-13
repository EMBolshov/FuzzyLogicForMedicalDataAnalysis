using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAnalysisResultProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        void CreateNewAnalysisResult(CreateAnalysisResultDto dto);
    }
}
