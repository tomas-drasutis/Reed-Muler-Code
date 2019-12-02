using NUnit.Framework;
using System;
using Encoder = Reed_Muler_Code.Encoders.Encoder;

namespace UnitTests
{
    public class EncoderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Encode_M3R2_Succeeds()
        {
            int m = 3;
            int r = 2;
            int[] vector = new int[] { 1, 0, 1, 1, 0, 0, 1 };
            int[] expectedVector = new int[] { 0, 0, 0, 1, 0, 0, 0, 1 };
            /*int[][] generatorMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                                                   new int[] { 1, 1, 0, 0, 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 1, 0, 1, 0 },
                                                   new int[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 0, 0, 1, 0, 0, 0 }};*/


            var result = Encoder.Encode(new Reed_Muler_Code.Vector(m, r, vector));

            PrintVector(expectedVector, "Expected:");
            PrintVector(result.Words, "Result:");
            Assert.AreEqual(expectedVector, result.Words);
        }

        private void PrintVector(int[] vector, string message)
        {
            Console.WriteLine(message);
            for (int i = 0; i < vector.GetLength(0); i++)
                Console.Write(vector[i]);

            Console.WriteLine();
        }
    }
}