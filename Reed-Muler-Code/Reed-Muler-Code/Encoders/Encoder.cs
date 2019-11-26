using Reed_Muler_Code.Matrix;
using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.Encoders
{
    public class Encoder
    {
        /// <summary>
        /// Encodes array of bits
        /// </summary>
        /// <param name="vector">Vector of bits that we want to encode</param>
        /// <param name="r">Parameter r value</param>
        /// <param name="m">Parameter m value</param>
        /// <returns>Encoded bits</returns>
        public static Vector Encode(Vector vector)
        {
            int[] encodedBits = Cache.GetEncodedVector(vector.M, vector.R, vector.Bits);

            if (encodedBits == null)
                encodedBits = MatrixMultiplicator.MatrixMultiplicator.MultiplyByGeneratorMatrix(vector.Bits, new GeneratorMatrix(vector.M, vector.R).GenerateMatrix());

            return new Vector(vector.M, vector.R, encodedBits);
        }
    }
}
