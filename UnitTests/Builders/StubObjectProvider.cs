using System;
using System.Collections.Generic;
using POCO.Domain;

namespace UnitTests.Builders
{
    public class StubObjectProvider
    {
        public Patient CreatePatient()
        {
            return new Patient
            {
                Guid = Guid.NewGuid(),
                MiddleName = "MiddleName",
                FirstName = "FirstName",
                LastName = "LastName",
                Age = 23,
                Gender = "Male",
                InsertedDate = DateTime.Now,
                IsRemoved = false,
                Id = 1
            };
        }

        public List<AnalysisResult> CreateAnalysisResultsForJDA(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        public List<AnalysisResult> CreateAnalysisResultsForAHZ(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 12m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        public List<AnalysisResult> CreateAnalysisResultsForFDA(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 21m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        public List<AnalysisResult> CreateAnalysisResultsForB12DA(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 21m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        public List<AnalysisResult> CreateAnalysisResultsWithNormalHgb(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 12m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Ферритин",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.03",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        public List<AnalysisResult> CreateAnalysisResultsWithLowHgb(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                }
            };
        }

        public List<Diagnosis> CreateDiagnoses()
        {
            return new List<Diagnosis>
            {
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "1.1",
                    Name = "Анемия хронических заболеваний"
                },
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "2.1",
                    Name = "Железодефицитная анемия"
                },
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "3.1",
                    Name = "Фолиеводефицитная анемия"
                },
                new Diagnosis
                {
                    Guid = Guid.NewGuid(),
                    IsRemoved = false,
                    MkbCode = "4.1",
                    Name = "B12-дефицитная анемия"
                }
            };
        }

        public List<AnalysisResult> CreateAnalysisResultsForJDAAndAHZ(Guid patientGuid)
        {
            return new List<AnalysisResult>
            {
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Гемоглобин (HGB)",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.01",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Железо в сыворотке",
                    Entry = 1m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.02",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Витамин В12",
                    Entry = 15m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.04",
                    IsRemoved = false
                },
                new AnalysisResult
                {
                    Guid = Guid.NewGuid(),
                    TestName = "Фолат сыворотки",
                    Entry = 25m,
                    Confidence = 1m,
                    PatientGuid = patientGuid,
                    ReferenceLow = 10m,
                    ReferenceHigh = 20m,
                    Loinc = "1.05",
                    IsRemoved = false
                }
            };
        }

        public List<Rule> CreateRules()
        {
            var result = new List<Rule>();
            result.AddRange(CreateRulesForJDA());
            result.AddRange(CreateRulesForAHZ());
            result.AddRange(CreateRulesForFDA());
            result.AddRange(CreateRulesForB12DA());
            return result;
        }

        public List<Rule> CreateRulesForJDA()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Железодефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                }
            };
        }

        public List<Rule> CreateRulesForAHZ()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Анемия хронических заболеваний",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                }
            };
        }

        public List<Rule> CreateRulesForFDA()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "Фолиеводефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                }
            };
        }

        public List<Rule> CreateRulesForB12DA()
        {
            return new List<Rule>
            {
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Гемоглобин (HGB)",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Железо в сыворотке",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Ферритин",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Витамин В12",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Low",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "Normal",
                    Power = 1
                },
                new Rule
                {
                    Guid = Guid.NewGuid(),
                    Test = "Фолат сыворотки",
                    DiagnosisName = "B12-дефицитная анемия",
                    IsRemoved = false,
                    InputTermName = "High",
                    Power = 1
                }
            };
        }
    }
}
