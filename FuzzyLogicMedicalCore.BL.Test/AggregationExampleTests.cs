using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FuzzyLogicMedicalCore.BL.FHIR;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FuzzyLogicMedicalCore.BL.Test
{
    [TestClass]
    public class AggregationExampleTests
    {
        //Diagnosis:
            //ЖДА, АХЗ
        //Analyzes:
            //KEY: Гемоглобин, Железо в сыворотке, Ферритин, Витамин В12, Фолат сыворотки
            //NON-KEY: Средний объем эритроцита, Гематокрит, Эритроциты, ОЖСС, Трансферин

        private const string PathToFolder = @"C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\GeneratedTestDataAggregation\";
        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());
        public List<string> MalePatientFirstNames { get; set; }
        public List<string> MalePatientMiddleNames { get; set; }
        public List<string> MalePatientLastNames { get; set; }
        public List<string> FemalePatientFirstNames { get; set; }
        public List<string> FemalePatientMiddleNames { get; set; }
        public List<string> FemalePatientLastNames { get; set; }

        [TestMethod]
        public void CreateTestData()
        {
            //Create available analyzes
            //Create strict rules
            //Create diagnosis by strict rules
            //Repeat 1000 times:
                //Create patient
                //Generate patient's analyzes results

            //Available analyzes hardcoded
            CreateStrictRules();
            CreateDiagnoses();
            CreatePatientsWithAnalyzesResults(1000);
        }

        [TestMethod]
        public void GetRulesByLearningOnTestData()
        {
            //Get half of generated data
            //Set diagnosis to a patients using full analyzes results data
            //Select all non-key analyzes results from previous patient results
            //Modify diagnosis by using fuzzy rules

            var trainingData = new List<AnalysisResult>();
            var files = Directory.GetFiles(PathToFolder, "Analyzes_*.json");
            foreach (var file in files)
            {
                using (var analysis = File.OpenText(file))
                {
                    var serializer = new JsonSerializer();
                    trainingData.AddRange((List<AnalysisResult>)serializer.Deserialize(analysis, typeof(List<AnalysisResult>)));
                }
            }

            var testingData = trainingData.GetRange(trainingData.Count / 2 - 1, trainingData.Count / 2);
            trainingData = trainingData.Except(testingData).ToList();
            
        }

        [TestMethod]
        public void MainTestMethod()
        {

        }

        [TestMethod]
        public void CleanUp()
        {
            var files = Directory.GetFiles(PathToFolder, "*.json");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            files = Directory.GetFiles(PathToFolder, "*.json");
            Assert.IsTrue(files == null || files.Length == 0);
        }

        private void CreateDiagnoses()
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
                }
            };

            using (var file = File.CreateText(PathToFolder + $"Diagnoses.json"))
            {
                var json = JsonConvert.SerializeObject(diagnoses, Formatting.Indented);
                file.Write(json);
            }
        }

        private void CreateStrictRules()
        {
            var allRules = new List<FuzzyRule>
            {
                new FuzzyRule()
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
                new FuzzyRule()
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
                }
            };

            using (var file = File.CreateText(PathToFolder + $"Rules.json"))
            {
                var json = JsonConvert.SerializeObject(allRules, Formatting.Indented);
                file.Write(json);
            }
        }

        private void CreatePatientsWithAnalyzesResults(int times)
        {
            FillFemalePatientFIOs();
            FillMalePatientFIOs();

            var patientList = new List<Patient>();

            for (var i = 0; i < times; i++)
            {
                var patient = new Patient();
                var gender = Random.Next(0, 2);
                if (gender == 0)
                {
                    var firstName = MalePatientFirstNames.ElementAt(Random.Next(0, 9));
                    var lastName = MalePatientLastNames.ElementAt(Random.Next(0, 9));
                    var middleName = MalePatientMiddleNames.ElementAt(Random.Next(0, 9));

                    patient.Id = i;
                    patient.Age = Random.Next(0, 70);
                    patient.FirstName = firstName;
                    patient.LastName = lastName;
                    patient.MiddleName = middleName;
                    patient.Gender = "Male";
                    patient.Guid = Guid.NewGuid();
                    
                    patientList.Add(patient);
                }
                else
                {
                    var firstName = FemalePatientFirstNames.ElementAt(Random.Next(0, 9));
                    var lastName = FemalePatientLastNames.ElementAt(Random.Next(0, 9));
                    var middleName = FemalePatientMiddleNames.ElementAt(Random.Next(0, 9));

                    patient.Id = i;
                    patient.Age = Random.Next(0, 70);
                    patient.FirstName = firstName;
                    patient.LastName = lastName;
                    patient.MiddleName = middleName;
                    patient.Gender = "Female";
                    patient.Guid = Guid.NewGuid();
                    
                    patientList.Add(patient);
                }

                GenerateAnalyzesWithReferencesAndResults(patient.Guid);
            }

            using (var file = File.CreateText(PathToFolder + "Patients.json"))
            {
                var json = JsonConvert.SerializeObject(patientList, Formatting.Indented);
                file.Write(json);
            }
        }

        private void GenerateAnalyzesWithReferencesAndResults(Guid patientGuid)
        {
            var resultList = new List<AnalysisResult>
            {
                //key
                GetReferencesForAnalysis("Гемоглобин (HGB)", patientGuid),
                GetReferencesForAnalysis("Железо в сыворотке", patientGuid),
                GetReferencesForAnalysis("Ферритин", patientGuid),
                GetReferencesForAnalysis("Витамин B12", patientGuid),
                GetReferencesForAnalysis("Фолат сыворотки", patientGuid),
                //non-key
                GetReferencesForAnalysis("Средний объем эритроцита", patientGuid),
                GetReferencesForAnalysis("Гематокрит", patientGuid),
                GetReferencesForAnalysis("Эритроциты", patientGuid),
                GetReferencesForAnalysis("ОЖСС", patientGuid),
                GetReferencesForAnalysis("Трансферин", patientGuid)
            };

            using (var file = File.CreateText(PathToFolder + $"Analyzes_{ patientGuid}.json"))
            {
                var json = JsonConvert.SerializeObject(resultList, Formatting.Indented);
                file.Write(json);
            }
        }

        private AnalysisResult GetReferencesForAnalysis(string analysisName, Guid patientGuid)
        {
            var result = new AnalysisResult
            {
                AnalysisName = analysisName,
                PatientGuid = patientGuid
            };

            switch (analysisName)
            {
                case "Гемоглобин (HGB)":
                case "Железо в сыворотке":
                case "Ферритин":
                case "Витамин B12":
                case "Фолат сыворотки":
                    result.IsKey = true;
                    GetRandomReferences(result);
                    break;
                default:
                    result.IsKey = false;
                    GetRandomReferences(result);
                    break;
            }

            return result;
        }

        private void GetRandomReferences(AnalysisResult result)
        {
            var lowMin = Random.Next(0, 200);
            var lowMax = Random.Next(lowMin, lowMin + 200);
            var midMin = (lowMin + lowMax) / 2;
            var highMin = lowMax;
            var highMax = Random.Next(highMin, highMin + 200);
            var midMax = (lowMax + highMax) / 2;
            var currentValue = Random.Next(lowMin, highMax);

            result.CurrentValue = currentValue;
            result.LowResult = new LowResult(currentValue) { MaxValue = lowMax, MinValue = lowMin };
            result.MidResult = new MidResult(currentValue) { MaxValue = midMax, MinValue = midMin };
            result.HighResult = new HighResult(currentValue) { MaxValue = highMax, MinValue = highMin };
        }

        // ReSharper disable once InconsistentNaming
        private void FillMalePatientFIOs()
        {
            var firstNames = "Антон, Семён, Иосиф, Варфоломей, Никифор, Серафим, Фома, Захар, Казимир, Венедикт";
            MalePatientFirstNames = firstNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var middleNames = "Прохорович, Изяславович, Артемиевич, Давыдович, Венедиктович, Андреевич, Данилевич, Глебович, Платонович, Мечиславович";
            MalePatientMiddleNames = middleNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var lastNames = "Ломовцев, Сурков, Ложкин, Шеповалов, Юдицкий, Махов, Стожко, Игнаткович, Чуличков, Сёмин";
            MalePatientLastNames = lastNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        // ReSharper disable once InconsistentNaming
        private void FillFemalePatientFIOs()
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
