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

            Channel channel = new Channel();
            Vector noisyVector = channel.SendThroughNoisyChannel(new Vector(m, r, encodedVector.ArrayToString()), mistakeProbability);

            int[] result = Decoder.Decode(noisyVector).ToArray();

            PrintVector(expectedVector, "Expected:");
            PrintVector(result, "Result:");
            Assert.AreEqual(expectedVector, result);
        }

        [Test]
        public void Decode_M3R2_1E_Succeeds()
        {
            int m = 3;
            int r = 2;
            int[] encodedVector = new int[] { 0, 0, 0, 1, 0, 0, 0, 1 };
            int[] channelVector = new int[] { 0, 1, 0, 1, 0, 0, 1, 1 };
            int[] expectedVector = new int[] { 1, 0, 1, 1, 0, 0, 1 };
            int[][] generatorMatrix = new int[][] { new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                                                   new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                                                   new int[] { 1, 1, 0, 0, 1, 1, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 1, 0, 1, 0 },
                                                   new int[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 1, 0, 0, 0, 0, 0 },
                                                   new int[] { 1, 0, 0, 0, 1, 0, 0, 0 }};
            Cache.AddGeneratorMatrix(m, r, generatorMatrix);

            Channel channel = new Channel();
            Vector noisyVector = new Vector(m, r, channelVector.ArrayToString());

            int[] result = Decoder.Decode(noisyVector).ToArray();

            PrintVector(noisyVector.Bits, "Noisy Vector:");
            PrintVector(expectedVector, "Expected:");
            PrintVector(result, "Result:");
            Console.WriteLine("Error count: " + channel.GetErrorPositions(new Vector(m, r, encodedVector.ArrayToString()), noisyVector).Count);
            Assert.AreEqual(expectedVector, result);
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
