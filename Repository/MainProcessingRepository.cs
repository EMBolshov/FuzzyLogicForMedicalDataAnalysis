using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using POCO.Domain;
using WebApi.POCO;

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
        public void CreateNewDiagnosis(CreateDiagnosisDto dto)
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
                new Diagnosis() {Name = "Diagnosis1", IcdCode = "1-11", Loinc = "111"},
                new Diagnosis() {Name = "Diagnosis2", IcdCode = "2-22", Loinc = "222"}
            };
            //TODO: remove stub
            return result;
        }

        //TODO
        public void CreateNewPatient(CreatePatientDto dto)
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
    }
}
