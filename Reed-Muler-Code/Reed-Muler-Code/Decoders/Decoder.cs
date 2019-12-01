using Reed_Muler_Code.Extensions;
using Reed_Muler_Code.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reed_Muler_Code.Decoders
{
    public class Decoder
    {
        /// <summary>
        /// Pasiima reiksmes is at siusto vektoriaus objekto ir kreipiasi i dekodavima
        /// </summary>
        /// <param name="vector">Vektoriaus objektas su uzkoduotu ir per kanala persiustu vektoriumi</param>
        /// <returns>Vektoriaus objektas su dekoduotu vektoriumi</returns>
        public static Vector Decode(Vector vector)
        {
            int m = vector.M;
            int r = vector.R;
            List<int> encodedVector = vector.Bits.ToList();

            List<int> decodedVector = Decode(encodedVector, m, r);

            decodedVector.Reverse();

            return new Vector(m, r, decodedVector.ToArray());
        }


        /// <summary>
        /// Dekoduoja atsiusta vektoriu
        /// </summary>
        /// <param name="encodedVector">Uzkuoduotas vektorius</param>
        /// <param name="m">M prametras naudojamas RM kode</param>
        /// <param name="r">R parametras naudojamas RM kode</param>
        /// <returns>Dekoduotas vektorius</returns>
        private static List<int> Decode(List<int> encodedVector, int m, int r)
        {
            int votesCounted = 0;
            List<List<int>> w;
            List<int> wt;
            List<int> decodedVector = new List<int>();

            int n = 2.CountPositivePow(m);
            int rows = m.CountCombination(r);
            int[][] generatorMatrix = GeneratorMatrix.Generate(m, r);

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

                    CalculateDominantVote(encodedVector, w, decodedVector);
                }
                else
                {
                    FindVotes(n, m, i, decodedVector, encodedVector);

                    if (decodedVector.Count != rows)
                    {
                        (int, List<int>) subtractedVector = SubtractFromMainVector(rows, n, encodedVector, generatorMatrix, decodedVector, votesCounted);
                        votesCounted = subtractedVector.Item1;
                        encodedVector = subtractedVector.Item2;
                    }
                }
            }
            return decodedVector;
        }

        /// <summary>
        /// Suranda w sarasui priklausancias reiksmes ir nusiuncia i balsu skaiciavimo funkcija
        /// </summary>
        /// <param name="n">Vektoriaus ilgis</param>
        /// <param name="m">M parametras naudojamas RM kode</param>
        /// <param name="i">R parametras atemus atitinkama skaiciu iteraciju</param>
        /// <param name="decodedVector"></param>
        /// <param name="encodedVector"></param>
        private static void FindVotes(int n, int m, int i, List<int> decodedVector, List<int> encodedVector)
        {
            List<int> wt;
            List<List<int>> w;

            int missingPositionsCount = m - i;

            List<string> bytes = BytesCounter.GetBytes(n, m);
            List<List<int>> missingPositionsList = GetZeroPossitionsList(missingPositionsCount, bytes);
            List<string> tList = BytesCounter.GetBytes(2.CountPositivePow(missingPositionsCount), missingPositionsCount);

            foreach (List<int> missingPosition in missingPositionsList)
            {
                w = new List<List<int>>();
                foreach (string t in tList)
                {
                    wt = new List<int>();
                    foreach (string _byte in bytes)
                    {
                        bool value = false;
                        for (int position = 0; position < missingPositionsCount; position++)
                        {
                            if (_byte[missingPosition[position]] == t[position])
                                value = true;
                            else
                            {
                                value = false;
                                break;
                            }
                        }
                        wt.Add(value == true ? 1 : 0);
                    }
                    w.Add(wt);
                }
                CalculateDominantVote(encodedVector, w, decodedVector);
            }
        }

        /// <summary>
        /// Sudaugina w reiksmes su uzkoduotu vektoriumi ir paskaiciuoja dominuojanti balsa.
        /// Ta balsa prideda prie dekoduoto vektoriaus
        /// </summary>
        /// <param name="encodedVector">Uzkoduotas vektorius</param>
        /// <param name="w">Vektoriu sarasas gautu is t baitu saraso</param>
        /// <param name="decodedVector">Dekoduotas vektorius</param>
        private static void CalculateDominantVote(List<int> encodedVector, List<List<int>> w, List<int> decodedVector)
        {
            int vote = 0;
            List<int> votes = new List<int>();

            for (int j = 0; j < w.Count; j++)
            {
                List<int> wtPack = w[j];
                vote = wtPack.Select((x, i) => x * encodedVector[i]).Aggregate((a, b) => a + b);
                votes.Add(vote % 2);
            }

            int mostOccuredVote = votes.GroupBy(i => i)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key).First();

            decodedVector.Add(mostOccuredVote);
        }

        /// <summary>
        /// Rastus vektoriaus zodzius sudaugina su generuojancios matricos vektoriais, juos susumuoja ir rezultata atima is uzkoduoto vektoriaus,
        /// taip gauname RM(m, r-1)
        /// </summary>
        /// <param name="k">Generuojancios matricos eiluciu skaicius/Vektoriaus ilgis</param>
        /// <param name="n">Generuojancios matricos stulpeliu skaicius/Uzkoduoto vektoriaus ilgis</param>
        /// <param name="encodedVector">Uzkoduotas vektorius</param>
        /// <param name="generatorMatrix">Generuojanti matrica</param>
        /// <param name="decodedVector">Dekoduotas vektorius</param>
        /// <param name="votesCounted">Suskaiciuotu balsu kiekis</param>
        /// <returns>Atnaujintas suskaiciuotu balsu skaicius ir uzkoduotas vektorius is kurio atimta vectorSum reiksme</returns>
        public static (int, List<int>) SubtractFromMainVector(int k, int n, List<int> encodedVector, int[][] generatorMatrix, List<int> decodedVector, int votesCounted)
        {
            List<int> vectorSum = new List<int>(new int[n]);
            int votesCount = decodedVector.Count;
            int rows = k - votesCount;

            for (int row = rows; row < k - votesCounted; row++)
            {
                int index = votesCount - (row - rows) - 1;
                List<int> newVector = new List<int>();
                for (int column = 0; column < n; column++)
                {
                    int bit = generatorMatrix[row][column] * decodedVector[index];
                    newVector.Add(bit);
                }

                vectorSum = AddVector(vectorSum, newVector);
            }

            List<int> vectorToReturn = SubtractVector(encodedVector, vectorSum);

            votesCounted = decodedVector.Count;
            return (votesCounted, vectorToReturn);
        }

        /// <summary>
        /// Vektoriu atimtis
        /// </summary>
        /// <param name="minuend">Vektorius is kurio atimama</param>
        /// <param name="subtrahend">Vektorius kuris atimamas</param>
        /// <returns>Vektoriu atimties rezultatas</returns>
        private static List<int> SubtractVector(List<int> minuend, List<int> subtrahend)
        {
            List<int> difference = minuend.Select((m, i) => m - subtrahend[i]).ToList();
            return difference.Select(x => Convert.ToInt32(x < 0 || x == 1)).ToList();
        }

        /// <summary>
        /// Vektoriu sudetis
        /// </summary>
        /// <param name="firstAddend"></param>
        /// <param name="secondAddend"></param>
        /// <returns></returns>
        private static List<int> AddVector(List<int> firstAddend, List<int> secondAddend) => firstAddend.Select((add, index) => (add + secondAddend[index]) % 2).ToList();

       /// <summary>
       /// Suranda baituose nulines pozicijas
       /// </summary>
       /// <param name="numberOfMissingPositions">Nuliniu poziciju skaiciu</param>
       /// <param name="bytes">Baitu kuriuose tikrinamos tuscios pozicijos sarasas</param>
       /// <returns>Nuliniu poziciju sarasas</returns>
        private static List<List<int>> GetZeroPossitionsList(int numberOfZeroPositions, List<string> bytes)
        {
            List<List<int>> listOfPositions = new List<List<int>>();

            foreach (string _byte in bytes)
            {
                int zeros = _byte.Where(x => x != '1').Count();

                if (zeros == numberOfZeroPositions)
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
