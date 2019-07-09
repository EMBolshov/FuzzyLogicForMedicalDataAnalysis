using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using FuzzyLogicMedicalCore.BL.FHIR;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;
using Newtonsoft.Json;

namespace RealDataGenerator
{
    public class AnemiaFakeDataFileGenerator
    {
        private readonly string _pathToFolder;
        public List<string> MalePatientFirstNames { get; set; }
        public List<string> MalePatientMiddleNames { get; set; }
        public List<string> MalePatientLastNames { get; set; }
        public List<string> FemalePatientFirstNames { get; set; }
        public List<string> FemalePatientMiddleNames { get; set; }
        public List<string> FemalePatientLastNames { get; set; }
        private static Random _random;

        public AnemiaFakeDataFileGenerator()
        {
            _pathToFolder = ConfigurationManager.AppSettings["PathToFolderWithGeneratedData"];
            _random = new Random();
            MalePatientFirstNames = new List<string>();
            MalePatientMiddleNames = new List<string>();
            MalePatientLastNames = new List<string>();
            FemalePatientFirstNames = new List<string>();
            FemalePatientMiddleNames = new List<string>();
            FemalePatientLastNames = new List<string>();
            FillMalePatientFIOs();
            FillFemalePatientFIOs();
        }

        public List<Patient> MakePatients(int times)
        {
            var patientList = new List<Patient>();
            
            for (var i = 0; i < times; i++)
            {
                var gender = _random.Next(0, 2);
                if (gender == 0)
                {
                    var firstName = MalePatientFirstNames.ElementAt(_random.Next(0, 9));
                    var lastName = MalePatientLastNames.ElementAt(_random.Next(0, 9));
                    var middleName = MalePatientMiddleNames.ElementAt(_random.Next(0, 9));

                    var patient = new Patient()
                    {
                        Id = i,
                        Age = _random.Next(0,70),
                        FirstName = firstName,
                        LastName = lastName,
                        MiddleName = middleName,
                        Gender = "Male",
                        Guid = Guid.NewGuid()
                    };
                    patientList.Add(patient);
                }
                else
                {
                    var firstName = FemalePatientFirstNames.ElementAt(_random.Next(0, 9));
                    var lastName = FemalePatientLastNames.ElementAt(_random.Next(0, 9));
                    var middleName = FemalePatientMiddleNames.ElementAt(_random.Next(0, 9));

                    var patient = new Patient()
                    {
                        Id = i,
                        Age = _random.Next(0, 70),
                        FirstName = firstName,
                        LastName = lastName,
                        MiddleName = middleName,
                        Gender = "Female",
                        Guid = Guid.NewGuid()
                    };
                    patientList.Add(patient);
                }
            }

            using (var file = File.CreateText(_pathToFolder + "Patients.json"))
            {
                var json = JsonConvert.SerializeObject(patientList, Formatting.Indented);
                file.Write(json);
            }

            return patientList;
        }

        public List<AnalysisResult> GenerateAnalyzesWithReferencesAndResults(Guid patientGuid)
        {
            var resultList = new List<AnalysisResult>
            {
                GetReferencesForAnalysis("Гемоглобин (HGB)", patientGuid),
                GetReferencesForAnalysis("Железо в сыворотке", patientGuid),
                GetReferencesForAnalysis("Ферритин", patientGuid),
                GetReferencesForAnalysis("Витамин B12", patientGuid),
                GetReferencesForAnalysis("Фолат сыворотки", patientGuid)
            };

            using (var file = File.CreateText(_pathToFolder + $"Analyses_{ patientGuid}.json"))
            {
                var json = JsonConvert.SerializeObject(resultList, Formatting.Indented);
                file.Write(json);
            }

            return resultList;
        }

        public AnalysisResult GetReferencesForAnalysis(string analysisName, Guid patientGuid)
        {
            var result = new AnalysisResult
            {
                AnalysisName = analysisName,
                PatientGuid = patientGuid
            };

            switch (analysisName)
            {
                default:
                    GetRandomReferences(result);
                    break;
            }
            
            return result;
        }

        public void GetRandomReferences(AnalysisResult result)
        {
            var lowMin = _random.Next(0, 200);
            var lowMax = _random.Next(lowMin, lowMin + 200);
            var midMin = (lowMin + lowMax) / 2;
            var highMin = lowMax;
            var highMax = _random.Next(highMin, highMin + 200);
            var midMax = (lowMax + highMax) / 2;
            var currentValue = _random.Next(lowMin, highMax);

            result.CurrentValue = currentValue;
            result.LowResult = new LowResult(currentValue) {MaxValue = lowMax, MinValue = lowMin};
            result.MidResult = new MidResult(currentValue) {MaxValue = midMax, MinValue = midMin};
            result.HighResult = new HighResult(currentValue) {MaxValue = highMax, MinValue = highMin};
        }

