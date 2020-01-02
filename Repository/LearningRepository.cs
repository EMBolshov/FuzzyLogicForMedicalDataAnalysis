namespace Repository
{
    public class LearningRepository : ILearningRepository
    {
        private static string _connectionString;

        public LearningRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
