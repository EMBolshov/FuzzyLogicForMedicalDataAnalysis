using System;
using FuzzyLogicMedicalCore;
using MedicalDataCore;

namespace FuzzyLogicTestingConsole.Manager
{
    public class MedicalDataManager
    {
       public Patient GetPatientById(Guid guid)
        {
            //TODO get patient info by guid from database
            //now - example - return new patient
            var patient = new Patient()
            {
                Age = 25,
                FirstName = "Test Subject FirstName",
                LastName = "Test Subject LastName",
                MiddleName = "Test Subject MiddleName",
                Gender = 0, //Male
                PatientId = guid
            };
            return patient;
        }
    }
}
