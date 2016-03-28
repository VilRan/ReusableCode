using System;

namespace VilRan.Mathematics
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return minValue + random.NextDouble() * (maxValue - minValue);
        }

        public static double NextGaussian(this Random random, double mean, double deviation)
        {
            double rn1 = random.NextDouble();
            double rn2 = random.NextDouble();
            double transform = Math.Sqrt(-2.0 * Math.Log(rn1)) * Math.Sin(2.0 * Math.PI * rn2);
            double result = mean + deviation * transform;
            return result;
        }
    }
}
