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
        /// Uzkoduoja atsiusta vektoriu
        /// </summary>
        /// <param name="vector">Vektorius kuris bus uzkoduojamas</param>
        /// <returns>Uzkoduotas vektorius</returns>
        public Vector EncodeVector(Vector vector)
        {
            return Encoder.Encode(vector);
        }

        /// <summary>
        /// Persiuncia vektoriu per triuksminga kanala
        /// </summary>
        /// <param name="vector">Vektorius kuris bus siunciamas per kanala</param>
        /// <param name="mistakeProbability">Klaidos tikimybe</param>
        /// <returns>Persiustas per kanala vektorius ir pozicijos kuriose ivyko klaidos</returns>
        public (Vector, List<int>) SendThroughChannel(Vector vector, double mistakeProbability)
        {
            var vectorFromChannel = Channel.SendThroughNoisyChannel(vector, mistakeProbability);
            var errors = Channel.GetErrorPositions(vector, vectorFromChannel);

            return (vectorFromChannel, errors);
        }

        /// <summary>
        /// Dekoduoja vektoriu
        /// </summary>
        /// <param name="vector">Dekoduojamas vektorius</param>
        /// <returns>Dekoduotas vektorius</returns>
        public Vector DecodeVector(Vector vector) => Decoder.Decode(vector);
    }
}
