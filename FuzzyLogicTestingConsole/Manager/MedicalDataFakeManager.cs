using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyLogicMedicalCore.FHIR;
using FuzzyLogicMedicalCore.FuzzyLogic;
using FuzzyLogicMedicalCore.MedicalFuzzyDataModel;
using FuzzyLogicTestingConsole.Data;

namespace FuzzyLogicTestingConsole.Manager
{
    public class MedicalDataFakeManager
    {
        public Patient GetFakePatient()
        {
            //TODO get patient info by guid from database
            //now - example - return new patient
            var guid = Guid.NewGuid();
            var patient = new Patient()
            {
                Age = 25,
                FirstName = "Test Subject FirstName",
                LastName = "Test Subject LastName",
                MiddleName = "Test Subject MiddleName",
                Gender = "Male",
                Guid = guid
            };

            return patient;
        }

        public List<IAnalysisResult> GetFakeAnalysisResults(Guid patientGuid)
        {
            var resultList = new List<IAnalysisResult>();
            var randomGenerator = new Random();
            //fake result set generation
            for (var i = 1; i < 4; i++)
            {
                var lowMin = randomGenerator.Next(0, 20);
                var lowMax = randomGenerator.Next(lowMin, lowMin + 20);
                var midMin = (lowMin + lowMax) / 2;
                var highMin = lowMax;
                var highMax = randomGenerator.Next(highMin, highMin + 20);
                var midMax = (lowMax + highMax) / 2;
                var currentValue = randomGenerator.Next(lowMin, highMax);

                var result = new FakeAnalysisResult()
                {
                    AnalysisName = $"Analysis №{i}",
                    LowResult = new LowResult()
                    { 
                        Name = $"result №{i}",
                        CurrentValue = currentValue,
                        MaxValue = lowMax,
                        MinValue = lowMin
                    },

                    MidResult = new MidResult()
                    {
                        Name = $"result №{i}",
                        CurrentValue = currentValue,
                        MaxValue = midMax,
                        MinValue = midMin
                    },

                    HighResult = new HighResult()
                    {
                        Name = $"result №{i}",
                        CurrentValue = currentValue,
                        MaxValue = highMax,
                        MinValue = highMin
                    }
                };

                result.LowResult.GetAffiliation();
                result.MidResult.GetAffiliation();
                result.HighResult.GetAffiliation();
                result.PatientGuid = patientGuid;

                resultList.Add(result);
            }

            return resultList;
        }

        public List<Observation> GetFakeObservationList(Guid patientGuid)
        {
            return new List<Observation>
            {
                new Observation()
                {
                    IndicationName = "Temperature",
                    PatientGuid = patientGuid,
                    ReferenceHigh = 37.0m,
                    ReferenceLow = 35.0m,
                    Value = 36.6m
                },

                new Observation()
                {
                    IndicationName = "Iron in blood",
                    PatientGuid = patientGuid,
                    ReferenceHigh = 1.0m,
                    ReferenceLow = 0.0m,
                    Value = 1.1m
                },

                new Observation()
                {
                    IndicationName = "White blood cells",
                    PatientGuid = patientGuid,
                    ReferenceHigh = 10.0m,
                    ReferenceLow = 0.0m,
                    Value = 5m
                }
            };
        }

        public List<Diagnosis> GetFakeDiagnoses()
        {
            return new List<Diagnosis>
            {
                new Diagnosis()
                {
                    Name = "Blood cancer"
                },

                new Diagnosis()
                {
                    Name = "Anemia"
                },

                new Diagnosis()
                {
                    Name = "Flu"
                }
            };
        }
       
        public List<Rule> GetAllFakeRules()
        {
            //TODO get rules from db
            var allRules = new List<Rule>
            {
                new Rule()
                {
                    Id = 1,
                    InputTerms = "Analysis №1:Low;Analysis №2:Low",
                    OutputTerms = "Diagnosis №1",
                },
                new Rule()
                {
                    Id = 2,
                    InputTerms = "Analysis №1:Low;Analysis №3:High",
                    OutputTerms = "Diagnosis №1;Diagnosis №2",
                },
                new Rule()
                {
                    Id = 3,
                    InputTerms = "Analysis №3:High;Analysis №2:High",
                    OutputTerms = "Diagnosis №2;Diagnosis №3",
                }
            };
            return allRules;
        }

        public void GetPowerOfRules(List<Rule> rules, List<IAnalysisResult> fakeResults)
        {
            foreach (var rule in rules)
            {
                rule.GetPower(fakeResults);
            }
        }
    }
}
