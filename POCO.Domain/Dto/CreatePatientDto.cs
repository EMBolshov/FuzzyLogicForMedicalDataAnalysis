using System;

namespace POCO.Domain.Dto
{
    public class CreatePatientDto
    {
        public Guid Guid { get; set; }
        public DateTime InsertedDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public bool IsRemoved { get; set; }
    }
}
