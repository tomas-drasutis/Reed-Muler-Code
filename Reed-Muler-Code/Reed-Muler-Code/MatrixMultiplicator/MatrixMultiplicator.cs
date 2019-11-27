using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.MatrixMultiplicator
{
    public class MatrixMultiplicator
    {
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

        private static (int rows, int columns) GetDimensions(int[][] matrix) => (rows: matrix.Length, columns: matrix[0].Length);
    }
}
