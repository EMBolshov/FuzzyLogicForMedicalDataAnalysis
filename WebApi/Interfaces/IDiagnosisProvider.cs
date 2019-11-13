using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDiagnosisProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Diagnosis> GetAllDiagnoses();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosisDto"></param>
        void CreateNewDiagnosis(CreateDiagnosisDto diagnosisDto);
    }
}
