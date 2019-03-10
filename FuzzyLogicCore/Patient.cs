using System;

namespace MedicalDataCore
{
    public class Patient
    {
        //todo use FHIR resource
        public Guid PatientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
    }

    //todo maybe do not use enum
    public enum Gender
    {
        Male,
        Female
    }
}
