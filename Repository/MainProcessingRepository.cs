using System;
using System.Collections.Generic;
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

        //TODO
        public void CreateDiagnosis(CreateDiagnosisDto dto)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Diagnosis (Name) " +
                          $"VALUES ('{dto.DiagnosisName}')";
                context.Execute(sql);
            }
        }

        public List<Diagnosis> GetAllDiagnoses()
        {
            var result = new List<Diagnosis>
            {
                new Diagnosis() {Name = "Diagnosis1", MkbCode = "1-11", Loinc = "111"},
                new Diagnosis() {Name = "Diagnosis2", MkbCode = "2-22", Loinc = "222"}
            };
            //TODO: remove stub
            return result;
        }

        //TODO
        public void CreatePatient(CreatePatientDto dto)
        {
            //TODO
        }

        public List<Patient> GetAllPatients()
        {
            //todo remove stub
            var result = new List<Patient>()
            {
                new Patient()
                {
                    Guid = Guid.NewGuid(),
                    MiddleName = "MiddleName1",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Age = 23,
                    Gender = "Male"
                },
                new Patient()
                {
                    Guid = Guid.NewGuid(),
                    MiddleName = "MiddleName2",
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Age = 35,
                    Gender = "Female"
                }
            };

            return result;
        }

        //todo
        public void CreateAnalysisResult(CreateAnalysisResultDto dto)
        {
            throw new NotImplementedException();
        }

        //todo
        public List<AnalysisResult> GetAnalysisResultsByPatientGuid(Guid patientGuid)
        {
            throw new NotImplementedException();
        }

        public void CreateRule(CreateRuleDto ruleDto)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Rule (Guid, DiagnosisName, Analysis, Power, InputTermName, IsRemoved) " +
                          $"VALUES ('{ruleDto.Guid}', '{ruleDto.DiagnosisName}', '{ruleDto.Analysis}', " +
                          $"'{ruleDto.Power}', '{ruleDto.InputTermName}', '{ruleDto.IsRemoved}')";

                context.Execute(sql);
            }
        }

        public List<Rule> GetAllActiveRules()
        {
            List<Rule> result;

            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT Id, Guid, DiagnosisName, Analysis, Power, InputTermName, IsRemoved " +
                          "FROM Rule WHERE IsRemoved = False";

                result = context.Query<Rule>(sql).ToList();
            }

            return result;
        }
    }
}
