using System;
using System.Collections.Generic;
using POCO.Domain;
using POCO.Domain.Dto;

namespace WebApi.Interfaces
{
    public interface IPatientProvider
    {
        List<Patient> GetAllPatients();

        void CreateNewPatient(CreatePatientDto createPatientDto);

        void RemovePatient(Guid patientGuid);
    }
}
