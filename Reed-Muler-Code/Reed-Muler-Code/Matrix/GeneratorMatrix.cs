using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reed_Muler_Code.Matrix
{
    public class GeneratorMatrix
    {
        /// <summary>
        ///  Sukuria generuojancia matrica pagal m ir r parametrus
        /// </summary>
        /// <param name="m">Parametras naudojamas RM kodavime</param>
        /// <param name="r">Parametras naudojamas RM kodavime</param>
        /// <returns>Generuojanti matrica</returns>
        public static int[][] Generate(int m, int r)
        {
            int n = 2.CountPositivePow(m);
            int rowCount = m.CountCombination(r);
            int columnCount = n;

            int[][] generatorMatrix = Cache.GetGeneratorMatrix(rowCount, columnCount);

            if (generatorMatrix != null)
                return generatorMatrix;

            generatorMatrix = new int[rowCount][];

            List<string> bytes = BytesCounter.GetBytes(n, m);
            List<List<int>> combinations = GetRowCombinations(bytes, columnCount, r);

            for (int i = 0; i < rowCount; i++)
            {
                generatorMatrix[i] = new int[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    if (i == 0)
                        generatorMatrix[i][j] = 1;
                    else
                    {
                        // Jei tik viena kombinacija, pagal indexa patikriname baito reiksme
                        // atitinkamai i generuojancia matrica irasome 1 arba 0
                        if (combinations[i].Count == 1)
                        {
                            int index = combinations[i].FirstOrDefault();

                            generatorMatrix[index][j] = bytes[j].ElementAtOrDefault(index - 1) == '0' ? 1 : 0;
                        }
                        // Kitu atveju einame per kombinacijas ir sudauginame jau anksciau irasytas reiksmes
                        else
                        {
                            int result = 1;

                            foreach (int index in combinations[i])
                                result *= generatorMatrix[index][j];

                            generatorMatrix[i][j] = result;
                        }
                    }
                }
            }

            Cache.AddGeneratorMatrix(rowCount, columnCount, generatorMatrix);
            return generatorMatrix;
        }

        /// <summary>
        /// Gauna visas eiluciu kombinacijas is baitu reiksmiu (Pvz.: Kai M=3, o R=2: v1, v2, v3, v1 v2, v1 v3, v2 v3)
        /// </summary>
        /// <param name="bytes">Baitu sarasas (Pvz.: 000, 001, 010, 011, 100, 101, 110, 111)</param>
        /// <param name="columnCount">Stulpeliu skaicius</param>
        /// <param name="r">Parametras naudojamas RM kodavime</param>
        /// <returns>Grazina eiluciu kombinaciju sarasa</returns>
        private static List<List<int>> GetRowCombinations(List<string> bytes, int columnCount, int r)
        {
            List<List<int>> unorderedCombinations = new List<List<int>>();
            for (int j = 0; j < columnCount; j++)
            {
                //Imamas indeksas tu poziciju kuriose yra vienetai
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
        /// Isrikiuoja eiluciu kombinaciju sarasa
        /// </summary>
        /// <param name="orderedCombinations">Neisrikiuotos kombinacijos</param>
        /// <returns>Isrikiuotas eiluciu kombinaciju sarasas</returns>
        private static List<List<int>> OrderCombinations(List<List<int>> orderedCombinations)
        {
            int maxCombinationsCount = orderedCombinations.Max(x => x.Count);
            Dictionary<string, List<int>> combinationsWithKeys = new Dictionary<string, List<int>>();
            
            for (int i = 0; i <= maxCombinationsCount; i++)
            {
                List<List<int>> currentListOfCombinations = orderedCombinations.Where(x => x.Count == i).ToList();
                foreach (List<int> combination in currentListOfCombinations)
                {
                    // Suformuojamas raktas pagal kombinacijas, raktas turi tiek simboliu kiek yra daugiausiai kombinaciju
                    string key = combination
                        .Select(number => number.ToString())
                        .Aggregate((a, b) => a + b).PadLeft(maxCombinationsCount, '0');

                    if (!combinationsWithKeys.ContainsKey(key))
                        combinationsWithKeys.Add(key, combination);
                }
            }

            // Isrikiuojama pagal rakta
            return combinationsWithKeys.OrderBy(x => x.Key).Select(y => y.Value).ToList();
        }
    }
}
