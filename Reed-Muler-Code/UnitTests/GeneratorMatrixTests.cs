using NUnit.Framework;
using Reed_Muler_Code.Matrix;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public class GeneratorMatrixTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GenerateMatrix_M1R0_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] { 1, 1 } };

            int[][] matrix = GeneratorMatrix.GenerateMatrix(1, 0);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        [Test]
        public void GenerateMatrix_M1R1_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] { 1, 1 },
                                                   new int[] { 1, 0 }};

            int[][] matrix = GeneratorMatrix.GenerateMatrix(1, 1);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        [Test]

        public void GenerateMatrix_M2R1_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] { 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0 }};

            int[][] matrix = GeneratorMatrix.GenerateMatrix(2, 1);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        [Test]

        public void GenerateMatrix_M3R1_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                                                   new int[] { 1, 1, 0, 0, 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 1, 0, 1, 0 }};

            int[][] matrix = GeneratorMatrix.GenerateMatrix(3, 1);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        [Test]

        public void GenerateMatrix_M3R2_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                                                   new int[] { 1, 1, 0, 0, 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 1, 0, 1, 0 },
                                                   new int[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 0, 0, 1, 0, 0, 0 }};

            int[][] matrix = GeneratorMatrix.GenerateMatrix(3, 2);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        [Test]

        public void GenerateMatrix_M3R3_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 0, 1, 0, 1, 0, 1, 0, 1 },
                                                   new int[] { 0, 0, 1, 1, 0, 0, 1, 1 },
                                                   new int[] { 0, 0, 0, 1, 0, 0, 0, 1 },
                                                   new int[] { 0, 0, 0, 0, 1, 1, 1, 1 },
                                                   new int[] { 0, 0, 0, 0, 0, 1, 0, 1 },
                                                   new int[] { 0, 0, 0, 0, 0, 0, 1, 1 },
                                                   new int[] { 0, 0, 0, 0, 0, 0, 0, 1 }};

            int[][] matrix = GeneratorMatrix.GenerateMatrix(3, 3);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        [Test]
        public void GenerateMatrix_M4R2_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                                                   new int[] { 1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0 },
                                                   new int[] { 1,1,1,1,0,0,0,0,1,1,1,1,0,0,0,0 },
                                                   new int[] { 1,1,0,0,1,1,0,0,1,1,0,0,1,1,0,0 },
                                                   new int[] { 1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0 },
                                                   new int[] { 1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0 },
                                                   new int[] { 1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0 },
                                                   new int[] { 1,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0 },
                                                   new int[] { 1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0 },
                                                   new int[] { 1,0,1,0,0,0,0,0,1,0,1,0,0,0,0,0 },
                                                   new int[] { 1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0 }};

            int[][] matrix = GeneratorMatrix.GenerateMatrix(4, 2);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        [Test]

        public void GenerateMatrix_M2R3_Succeeds()
        {
            int[][] expectedMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                                                   new int[] { 1, 1, 0, 0, 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 1, 0, 1, 0 },
                                                   new int[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 0, 0, 1, 0, 0, 0 }};

            int[][] matrix = GeneratorMatrix.GenerateMatrix(2, 3);

            PrintMatrix(matrix, "Received");
            PrintMatrix(expectedMatrix, "Expected");
            Assert.AreEqual(expectedMatrix, matrix);
        }

        private void PrintMatrix(int[][] matrix, string message)
        {
            Console.WriteLine(message);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix[0].GetLength(0); j++)
                {
                    Console.Write(matrix[i][j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
