using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reed_Muler_Code.Matrix
{
    public class GeneratorMatrix
    {
        private int _columns;
        private int _rows;
        private List<List<int>> _combinations;
        public int[][] Matrix;
        private int _m;
        private int _r;

        public GeneratorMatrix(int m, int r)
        {
            _m = m;
            _r = r;
            _combinations = new List<List<int>>();
            GenerateMatrix();
        }

        /// <summary>
        /// Generuoja matricą
        /// </summary>
        /// <returns></returns>
        public int[][] GenerateMatrix()
        {
            int[][] generatorMatrix = Cache.GetGeneratorMatrix(_m, _r);

            if (generatorMatrix != null)
                return generatorMatrix;

            var n = (int)Math.Pow(2, _m);
            _rows = _m.CountCombination(_r);
            _columns = n;
            Matrix = new int[_rows][];
            var bytes = BytesCounter.GetBytes(n, _m);

            GetCombinations(bytes);

            for (int i = 0; i < _rows; i++)
            {
                Matrix[i] = new int[_columns];
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 0)
                    {
                        Matrix[i][j] = 1;
                    }
                    else
                    {
                        if (_combinations[i].Count == 1)
                        {
                            var index = _combinations[i].FirstOrDefault();

                            if (bytes[j].ElementAtOrDefault(index - 1) == '0')
                                Matrix[index][j] = 1;
                            else
                                Matrix[index][j] = 0;
                        }
                        else
                        {
                            var multiplication = 1;

                            foreach (var index in _combinations[i])
                                multiplication *= Matrix[index][j];

                            Matrix[i][j] = multiplication;
                        }
                    }
                }
            }

            Cache.AddGeneratorMatrix(_m, _r, Matrix);
            return Matrix;
        }

        //Gauna visus derinius iš baitų(pvz, 000, 001, 010 ir t.t.).   
        private void GetCombinations(List<string> bytes)
        {
            var unorderedCombinations = new List<List<int>>();
            for (int j = 0; j < _columns; j++)
            {
                var list = new List<int>();
                var combination = bytes[j].Select((character, index) => new { index, character })
                    .Where(t => t.character == '1')
                    .Select(t => t.index + 1)
                    .ToList();

                if (combination.Count == 0)
                    combination.Add(0);

                if (combination.Count <= _r)
                    unorderedCombinations.Add(combination);
            }

            var orderedCombinations = unorderedCombinations.OrderBy(x => x.Count).ToList();
            _combinations = OrderCombinations(orderedCombinations);
        }


        /// <summary>
        /// Surikiuoja derinius ir paruošia matricai.
        /// </summary>
        /// <param name="orderedCombinations"></param>
        /// <returns></returns>
        private List<List<int>> OrderCombinations(List<List<int>> orderedCombinations)
        {
            var combinationsWithKeys = new Dictionary<string, List<int>>();

            var listsLengths = new List<int>();

            var maxCount = orderedCombinations.Max(x => x.Count);
            for (var i = 0; i <= maxCount; i++)
            {
                var currentListOfCombinations = orderedCombinations.Where(x => x.Count == i).ToList();
                foreach (var combination in currentListOfCombinations)
                {
                    var key = combination
                        .Select(number => number.ToString())
                        .Aggregate((a, b) => a + b).PadLeft(maxCount, '0');

                    if (!combinationsWithKeys.ContainsKey(key))
                        combinationsWithKeys.Add(key, combination);
                }
            }

            var orderedCombinationsWithKeys = combinationsWithKeys.OrderBy(x => x.Key);
            return orderedCombinationsWithKeys.Select(x => x.Value).ToList();
        }
    }
}
