using System;
using System.Collections.Generic;
using System.Linq;

namespace Reed_Muler_Code.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Suskaiciuoja skaiciaus faktoriala
        /// </summary>
        /// <param name="number">Skaicius kurio faktorialas skaiciuojamas</param>
        /// <returns>Suskaiciuotas faktorialas</returns>
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
        /// <param name="m">Formules M parametras</param>
        /// <param name="r">Formules R parametras</param>
        /// <returns>Grazinamas k naudojamas vektoriaus ilgiui ir generuojancios matricus eiluciu skaiciui </returns>
        public static int CountCombination(this int m, int r)
        {
            int k = 0;

            for (int i = 0; i <= r; i++)
            {
                int unit = m.CountFactorial();
                int divider = i.CountFactorial() * (m - i).CountFactorial();
                k += unit / divider;
            }

            return k;
        }

        /// <summary>
        /// Pakelimas laipsniu teigiamu skaiciu
        /// </summary>
        /// <param name="number">Skaicius kuri kelsim laipsniu</param>
        /// <param name="power">Laipsnis kuriuo kelsim</param>
        /// <returns>Laipsninu pakeltas skaicius</returns>
        public static int CountPositivePow(this int number, int power)
        {
            int result = 1;
            for (int i = 1; i <= power; i++)
                result *= number;

            return result;
        }

        /// <summary>
        /// Pavercia skaiciu sarasa i kableliais atskirta simboliu eilute
        /// </summary>
        /// <param name="list">Skaiciu sarasas</param>
        /// <returns>Kableliais atskirta simboliu eilute</returns>
        public static string ListToCommaSeparatedString(this List<int> list)
        {
            List<string > stringList = list.Select(x => x.ToString()).ToList();
            return string.Join(", ", stringList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="partLength"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitInParts(this string s, int partLength)
        {
            if (s == null)
                throw new ArgumentNullException("String cannot be null");

            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        /// <summary>
        /// Skaiciu sarasa pavercia i simboliu eilute
        /// </summary>
        /// <param name="list">Skaiciu sarasas</param>
        /// <returns>Simboliu eilute</returns>
        public static string ListToString(this List<int> list) => list.Select(x => x.ToString()).ToList().Aggregate((x, y) => x + y);

        /// <summary>
        /// Skaiciu masyvas paverciamas i simboliu eilute
        /// </summary>
        /// <param name="array">Skaiciu masyvas</param>
        /// <returns>Simboliu eilute</returns>
        public static string ArrayToString(this int[] array) => string.Join("", array);

        /// <summary>
        /// Simboliu eiluciu sarasas paverciamas i simboliu eilute
        /// </summary>
        /// <param name="list">Simboliu eiluciu sarasas</param>
        /// <returns></returns>
        public static string StringListToString(this List<string> list) => string.Join("", list);

        /// <summary>
        /// Simboliu eilute paverciama i skaiciu sarasa
        /// </summary>
        /// <param name="str">Simboliu eilute</param>
        /// <returns>Skaiciu sarasas</returns>
        public static List<int> StringToIntList(this string str) => str.ToCharArray().Select(x => int.Parse(x.ToString())).ToList();

        /// <summary>
        /// Simboliu eilute paverciama i skaiciu masyva
        /// </summary>
        /// <param name="str">Simboliu eilute</param>
        /// <returns>Skaiciu masyvas</returns>
        public static int[] StringToIntArray(this string str) => str.Select(c => int.Parse(c.ToString())).ToArray();
    }
}
