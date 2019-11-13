﻿using System;

namespace POCO.Domain
{
    public class Patient
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
