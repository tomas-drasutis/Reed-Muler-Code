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
            var dimensions = GetDimensions(generatorMatrix);

            var encodedVector = new List<int>(vector.Length);
            int rows = dimensions.Rows;
            int columns = dimensions.Columns;

            for (int i = 0; i < columns; i++)
            {
                int vectorBit = 0;
                for (int j = 0; j < rows; j++)
                    vectorBit += (generatorMatrix[j][i] * vector[j]);

                encodedVector.Add(vectorBit % 2);
            }
            return encodedVector.ToArray();
        }

        private static (int Rows, int Columns) GetDimensions(int[][] matrix) => (Rows: matrix.Length, Columns: matrix[0].Length);
    }
}
