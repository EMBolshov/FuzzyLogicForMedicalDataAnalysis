namespace RealDataGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var generator = new RealDataFileGenerator();
            generator.MakeDiagnoses();
            generator.MakeRules();
            var patients = generator.MakePatients(2000);
            foreach (var patient in patients)
            {
                generator.GenerateAnalyzesWithReferencesAndResults(patient.Guid);
            }
        }
    }
}
