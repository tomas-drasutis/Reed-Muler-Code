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
        /// Uzkoduoja vektoriu nusiusdama ji ir atitinkama generuojancia matrica i daugybos funkcija
        /// Issaugo uzkoduota vektoriu i Cache
        /// </summary>
        /// <param name="vector">Vektorius kuris bus uzkoduojamas</param>
        /// <param name="r">R parametras naudojamas RM kode</param>
        /// <param name="m">M parametras naudojamas RM kode</param>
        /// <returns>Uzkoduotas vektorius</returns>
        public static Vector Encode(Vector vector)
        {
            int[] encodedWords = Cache.GetEncodedVector(vector.M, vector.R, vector.Words);

            if (encodedWords == null)
                encodedWords = MatrixMultiplicator.MatrixMultiplicator.MultiplyByGeneratorMatrix(vector.Words, GeneratorMatrix.Generate(vector.M, vector.R));

            Cache.AddEncodedVector(vector.M, vector.R, vector.Words, encodedWords);
            return new Vector(vector.M, vector.R, encodedWords);
        }
    }
}