        public void MakeDiagnoses()
        {
            var diagnoses = new List<Diagnosis>
            {
                new Diagnosis()
                {
                    Name = "Железодефицитная анемия (ЖДА)"
                },

                new Diagnosis()
                {
                    Name = "Анемия хронических заболеваний (АХЗ)"
                },

                new Diagnosis()
                {
                    Name = "B12-дефицитная анемия"
                },

                new Diagnosis()
                {
                    Name = "Фолиеводефицитная анемия"
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
                    InputTerms = new List<InputTerm>()
                    {
                        new InputTerm()
                        {
                            AnalysisName = "Гемоглобин (HGB)",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Железо в сыворотке",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Гемоглобин (HGB)",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Гемоглобин (HGB)",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Ферритин",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Витамин B12",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Витамин B12",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Фолат сыворотки",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Фолат сыворотки",
                            AnalysisTerm = "High"
                        }
                    },

                    OutputTerms = new List<string>(){"Железодефицитная анемия (ЖДА)"},
                },
                new Rule()
                {
                    Id = 2,
                    InputTerms = new List<InputTerm>()
                    {
                        new InputTerm()
                        {
                            AnalysisName = "Гемоглобин (HGB)",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Железо в сыворотке",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Железо в сыворотке",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Ферритин",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Ферритин",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Витамин B12",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Витамин B12",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Фолат сыворотки",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Фолат сыворотки",
                            AnalysisTerm = "High"
                        },
                    },

                    OutputTerms = new List<string>(){"Анемия хронических заболеваний (АХЗ)"},
                },
                new Rule()
                {
                    Id = 3,
                    InputTerms = new List<InputTerm>()
                    {
                        new InputTerm()
                        {
                            AnalysisName = "Гемоглобин (HGB)",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Железо в сыворотке",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Железо в сыворотке",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Ферритин",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Ферритин",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Витамин B12",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Фолат сыворотки",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Фолат сыворотки",
                            AnalysisTerm = "High"
                        }
                    },

                    OutputTerms = new List<string>(){"B12-дефицитная анемия"},
                },
                new Rule()
                {
                    Id = 4,
                    InputTerms = new List<InputTerm>()
                    {
                        new InputTerm()
                        {
                            AnalysisName = "Гемоглобин (HGB)",
                            AnalysisTerm = "Low"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Железо в сыворотке",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Железо в сыворотке",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Ферритин",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Ферритин",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Витамин B12",
                            AnalysisTerm = "Mid"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Витамин B12",
                            AnalysisTerm = "High"
                        },
                        new InputTerm()
                        {
                            AnalysisName = "Фолат сыворотки",
                            AnalysisTerm = "Low"
                        }
                    },

                    OutputTerms = new List<string>(){"Фолиеводефицитная анемия"},
                }
            };

            using (var file = File.CreateText(_pathToFolder + $"Rules.json"))
            {
                var json = JsonConvert.SerializeObject(allRules, Formatting.Indented);
                file.Write(json);
            }
        }

        // ReSharper disable once InconsistentNaming
        public void FillMalePatientFIOs()
        {
            var firstNames = "Антон, Семён, Иосиф, Варфоломей, Никифор, Серафим, Фома, Захар, Казимир, Венедикт";
            MalePatientFirstNames = firstNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var middleNames = "Прохорович, Изяславович, Артемиевич, Давыдович, Венедиктович, Андреевич, Данилевич, Глебович, Платонович, Мечиславович";
            MalePatientMiddleNames = middleNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var lastNames = "Ломовцев, Сурков, Ложкин, Шеповалов, Юдицкий, Махов, Стожко, Игнаткович, Чуличков, Сёмин";
            MalePatientLastNames = lastNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        // ReSharper disable once InconsistentNaming
        public void FillFemalePatientFIOs()
        {
            var firstNames = "Виктория, Татьяна, Мария, Валерия, Ирина, Людмила, Любовь, Галина, Вера, Анфиса";
            FemalePatientFirstNames = firstNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var middleNames = "Матвеевна, Данииловна, Тимофеевна, Николаевна, Святославовна, Борисовна, Романовна, Филипповна, Геннадьевна, Валентиновна";
            FemalePatientMiddleNames = middleNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var lastNames = "Дроздова, Савина, Рябова, Потапова, Ермакова, Зыкова, Соболева, Борисова, Воробьёва, Исакова";
            FemalePatientLastNames = lastNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
