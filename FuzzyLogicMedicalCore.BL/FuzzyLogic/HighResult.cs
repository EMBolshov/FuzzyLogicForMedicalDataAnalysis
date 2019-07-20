using System;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class HighResult : AbstractResult
    {
        public HighResult(decimal currentValue)
        {
            CurrentValue = currentValue;
        }

        public override void GetAffiliation()
        {
            try
            {
                if (CurrentValue >= MaxValue)
                {
                    Affiliation = 100m;
                    return;
                }

                var firstPoint = (MinValue, 0m);
                var secondPoint = (MaxValue, 100m);

                var k = (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);
                var b = firstPoint.Item2 - firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);

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
