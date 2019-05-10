using System;

namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public class LowResult : AbstractResult
    {
        public override void GetAffiliation()
        {
            try
            {
                var midValue = (MaxValue + MinValue) / 2;

                if (CurrentValue == midValue)
                {
                    Affiliation = 100.00m;
                }

                (decimal, decimal) firstPoint;
                (decimal, decimal) secondPoint;

                if (CurrentValue > midValue)
                {
                    firstPoint = (midValue, 100.00m);
                    secondPoint = (MaxValue, 0.00m);

                    var k = (secondPoint.Item2 - firstPoint.Item2)
                            / (secondPoint.Item1 - firstPoint.Item1);
                    var b = firstPoint.Item2 - firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                            / (secondPoint.Item1 - firstPoint.Item1);

                    Affiliation = k * CurrentValue + b;
                }
                else
                {
                    Affiliation = 100m;
                }

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
