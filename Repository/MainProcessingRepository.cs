using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dapper;
using Npgsql;
using POCO.Domain;
using POCO.Domain.Dto;

namespace Repository
{
    public class MainProcessingRepository : IMainProcessingRepository
    {
        private static string _connectionString;

        public MainProcessingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void CreateDiagnosis(CreateDiagnosisDto dto)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO \"Diagnosis\" (\"Name\", \"Guid\", \"MkbCode\", \"IsRemoved\") " +
                          $"VALUES ('{dto.DiagnosisName}', '{dto.Guid}', '{dto.MkbCode}', '{dto.IsRemoved}')";

                context.Execute(sql);
            }
        }

        public List<Diagnosis> GetAllDiagnoses()
        {
            List<Diagnosis> result;

            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT \"Id\", \"Name\", \"Guid\", \"MkbCode\", \"IsRemoved\" " +
                          "FROM \"Diagnosis\" WHERE \"IsRemoved\" = 'False'";

                result = context.Query<Diagnosis>(sql).ToList();
            }

            return result;
        }

        public void RemoveDiagnosisByGuid(Guid diagnosisGuid)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "UPDATE \"Diagnosis\" set \"IsRemoved\" = \'true\' " +
                          $"WHERE \"Guid\" = '{diagnosisGuid}'";

                context.Execute(sql);
            }
        }

        public void CreatePatient(CreatePatientDto dto)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO \"Patient\" (\"Guid\", \"InsertedDate\", \"FirstName\", " +
                          "\"MiddleName\", \"LastName\", \"Age\", \"Gender\", \"IsRemoved\") " +
                          $"VALUES ('{dto.Guid}', '{dto.InsertedDate}', '{dto.FirstName}', " +
                          $"'{dto.MiddleName}', '{dto.LastName}', '{dto.Age}', '{dto.Gender}', " +
                          $"'{dto.IsRemoved}')";

                context.Execute(sql);
            }
        }

        public List<Patient> GetAllPatients()
        {
            List<Patient> result;

            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT \"Id\", \"Guid\", \"InsertedDate\", \"FirstName\", " +
                          "\"MiddleName\", \"LastName\", \"Age\", \"Gender\", \"IsRemoved\" " +
                          "FROM \"Patient\" WHERE \"IsRemoved\" = 'False'";

                result = context.Query<Patient>(sql).ToList();
            }

            return result;
        }

        public void RemovePatientByGuid(Guid patientGuid)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "UPDATE \"Patient\" set \"IsRemoved\" = \'true\' " +
                          $"WHERE \"Guid\" = '{patientGuid}'";

                context.Execute(sql);
            }
        }

        public void CreateAnalysisResult(CreateAnalysisResultDto dto)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO \"AnalysisResult\" (\"Guid\", \"PatientGuid\", \"InsertedDate\", " +
                          "\"TestName\", \"TestName\", \"Loinc\", \"ReportedName\", \"Entry\", " +
                          "\"FormattedEntry\", \"ReferenceLow\", \"ReferenceHigh\", \"Confidence\", \"IsRemoved\") " +
                          $"VALUES ('{dto.Guid}', '{dto.PatientGuid}', '{dto.InsertedDate}', " +
                          $"'{dto.AnalysisName}', '{dto.TestName}', '{dto.Loinc}', '{dto.ReportedName}', " +
                          $"'{dto.Entry.ToString(CultureInfo.InvariantCulture).Replace(',','.')}', " +
                          $"'{dto.FormattedEntry}', '{dto.ReferenceLow.ToString(CultureInfo.InvariantCulture).Replace(',', '.')}', " +
                          $"'{dto.ReferenceHigh.ToString(CultureInfo.InvariantCulture).Replace(',', '.')}', " +
                          $"'{dto.Confidence}', '{dto.IsRemoved}')";

                context.Execute(sql);
            }
        }
        
        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            List<AnalysisResult> result;

            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT \"Id\", \"Guid\", \"PatientGuid\", \"InsertedDate\", " +
                          "\"TestName\", \"TestName\", \"Loinc\", \"ReportedName\", \"Entry\", " +
                          "\"FormattedEntry\", \"ReferenceLow\", \"ReferenceHigh\", \"IsRemoved\" " +
                          "FROM \"AnalysisResult\" WHERE \"IsRemoved\" = 'False' AND " +
                          $"\"PatientGuid\" = '{patientGuid}'";

                result = context.Query<AnalysisResult>(sql).ToList();
            }

            return result;
        }

        //TODO: Implement! Need to join with diagnosis results - А оно мне действительно надо?
        public List<AnalysisResult> GetPositiveAnalysisResultsByDiagnosisGuid(Guid diagnosisGuid)
        {
            throw new NotImplementedException();
        }

        public void RemoveAnalysisResultByGuid(Guid analysisResultGuid)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "UPDATE \"AnalysisResult\" set \"IsRemoved\" = \'true\' " +
                          $"WHERE \"Guid\" = '{analysisResultGuid}'";

                context.Execute(sql);
            }
        }

        public void CreateRule(CreateRuleDto ruleDto)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO \"Rule\" (\"Guid\", \"DiagnosisName\", \"Test\", \"Power\", \"InputTermName\", \"IsRemoved\") " +
                          $"VALUES ('{ruleDto.Guid}', '{ruleDto.DiagnosisName}', '{ruleDto.Test}', " +
                          $"'{ruleDto.Power}', '{ruleDto.InputTermName}', '{ruleDto.IsRemoved}')";

                context.Execute(sql);
            }
        }

        public List<Rule> GetAllActiveRules()
        {
            List<Rule> result;

            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT \"Id\", \"Guid\", \"DiagnosisName\", \"Test\", \"Power\", \"InputTermName\", \"IsRemoved\" " +
                          "FROM \"Rule\" WHERE \"IsRemoved\" = 'False'";

                result = context.Query<Rule>(sql).ToList();
            }

            return result;
        }

        public void RemoveRuleByGuid(Guid ruleGuid)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "UPDATE \"Rule\" set \"IsRemoved\" = \'true\' " +
                          $"WHERE \"Guid\" = '{ruleGuid}'";

                context.Execute(sql);
            }
        }

        public void SaveProcessedResult(ProcessedResult result)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO \"ProcessedResult\" (\"Guid\", \"PatientGuid\", \"DiagnosisGuid\", \"Value\", \"InsertedDate\") " +
                          $"VALUES ('{result.Guid}', '{result.PatientGuid}', '{result.DiagnosisGuid}', " +
                          $"'{result.Value}', '{result.InsertedDate}')";

                context.Execute(sql);
            }
        }

        public List<ProcessedResult> GetAllPositiveResults()
        {
            List<ProcessedResult> result;

            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT \"Id\", \"Guid\", \"PatientGuid\", \"DiagnosisGuid\", \"Value\", \"InsertedDate\" " +
                          "FROM \"ProcessedResult\" WHERE \"Value\" > 0";

                result = context.Query<ProcessedResult>(sql).ToList();
            }

            return result;
        }
    }
}
