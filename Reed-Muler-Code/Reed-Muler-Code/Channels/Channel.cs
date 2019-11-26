using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.Channels
{
    public static class Channel
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Sends a vector through the noisy channel
        /// </summary>
        /// <param name="vector">Vector to send</param>
        /// <param name="mistakeProbability">How likely is a mistake to happen</param>
        /// <returns>Vector from channel</returns>
        public static Vector SendThroughNoisyChannel(Vector vector, double mistakeProbability) =>
            new Vector(vector.M, vector.R, vector.Bits.Select(bit => _random.Next(0, 100) < mistakeProbability * 100 ? 1 - bit : bit).ToArray());

        public static string SendThroughNoisyChannel(string message, double mistakeProbability)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var character in message)
                stringBuilder.Append(_random.Next(0, 100) < mistakeProbability * 100
                    ? (1 - int.Parse(character.ToString())).ToString()
                    : character.ToString());

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets error positions of vectors before and after channel
        /// </summary>
        /// <param name="vector">Vector before channel</param>
        /// <param name="vectorAfterChannel">Vector after channel</param>
        /// <returns>Error positions</returns>
        public static List<int> GetErrorPositions(Vector vector, Vector vectorAfterChannel)
        {
            var positions = new List<int>();

            for (var i = 0; i < vector.Bits.Length; i++)
                if (vector.Bits[i] != vectorAfterChannel.Bits[i])
                    positions.Add(i + 1);

            return positions;
        }
    }
}
