using System;

namespace FuzzyLogicMedicalCore.BL.FuzzyLogic
{
    public class MidResult : AbstractResult
    {
        public MidResult(decimal currentValue)
        {
            CurrentValue = currentValue;
        }

        public override void GetAffiliation()
        {
            try
            {
                var midValue = (MaxValue + MinValue) / 2;

                if (CurrentValue == midValue)
                {
                    Affiliation = 100.00m;
                    return;
                }

                (decimal, decimal) firstPoint;
                (decimal, decimal) secondPoint;

                if (CurrentValue > midValue)
                {
                    firstPoint = (midValue, 100.00m);
                    secondPoint = (MaxValue, 0.00m);
                }
                else
                {
                    firstPoint = (MinValue, 0.00m);
                    secondPoint = (midValue, 100.00m);
                }

                var k = (secondPoint.Item2 - firstPoint.Item2) / (secondPoint.Item1 - firstPoint.Item1);
                var b = firstPoint.Item2 - (firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                                            / (secondPoint.Item1 - firstPoint.Item1));

                Affiliation = k * CurrentValue + b;

                if (Affiliation < 0)
                {
                    Affiliation = 0;
                }
            }
            catch (Exception)
            {
                //skip
            }
        }
    }
}
