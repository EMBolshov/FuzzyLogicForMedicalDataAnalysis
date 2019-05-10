namespace FakeDataGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var generator = new FakeDataFileGenerator();
            generator.MakeDiagnoses();
            generator.MakeRules();
            var patients = generator.MakePatients(1000);
            foreach (var patient in patients)
            {
                generator.MakeAnalysis(patient.Guid);
            }
        }
    }
}
