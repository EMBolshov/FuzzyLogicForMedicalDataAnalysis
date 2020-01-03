using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using POCO.Domain;
using POCO.Domain.Dto;
using WebApi.Interfaces;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Controllers
{
    /// <summary>
    /// CRUD for patient
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientProvider _patientProvider;
        
        public PatientController(IPatientProvider patientProvider)
        {
            _patientProvider = patientProvider;
        }

        /// <summary>
        /// Insert new patient into DB
        /// </summary>
        /// <param name="dto">DTO with all patient info</param>
        [HttpPost("CreateNewPatient")]
        public void CreateNewPatient([FromBody] CreatePatientDto dto)
        {
            _patientProvider.CreateNewPatient(dto);
        }

        /// <summary>
        /// Get all patients from DB with IsRemoved = false
        /// </summary>
        [HttpGet("GetAllPatients")]
        public List<Patient> GetAllPatients()
        {
            return _patientProvider.GetAllPatients();
        }

        /// <summary>
        /// Set IsRemoved = true for patient by GUID
        /// </summary>
        /// <param name="patientGuid">Patient's GUID</param>
        [HttpPost("RemovePatient")]
        public void RemovePatient([FromBody] Guid patientGuid)
        {
            _patientProvider.RemovePatient(patientGuid);
        }
    }
}
