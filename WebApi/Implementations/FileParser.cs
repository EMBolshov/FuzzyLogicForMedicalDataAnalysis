using System;
using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using POCO.Domain;
using WebApi.Interfaces;
using WebApi.POCO;

namespace WebApi.Implementations
{
    public class FileParser : IFileParser
    {
        public List<AnalysisResult> GetAnalysisResultsFromCsv(string path)
        {
            var result = new List<AnalysisResult>();
            var engine = new FileHelperEngine<PatientAnalysisResultCsvFormat>();
            var specialTestNames = new List<string>
            {
                "OAK_22_ПОКАЗАТЕЛЯ_EXT_IPU",
                "OAK_8_ПОКАЗАТЕЛЕЙ_EXT_IPU",
                "OAK_22_ПОКАЗАТЕЛЯ_С_МИКР_EXT_IPU",
                "OAK_РЕТИКУЛОЦИТЫ_EXT_IPU",
                "ТРАНСФЕРРИН_COBAS",
                "РЕЦЕПТОР_ТРАНСФЕРРИН_ДИАЛАБ"
            };

            var fileResult = engine.ReadFile(path).ToList();

            fileResult.ForEach(record =>
            {
                var analysisResult = new AnalysisResult
                {
                    PatientGuid = record.PatientGuid,
                    AnalysisName = record.AnalysisName,
                    Entry = record.Entry,
                    FormattedEntry = record.FormattedEntry,
                    Guid = Guid.NewGuid(),
                    InsertedDate = DateTime.Now,
                    ReferenceHigh = record.ReferenceHigh,
                    ReferenceLow = record.ReferenceLow,
                    ReportedName = record.ReportedName,
                    IsRemoved = false
                };

                var testName = specialTestNames.Contains(record.AnalysisName)
                    ? ChangeTestName(record.ReportedName)
                    : MapTestNameByAnalysisName(record.AnalysisName);

                analysisResult.TestName = testName;

                result.Add(analysisResult);
            });

            return result;
        }

        public List<Patient> GetPatientsFromCsv(string path)
        {
            var result = new List<Patient>();
            var engine = new FileHelperEngine<PatientAnalysisResultCsvFormat>();
            var fileResult = engine.ReadFile(path).ToList();

            fileResult.ForEach(record =>
            {
                var patient = new Patient
                {
                    Guid = record.PatientGuid,
                    InsertedDate = DateTime.Now,
                    IsRemoved = false,
                    MiddleName = "MiddleName",
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Age = record.Age,
                    Gender = record.Gender
                };

                result.Add(patient);
            });

            return result;
        }

        private string MapTestNameByAnalysisName(string analysisName)
        {
            switch (analysisName)
            {
                case "ФЕРРИТИН_COBAS":
                    return "Ферритин";
                case "ЖЕЛЕЗО_СЫВ_COBAS":
                case "ЖЕЛЕЗО_СЫВ_ХРОМОЛАБ":
                    return "Железо в сыворотке";
                case "ВИТАМИН_B12_ЕВРОТЕСТ":
                case "ВИТАМИН_В12_COBAS":
                    return "Витамин В12";
                case "ФОЛИЕВАЯ_КИСЛОТА_COBAS":
                    return "Фолат сыворотки";
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Don't know how to map analysis with name {analysisName} with TestName");
            }
        }

        private string ChangeTestName(string testName)
        {
            switch (testName)
            {
                case "Концентрация":
                    return "Tрансферрин";
                case "Коэффициент насыщения трансферрина железом":
                    return "Насыщение трансферрина";
                default:
                    return testName;
            }
        }
    }
}
