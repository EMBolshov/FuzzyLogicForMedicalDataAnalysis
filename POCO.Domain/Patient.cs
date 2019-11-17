using System;

namespace POCO.Domain
{
    public class Patient
    {
        public long Id { get; set; }
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
