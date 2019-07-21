using System;

namespace FuzzyLogicMedicalCore.BL.FHIR
{
    public class Patient
    {        
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
