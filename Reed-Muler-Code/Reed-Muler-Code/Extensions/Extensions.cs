using System;
using System.Collections.Generic;
using System.Linq;

namespace Reed_Muler_Code.Extensions
{
    public static class Extensions
    {
        public static int CountFactorial(this int number)
        {
            int result = 1;

            for (int i = 1; i <= number; i++)
                result *= i;

            return result;
        }

        /// <summary>
        /// Suskaičiuoja derinių skaičių pagal formulę
        /// </summary>
        /// <param name="m"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static int CountCombination(this int m, int r)
        {
            int count = 0;

            for (int i = 0; i <= r; i++)
            {
                int unit = m.CountFactorial();
                int divider = i.CountFactorial() * (m - i).CountFactorial();
                count += unit / divider;
            }

            return count;
        }

        public static string ListToCommaSeparatedString(this List<int> list)
        {
            List<string > stringList = list.Select(x => x.ToString()).ToList();
            return string.Join(", ", stringList);
        }

        public static IEnumerable<string> SplitInParts(this string s, int partLength)
        {
            if (s == null)
                throw new ArgumentNullException("String cannot be null");

            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public static string ListToString(this List<int> list) => list.Select(x => x.ToString()).ToList().Aggregate((x, y) => x + y);
        public static string ArrayToString(this int[] array) => string.Join("", array);
        public static string StringListToString(this List<string> list) => string.Join("", list);
        public static List<int> StringToIntList(this string str) => str.ToCharArray().Select(x => int.Parse(x.ToString())).ToList();
        public static int[] StringToIntArray(this string str) => str.Select(c => int.Parse(c.ToString())).ToArray();
    }
}
