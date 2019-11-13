using System.Collections.Generic;
using POCO.Domain;
using WebApi.POCO;

namespace WebApi.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPatientProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Patient> GetAllPatients();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createPatientDto"></param>
        void CreateNewPatient(CreatePatientDto createPatientDto);
    }
}
