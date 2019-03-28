using System.Collections.Generic;

namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public class Rule
    {
        public int Id { get; set; }
        public string InputTerms { get; set; } //хранение в базе через разделитель, конвертация в список
        public string OutputTerms { get; set; } //todo - где конвертим?
    }
}
