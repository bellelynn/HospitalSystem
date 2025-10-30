using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManageSystem.Utilities
{
    public static class Extensions
    {
        // Extension method to check if a string is null, empty, or consists only of whitespace characters.
        public static bool IsNullOrTrimEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        // Extension method to return a fallback string if the original string is null or empty.
        //Given a safe fallback value, it ensures that the returned string is never null or empty.
        public static string NullSafe(this string value, string fallback = "")
        {
            return string.IsNullOrEmpty(value) ? fallback : value;
        }

        // Extension method to return an empty enumerable if the source is null.
        // This prevents null reference exceptions when iterating over collections.
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
        {
            // Extension method to return an empty enumerable if the source is null.
            return source ?? Enumerable.Empty<T>();
        }

        // Extension method to check if an integer value is within a specified range (inclusive).
        public static bool InRange<T>(this int value, int min, int max)
        {
            // Extension method to check if a value is within a specified range (inclusive).
            return value >= min && value <= max;

        }

        public static void PrintList<T>(this IEnumerable<T> items, string header)
        {
            // Extension method to print a list with a header and a specified action for each item.
            Console.WriteLine(header);
            foreach (var item in items.OrEmpty())
            {
                Console.WriteLine(item);
            }
        }
    }
}