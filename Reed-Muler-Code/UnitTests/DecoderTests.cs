using NUnit.Framework;
using Reed_Muler_Code;
using Reed_Muler_Code.Channels;
using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Decoder = Reed_Muler_Code.Decoders.Decoder;

namespace UnitTests
{
    public class DecoderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Decode_Succeeds()
        {
            int m = 3;
            int r = 2;
            double mistakeProbability = 0.0;
            int[] encodedVector = new int[] { 0, 0, 0, 1, 0, 0, 0, 1 };
            int[] expectedVector = new int[] { 1, 0, 1, 1, 0, 0, 1 };
            int[][] generatorMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                                                   new int[] { 1, 1, 0, 0, 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 1, 0, 1, 0 },
                                                   new int[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 0, 0, 1, 0, 0, 0 }};

            Cache.AddGeneratorMatrix(m, r, generatorMatrix);
            Vector noisyVector = Channel.SendThroughNoisyChannel(new Vector(m, r, encodedVector.ArrayToString()), mistakeProbability);

            var result = Decoder.Decode(noisyVector);

            PrintVector(expectedVector, "Expected:");
            PrintVector(result.Bits, "Result:");
            Assert.AreEqual(expectedVector, result.Bits);
        }

        [Test]
        public void Decode_M3R1_Succeeds()
        {
            int m = 3;
            int r = 1;
            int[] encodedVector = new int[] { 0, 1, 0, 1, 0, 1, 0, 1 };
            int[] channelVector = new int[] { 0, 1, 0, 1, 0, 1, 0, 0 };
            int[] expectedVector = new int[] { 1, 0, 0, 1};
            int[][] generatorMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                                                   new int[] { 1, 1, 0, 0, 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 1, 0, 1, 0 }};

            Cache.AddGeneratorMatrix(m, r, generatorMatrix);
            Vector noisyVector = new Vector(m, r, channelVector.ArrayToString());

            var result = Decoder.Decode(noisyVector);

            PrintVector(noisyVector.Bits, "Noisy Vector:");
            PrintVector(expectedVector, "Expected:");
            PrintVector(result.Bits, "Result:");
            Console.WriteLine("Error count: " + Channel.GetErrorPositions(new Vector(m, r, encodedVector.ArrayToString()), noisyVector).Count);
            Assert.AreEqual(expectedVector, result.Bits);
        }

        [Test]
        public void Decode_M4R2_1E_Succeeds()
        {
            int m = 4;
            int r = 2;
            int[] encodedVector = new int[] { 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1 };
            int[] channelVector = new int[] { 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1 };
            int[] expectedVector = new int[] { 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1 };
            int[][] generatorMatrix = new int[][] { new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
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

            Cache.AddGeneratorMatrix(m, r, generatorMatrix);
            Vector noisyVector = new Vector(m, r, channelVector.ArrayToString());

            var result = Decoder.Decode(noisyVector);

            PrintVector(noisyVector.Bits, "Noisy Vector:");
            PrintVector(expectedVector, "Expected:");
            PrintVector(result.Bits, "Result:");
            Console.WriteLine("Error count: " + Channel.GetErrorPositions(new Vector(m, r, encodedVector.ArrayToString()), noisyVector).Count);
            Assert.AreEqual(expectedVector, result.Bits);
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
