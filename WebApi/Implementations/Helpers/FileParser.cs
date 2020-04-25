using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FileHelpers;
using POCO.Domain;
using WebApi.Interfaces.Helpers;
using WebApi.POCO;

namespace WebApi.Implementations.Helpers
{
    public class FileParser : IFileParser
    {
        private readonly INamingMapper _mapper;

        public FileParser(INamingMapper mapper)
        {
            _mapper = mapper;
        }

        public List<AnalysisResult> GetAnalysisResultsFromCsv(string path)
        {
            var result = new List<AnalysisResult>();
            var fileHelperEngine = new FileHelperEngine<PatientAnalysisResultCsvFormat>();
            var specialTestNames = new List<string>
            {
                "OAK_22_ПОКАЗАТЕЛЯ_EXT_IPU",
                "OAK_8_ПОКАЗАТЕЛЕЙ_EXT_IPU",
                "OAK_22_ПОКАЗАТЕЛЯ_С_МИКР_EXT_IPU",
                "OAK_РЕТИКУЛОЦИТЫ_EXT_IPU",
                "ТРАНСФЕРРИН_COBAS",
                "РЕЦЕПТОР_ТРАНСФЕРРИН_ДИАЛАБ"
            };

            var fileResult = fileHelperEngine.ReadFile(path).ToList();

            fileResult.ForEach(record =>
            {
                var analysisResult = new AnalysisResult
                {
                    PatientGuid = record.PatientGuid,
                    AnalysisName = record.AnalysisName,
                    //Entry пустой, FormattedEntry лучше сразу парсить как decimal
                    Entry = record.FormattedEntry,
                    FormattedEntry = record.FormattedEntry.ToString(CultureInfo.InvariantCulture),
                    Guid = Guid.NewGuid(),
                    InsertedDate = DateTime.Now,
                    ReferenceHigh = record.ReferenceHigh,
                    ReferenceLow = record.ReferenceLow,
                    ReportedName = record.ReportedName,
                    Confidence = 1m,
                    IsRemoved = false
                };

                var testName = specialTestNames.Contains(record.AnalysisName)
                    ? _mapper.ChangeTestName(record.ReportedName)
                    : _mapper.MapTestNameByAnalysisName(record.AnalysisName);

                analysisResult.TestName = testName;
                analysisResult.Loinc = _mapper.MapTestNameWithLoinc(testName);

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

            result = result.GroupBy(x => x.Guid, (key, group) => group.First()).ToList();

            return result;
        }
    }
}
