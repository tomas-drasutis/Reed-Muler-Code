using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.Channels
{
    public class Channel
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Vektorius persiunciamas per triuksminga kanala.
        /// Atsitiktinai su kazkokia tikimeybe invertuojami bitai.
        /// </summary>
        /// <param name="vector">Vektorius kuris bus uzkoduojamas</param>
        /// <param name="mistakeProbability">Klaidos tikimybe</param>
        /// <returns>Per kanala persiustas vektorius</returns>
        public static Vector SendThroughNoisyChannel(Vector vector, double mistakeProbability) =>
            new Vector(vector.M, vector.R, vector.Words.Select(bit => _random.Next(0, 100) < mistakeProbability * 100 ? 1 - bit : bit).ToArray());


        /// <summary>
        /// Simboliu eilute persiunciama per triuksminga kanala
        /// Del greitesnio veikimo naudojamas StringBuilder
        /// </summary>
        /// <param name="message">Siunciama simboliu eilute</param>
        /// <param name="mistakeProbability">Klaidos tikimybe</param>
        /// <returns>Per kanala persiusta simboliu eilute</returns>
        public static string SendThroughNoisyChannel(string message, double mistakeProbability)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char character in message)
                stringBuilder.Append(_random.Next(0, 100) < mistakeProbability * 100
                    ? (1 - int.Parse(character.ToString())).ToString()
                    : character.ToString());

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Palyginami pradinis ir persiustas vektoriai ir surandamos klaidu pozicijos
        /// </summary>
        /// <param name="vector">Pradinis vektorius</param>
        /// <param name="vectorAfterChannel">Per kanala persiustas vektorius</param>
        /// <returns>Klaidu poziciju sarasas</returns>
        public static List<int> GetErrorPositions(Vector vector, Vector vectorAfterChannel)
        {
            List<int> positions = new List<int>();

            for (int i = 0; i < vector.Words.Length; i++)
                if (vector.Words[i] != vectorAfterChannel.Words[i])
                    positions.Add(i + 1);

            return positions;
        }
    }
}
