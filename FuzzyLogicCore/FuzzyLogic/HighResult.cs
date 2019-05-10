namespace FuzzyLogicMedicalCore.FuzzyLogic
{
    public class HighResult : AbstractResult
    {
        public override void GetAffiliation()
        {
            var midValue = (MaxValue + MinValue) / 2;

            if (CurrentValue == midValue)
            {
                Affiliation = 100m;
            }

            (decimal, decimal) firstPoint;
            (decimal, decimal) secondPoint;

            if (CurrentValue > midValue)
            {
                Affiliation = 100m;
            }
            else
            {
                firstPoint = (MinValue, 0m);
                secondPoint = (midValue, 100m);

                var k = (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);
                var b = firstPoint.Item2 - firstPoint.Item1 * (secondPoint.Item2 - firstPoint.Item2)
                        / (secondPoint.Item1 - firstPoint.Item1);

                Affiliation = k * CurrentValue + b;
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
    }
}
