using System;
using System.Collections.Generic;
using FuzzyLogicMedicalCore;
using FuzzyLogicMedicalCore.FHIR;
using FuzzyLogicMedicalCore.FuzzyLogic;
using FuzzyLogicMedicalCore.MedicalFuzzyDataModel;

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
                Gender = "Male",
                PatientId = guid
            };
            return patient;
        }

        public List<FakeAnalysisResult> GetFakeAnalysisResults(Guid patientGuid)
        {
            var resultList = new List<FakeAnalysisResult>();
            var randomGenerator = new Random();
            //fake result set generation
            for (var i = 0; i < 3; i++)
            {
                var lowMin = randomGenerator.Next(0, 20);
                var lowMax = randomGenerator.Next(lowMin, lowMin + 20);
                var midMin = randomGenerator.Next(lowMax, lowMax + 20);
                var midMax = randomGenerator.Next(midMin, midMin + 20);
                var highMin = randomGenerator.Next(midMax, midMax + 20);
                var highMax = randomGenerator.Next(highMin, highMin + 20);

                var result = new FakeAnalysisResult()
                {
                    LowResult = new LowResult()
                    { 
                        Name = "result №" + i,
                        CurrentValue = randomGenerator.Next(lowMin, lowMax),
                        MaxValue = lowMax,
                        MinValue = lowMin
                    },

                    MidResult = new MidResult()
                    {
                        Name = "result №" + i,
                        CurrentValue = randomGenerator.Next(midMin, midMax),
                        MaxValue = lowMax,
                        MinValue = lowMin
                    },

                    HighResult = new HighResult()
                    {
                        Name = "result №" + i,
                        CurrentValue = randomGenerator.Next(highMin, highMax),
                        MaxValue = lowMax,
                        MinValue = lowMin
                    }
                };

                result.LowResult.CalculateAffiliation();
                result.MidResult.CalculateAffiliation();
                result.HighResult.CalculateAffiliation();
                result.PatientGuid = patientGuid;

                resultList.Add(result);
            }

            return resultList;
        }
    }
}
