using System;

namespace FuzzyLogicMedicalCore.FHIR
{
    public class Patient
    {
        //todo use FHIR resource
        public Guid PatientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
