using Reed_Muler_Code.Channels;
using Reed_Muler_Code.Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Encoder = Reed_Muler_Code.Encoders.Encoder;
using Decoder = Reed_Muler_Code.Decoders.Decoder;

namespace Reed_Muler_Code.Services
{
    public class VectorService
    {
        /// <summary>
        /// Encodes a vector
        /// </summary>
        /// <param name="vector">Vector bits to encode</param>
        /// <param name="m">Parameter m value</param>
        /// <returns>Encoded vector</returns>
        public Vector EncodeVector(int[] vectorBits, int r, int m)
        {
            Vector vector = new Vector(m, r ,vectorBits);
            return Encoder.Encode(vector);
        }

        /// <summary>
        /// Sends vector through a noisy channel
        /// </summary>
        /// <param name="vector">Vector that will be sent through the channel</param>
        /// <param name="mistakeProbability">Mistake probability</param>
        /// <returns>Vector from channel and list of error positions</returns>
        public (Vector, List<int>) SendThroughChannel(Vector vector, double mistakeProbability)
        {
            var vectorFromChannel = Channel.SendThroughNoisyChannel(vector, mistakeProbability);
            var errors = Channel.GetErrorPositions(vector, vectorFromChannel);

            return (vectorFromChannel, errors);
        }

        /// <summary>
        /// Decodes a vector
        /// </summary>
        /// <param name="vector">Vector to decode</param>
        /// <returns>Decoded vector</returns>
        public Vector DecodeVector(Vector vector)
        {
            return Decoder.Decode(vector);
        }
    }
}
