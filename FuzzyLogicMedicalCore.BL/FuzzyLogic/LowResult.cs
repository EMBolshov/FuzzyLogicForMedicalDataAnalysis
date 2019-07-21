using System;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class LowResult : AbstractResult
    {
        public LowResult(decimal currentValue)
        {
            CurrentValue = currentValue;
        }
       
        public override void GetAffiliation()
        {
            try
            {
                if (CurrentValue <= MinValue)
                {
                    Affiliation = 100.00m;
                    return;
                }

                var firstPoint = (MinValue, 100.00m);
                var secondPoint = (MaxValue, 0.00m);

                var k = (secondPoint.Item2 - firstPoint.Item2) / (secondPoint.Item1 - firstPoint.Item1);
                var b = firstPoint.Item2 - (firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2) 
                                                               /(secondPoint.Item1 - firstPoint.Item1));

                Affiliation = k * CurrentValue + b;
                

                if (Affiliation < 0m)
                {
                    Affiliation = 0m;
                }

                if (Affiliation > 100m)
                {
                    Affiliation = 100m;
                }
            }
            catch (Exception)
            {
                //skip
            }
        }
    }
}
