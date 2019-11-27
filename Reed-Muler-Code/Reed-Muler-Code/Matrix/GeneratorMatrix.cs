using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reed_Muler_Code.Matrix
{
    public class GeneratorMatrix
    {
        /// <summary>
        /// Generuoja matricą
        /// </summary>
        /// <returns></returns>
        public static int[][] GenerateMatrix(int m, int r)
        {
            var n = (int)Math.Pow(2, m);
            int rows = m.CountCombination(r);
            int columns = n;

            int[][] generatorMatrix = Cache.GetGeneratorMatrix(rows, columns);

            if (generatorMatrix != null)
                return generatorMatrix;

            generatorMatrix = new int[rows][];
            List<List<int>> combinations = new List<List<int>>();

            List<string> bytes = BytesCounter.GetBytes(n, m);

            combinations = GetCombinations(bytes, columns, r);

            for (int i = 0; i < rows; i++)
            {
                generatorMatrix[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    if (i == 0)
                        generatorMatrix[i][j] = 1;
                    else
                    {
                        if (combinations[i].Count == 1)
                        {
                            int index = combinations[i].FirstOrDefault();

                            if (bytes[j].ElementAtOrDefault(index - 1) == '0')
                                generatorMatrix[index][j] = 1;
                            else
                                generatorMatrix[index][j] = 0;
                        }
                        else
                        {
                            int multiplication = 1;

                            foreach (int index in combinations[i])
                                multiplication *= generatorMatrix[index][j];

                            generatorMatrix[i][j] = multiplication;
                        }
                    }
                }
            }

            Cache.AddGeneratorMatrix(rows, columns, generatorMatrix);
            return generatorMatrix;
        }

        //Gauna visus derinius iš baitų(pvz, 000, 001, 010 ir t.t.).   
        private static List<List<int>> GetCombinations(List<string> bytes, int columns, int r)
        {
            List<List<int>> unorderedCombinations = new List<List<int>>();
            for (int j = 0; j < columns; j++)
            {
                List<int> list = new List<int>();
                List<int> combination = bytes[j].Select((character, index) => new { index, character })
                    .Where(t => t.character == '1')
                    .Select(t => t.index + 1)
                    .ToList();

                if (combination.Count == 0)
                    combination.Add(0);

                if (combination.Count <= r)
                    unorderedCombinations.Add(combination);
            }

            return OrderCombinations(unorderedCombinations.OrderBy(x => x.Count).ToList());
        }


        /// <summary>
        /// Surikiuoja derinius ir paruošia matricai.
        /// </summary>
        /// <param name="orderedCombinations"></param>
        /// <returns></returns>
        private static List<List<int>> OrderCombinations(List<List<int>> orderedCombinations)
        {
            Dictionary<string, List<int>> combinationsWithKeys = new Dictionary<string, List<int>>();

            List<int> listsLengths = new List<int>();

            int maxCount = orderedCombinations.Max(x => x.Count);
            for (int i = 0; i <= maxCount; i++)
            {
                List<List<int>> currentListOfCombinations = orderedCombinations.Where(x => x.Count == i).ToList();
                foreach (List<int> combination in currentListOfCombinations)
                {
                    string key = combination
                        .Select(number => number.ToString())
                        .Aggregate((a, b) => a + b).PadLeft(maxCount, '0');

                    if (!combinationsWithKeys.ContainsKey(key))
                        combinationsWithKeys.Add(key, combination);
                }
            }

            return combinationsWithKeys.OrderBy(x => x.Key).Select(y => y.Value).ToList();
        }
    }
}
