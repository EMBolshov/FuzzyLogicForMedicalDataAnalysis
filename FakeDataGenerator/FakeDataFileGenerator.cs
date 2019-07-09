using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using FuzzyLogicMedicalCore.BL.FHIR;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;
using FuzzyLogicMedicalCore.BL.MedicalFuzzyDataModel;
using Newtonsoft.Json;

namespace FakeDataGenerator
{
    public class FakeDataFileGenerator
    {
        private readonly string _pathToFolder;
        public List<string> PatientFirstNames { get; set; }
        public List<string> PatientMiddleNames { get; set; }
        public List<string> PatientLastNames { get; set; }
        private static Random _random;

        public FakeDataFileGenerator()
        {
            _pathToFolder = ConfigurationManager.AppSettings["PathToFolderWithGeneratedData"];
            _random = new Random();
            PatientFirstNames = new List<string>();
            PatientMiddleNames = new List<string>();
            PatientLastNames = new List<string>();
            FillPatientFIOs();
        }

        public List<Patient> MakePatients(int times)
        {
            var patientList = new List<Patient>();
            for (var i = 0; i < times; i++)
            {
                var firstName = PatientFirstNames.ElementAt(_random.Next(0, 9));
                var lastName = PatientLastNames.ElementAt(_random.Next(0, 9));
                var middleName = PatientMiddleNames.ElementAt(_random.Next(0, 9));

                var patient = new Patient()
                {
                    Id = i,
                    Age = 25,
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName,
                    Gender = "Male",
                    Guid = Guid.NewGuid()
                };
                patientList.Add(patient);
            }

            using (var file = File.CreateText(_pathToFolder + "Patients.json"))
            {
                var json = JsonConvert.SerializeObject(patientList, Formatting.Indented);
                file.Write(json);
            }

            return patientList;
        }

        public List<AnalysisResult> MakeAnalysis(Guid patientGuid)
        {
            var resultList = new List<AnalysisResult>();
            for (var i = 1; i < 4; i++)
            {
                var lowMin = _random.Next(0, 200);
                var lowMax = _random.Next(lowMin, lowMin + 200);
                var midMin = (lowMin + lowMax) / 2;
                var highMin = lowMax;
                var highMax = _random.Next(highMin, highMin + 200);
                var midMax = (lowMax + highMax) / 2;
                var currentValue = _random.Next(lowMin, highMax);

                var result = new AnalysisResult()
                {
                    AnalysisName = $"Analysis №{i}",
                    CurrentValue = currentValue,
                    LowResult = new LowResult(currentValue)
                    {
                        Name = $"result №{i}",
                        MaxValue = lowMax,
                        MinValue = lowMin
                    },

                    MidResult = new MidResult(currentValue)
                    {
                        Name = $"result №{i}",
                        MaxValue = midMax,
                        MinValue = midMin
                    },

                    HighResult = new HighResult(currentValue)
                    {
                        Name = $"result №{i}",
                        MaxValue = highMax,
                        MinValue = highMin
                    }
                };

                result.PatientGuid = patientGuid;

                resultList.Add(result);
            }

            using (var file = File.CreateText(_pathToFolder + $"Analyses_{ patientGuid}.json"))
            {
                var json = JsonConvert.SerializeObject(resultList, Formatting.Indented);
                file.Write(json);
            }

            return resultList;
        }

        public void MakeDiagnoses()
        {
            var diagnoses = new List<Diagnosis>
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

            using (var file = File.CreateText(_pathToFolder + $"Diagnoses.json"))
            {
                var json = JsonConvert.SerializeObject(diagnoses, Formatting.Indented);
                file.Write(json);
            }
        }

        public void MakeRules()
        {
            var allRules = new List<Rule>
            {
                new Rule()
                {
                    Id = 1,
                    InputTerms = "Analysis №1:Low;Analysis №2:Low",
                    OutputTerms = "Blood cancer",
                },
                new Rule()
                {
                    Id = 2,
                    InputTerms = "Analysis №1:Low;Analysis №3:High",
                    OutputTerms = "Blood cancer;Anemia",
                },
                new Rule()
                {
                    Id = 3,
                    InputTerms = "Analysis №3:High;Analysis №2:High",
                    OutputTerms = "Anemia;Flu",
                }
            };

            using (var file = File.CreateText(_pathToFolder + $"Rules.json"))
            {
                var json = JsonConvert.SerializeObject(allRules, Formatting.Indented);
                file.Write(json);
            }
        }

        public void FillPatientFIOs()
        {
            var firstNames = "Антон, Семён, Иосиф, Варфоломей, Никифор, Серафим, Фома, Захар, Казимир, Венедикт";
            PatientFirstNames = firstNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var middleNames = "Прохорович, Изяславович, Артемиевич, Давыдович, Венедиктович, Андреевич, Данилевич, Глебович, Платонович, Мечиславович";
            PatientMiddleNames = middleNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var lastNames = "Ломовцев, Сурков, Ложкин, Шеповалов, Юдицкий, Махов, Стожко, Игнаткович, Чуличков, Сёмин";
            PatientLastNames = lastNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
