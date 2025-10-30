using System;

namespace HospitalManageSystem.Utilities
{
    public  class IdGenerator
    {
        private static readonly Random _random = new Random();

        // Generates a random ID with 5-8 number of digits.
        public static int GenerateId()
        {
            return GenerateId(5, 8);
        }

        // Random instance for generating random numbers.
        public static int GenerateId(int minDigits, int maxDigits)
        {
            if (minDigits < 1 || maxDigits < minDigits) minDigits = 5;
            int digits = _random.Next(minDigits, maxDigits + 1);
            int min = (int)Math.Pow(10, digits - 1);
            int max = (int)Math.Pow(10, digits) - 1;
            return _random.Next(min, max + 1);
        }


        public static int GenerateUniqueId(Func<int, bool> isUnique)
        {
            int id;
            int guard = 0;
            do
            {
                id = GenerateId();
                guard ++;
                if (guard > 1000)
                {
                    throw new Exception("Failed to generate a unique ID after maximum attempts.");
                }
            } while (!isUnique(id));
            return id;
        }
    }
}
