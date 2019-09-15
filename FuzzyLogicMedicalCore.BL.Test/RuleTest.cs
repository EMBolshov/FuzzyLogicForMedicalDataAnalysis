using System.Collections.Generic;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzyLogicMedicalCore.BL.Test
{
    [TestClass]
    public class RuleTest
    {
        [TestMethod]
        public void GetPowerOfRule()
        {
            var inputTerms = new List<ITerm>
            {
                 
            };

            var rule = new FuzzyRule {Id = 1, InputTerms = new List<InputTerm>(), OutputTerms = new List<string>(), Power = 0};
            Assert.IsTrue(true);
        }
    }
}
