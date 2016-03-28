namespace VilRan.Mathematics
{
    public static class MathUtility
    {
        /// <summary>
        /// Wraps a value between 0 and a desired maximum.
        /// </summary>
        /// <param name="n">The value to be wrapped.</param>
        /// <param name="max">The exclusive upper bound for the returned value.</param>
        /// <returns></returns>
        public static int Wrap(int n, int max)
        {
            if (n < 0)
                return max + n % max;
            return n % max;
        }

        /// <summary>
        /// Wraps a value between 0 and a desired maximum.
        /// </summary>
        /// <param name="n">The value to be wrapped.</param>
        /// <param name="max">The exclusive upper bound for the returned value.</param>
        /// <returns></returns>
        public static float Wrap(float n, float max)
        {
            if (n < 0)
                return max + n % max;
            return n % max;
        }

        /// <summary>
        /// Reduces the absolute value of a number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static float Reduce(float value, float subtrahend)
        {
            if (value - subtrahend > 0)
                return value - subtrahend;
            else if (value + subtrahend < 0)
                return value + subtrahend;
            return 0;
        }
    }
}
