using System;

namespace IndependentWork12
{
    public static class MathOperations
    {
        public static bool IsPrime(int number)
        {
            if (number < 2)
            {
                return false;
            }

            if (number == 2)
            {
                return true;
            }

            if (number % 2 == 0)
            {
                return false;
            }

            int boundary = (int)Math.Sqrt(number);
            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static double HeavyTransform(int number)
        {
            double result = number;
            for (int i = 1; i <= 25; i++)
            {
                result = Math.Sqrt(result * result + i) + Math.Sin(i) * Math.Cos(number % 360);
            }

            return result;
        }

        public static bool IsEven(int number)
        {
            return number % 2 == 0;
        }
    }
}