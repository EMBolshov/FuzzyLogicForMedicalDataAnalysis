using System.Transactions;
using Repository;

namespace UnitTests.MocksAndStubs
{
    public class FakeMainProcessingRepository : IMainProcessingRepository
    {
        //TODO 
        public void CreateNewDiagnosis(string diagnosisName)
        {
            using (var scope = new TransactionScope())
            {
                var sql = "INSERT INTO Diagnosis (Name) " +
                          $"VALUES ('{diagnosisName}')";
                
            }
        }
    }
}
