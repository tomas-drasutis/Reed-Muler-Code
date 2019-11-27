using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reed_Muler_Code.Decoders
{
    public class Decoder
    {
        public static Vector Decode(Vector vector)
        {
            int m = vector.M;
            int r = vector.R;
            List<int> encodedVector = vector.Bits.ToList();

            List<int> decodedVector = GetVotes(encodedVector, m, r);

            decodedVector.Reverse();

            return new Vector(m, r, decodedVector.ToArray());
        }


        /// <summary>
        /// Gauna visus koeficientus kurie yra pradinis vektorius
        /// </summary>
        /// <param name="encodedVector"></param>
        /// <returns></returns>
        private static List<int> GetVotes(List<int> encodedVector, int m, int r)
        {
            int votesCounted = 0;
            List<string> bytes;
            List<List<int>> w;
            List<int> wt;
            List<int> finalVotes = new List<int>();

            int n = (int)Math.Pow(2, m);
            int rows = m.CountCombination(r);
            bytes = BytesCounter.GetBytes(n, m);
            int[][] generatorMatrix = Cache.GetGeneratorMatrix(rows, n);

            for (int i = r; i >= 0; i--)
            {
                if (i == 0)
                {
                    w = new List<List<int>>();
                    for (int h = 0; h < n; h++)
                    {
                        wt = new List<int>();
                        for (int j = 0; j < n - 1; j++)
                            wt.Add(0);

                        wt.Insert(h, 1);
                        w.Add(wt);
                    }

                    CalculateDominantVote(encodedVector, w, finalVotes);
                }

                else
                {
                    int positionsMissingCount = m - i;
                    var missingPositionsList = GetMissingPossitionsList(positionsMissingCount, bytes);

                    var tList = BytesCounter.GetBytes((int)Math.Pow(2, positionsMissingCount), positionsMissingCount);

                    foreach (List<int> missingPosition in missingPositionsList)
                    {
                        w = new List<List<int>>();
                        foreach (string t in tList)
                        {
                            wt = new List<int>();
                            foreach (string _byte in bytes)
                            {
                                bool value = false;
                                for (int pos = 0; pos < positionsMissingCount; pos++)
                                {
                                    if (_byte[missingPosition[pos]] == t[pos])
                                        value = true;
                                    else
                                    {
                                        value = false;
                                        break;
                                    }
                                }
                                wt.Add(value == true ? 1 :0);
                            }
                            w.Add(wt);
                        }
                        CalculateDominantVote(encodedVector, w, finalVotes);
                    }

                    if (finalVotes.Count != rows)
                    {
                        var a = SubtractFromMainVector(rows, n, encodedVector, generatorMatrix, finalVotes, votesCounted);
                        encodedVector = a.Item2;
                        votesCounted = a.Item1;
                    }
                       
                }
            }
            return finalVotes;
        }


        /// <summary>
        /// Sudaugina paketus su užkoduotu vektoriumi ir paskaičiuoja dominuojantį koeficientą.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="encodedVector"></param>
        private static void CalculateDominantVote(List<int> encodedVector, List<List<int>> w, List<int> finalVotes)
        {
            int vote = 0;
            var votes = new List<int>();

            for (int j = 0; j < w.Count; j++)
            {
                var wtPack = w[j];
                vote = wtPack.Select((x, i) => x * encodedVector[i]).Aggregate((a, b) => a + b);
                votes.Add(vote % 2);
            }

            var mostOccuredVote = votes.GroupBy(i => i)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key).First();

            finalVotes.Add(mostOccuredVote);
        }


        /// <summary>
        /// Sudaugina matricos eilutes su gautais koeficientais ir atima iš užkoduoto vektoriaus
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <param name="encodedVector"></param>
        /// <returns></returns>
        public static (int, List<int>) SubtractFromMainVector(int k, int n, List<int> encodedVector, int[][] generatorMatrix, List<int> finalVotes, int votesCounted)
        {
            List<int> vectorSum = new List<int>(new int[n]);
            int votesCount = finalVotes.Count;
            int rows = k - votesCount;

            for (int row = rows; row < k - votesCounted; row++)
            {
                int index = votesCount - (row - rows) - 1;
                List<int> newVector = new List<int>();
                for (int column = 0; column < n; column++)
                {
                    int bit = generatorMatrix[row][column] * finalVotes[index];
                    newVector.Add(bit);
                }

                vectorSum = AddVector(vectorSum, newVector);
            }

            List<int> vectorToReturn = SubtractVector(encodedVector, vectorSum);

            votesCounted = finalVotes.Count;
            return (votesCounted, vectorToReturn);
        }

        /// <summary>
        /// Atima vieną vektorių iš kito
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        private static List<int> SubtractVector(List<int> minuend, List<int> subtrahend)
        {
            List<int> difference = minuend.Select((m, i) => m - subtrahend[i]).ToList();
            return difference.Select(x => Convert.ToInt32(x < 0 || x == 1)).ToList();
        }


        /// <summary>
        /// Sudeda du vektorius
        /// </summary>
        /// <param name="firstAddend"></param>
        /// <param name="secondAddend"></param>
        /// <returns></returns>
        private static List<int> AddVector(List<int> firstAddend, List<int> secondAddend) => firstAddend.Select((add, index) => (add + secondAddend[index]) % 2).ToList();

        /// <summary>
        /// Gauna trūkstamas pozicijas atitinkamoms dimensijoms
        /// </summary>
        /// <param name="numberOfMissingPositions"></param>
        /// <returns></returns>
        private static List<List<int>> GetMissingPossitionsList(int numberOfMissingPositions, List<string> bytes)
        {
            List<List<int>> listOfPositions = new List<List<int>>();

            foreach (string _byte in bytes)
            {
                int zeros = _byte.Where(x => x != '1').Count();

                if (zeros == numberOfMissingPositions)
                {
                    List<int> positions = _byte.Select((character, index) => new { index, character })
                    .Where(t => t.character == '0')
                    .Select(t => t.index)
                    .ToList();

                    listOfPositions.Add(positions);
                }
            }

            return listOfPositions;
        }
    }
}
