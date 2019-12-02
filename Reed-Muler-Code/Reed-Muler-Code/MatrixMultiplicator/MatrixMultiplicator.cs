using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.MatrixMultiplicator
{
    public class MatrixMultiplicator
    {
        /// <summary>
        /// Sudaugina vektoriu su generuojancia matrica
        /// </summary>
        /// <param name="vector">Vektorius kuris bus sudauginamas</param>
        /// <param name="generatorMatrix">Generuojanti matrica kuri bus sudauginama</param>
        /// <returns>Vektorius gautas sudauginus atsiusta vektoriu su generuojancia matrica</returns>
        public static int[] MultiplyByGeneratorMatrix(int[] vector, int[][] generatorMatrix)
        {
            (int rows, int columns) dimensions = GetDimensions(generatorMatrix);

            int rows = dimensions.rows;
            int columns = dimensions.columns;
            List<int> encodedVector = new List<int>(vector.Length);

            for (int i = 0; i < columns; i++)
            {
                int vectorBit = 0;
                for (int j = 0; j < rows; j++)
                    vectorBit += (generatorMatrix[j][i] * vector[j]);

                encodedVector.Add(vectorBit % 2);
            }
            return encodedVector.ToArray();
        }

        /// <summary>
        /// Gauna matricos eiluciu ir stulpeliu skaiciu
        /// </summary>
        /// <param name="matrix">Matrica kurios dimensijas norima suzinoti</param>
        /// <returns>Matricos eiluciu ir stulpeliu skaicius</returns>
        private static (int rows, int columns) GetDimensions(int[][] matrix) => (rows: matrix.Length, columns: matrix[0].Length);
    }
}
