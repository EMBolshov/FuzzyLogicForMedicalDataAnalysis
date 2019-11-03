using Dapper;
using Npgsql;

namespace Repository
{
    public class MainProcessingRepository : IMainProcessingRepository
    {
        private static string _connectionString;

        public MainProcessingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateNewDiagnosis(string diagnosisName)
        {
            using (var context = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Diagnosis (Name) " +
                          $"VALUES ('{diagnosisName}')";
                context.Execute(sql);
            }
        }
    }
}
