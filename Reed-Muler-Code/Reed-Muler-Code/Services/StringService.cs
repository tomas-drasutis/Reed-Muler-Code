using System;
using Reed_Muler_Code.Extensions;
using Reed_Muler_Code.Channels;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Encoder = Reed_Muler_Code.Encoders.Encoder;
using Decoder = Reed_Muler_Code.Decoders.Decoder;
using Reed_Muler_Code.Handlers;

namespace Reed_Muler_Code.Services
{
    public class StringService
    {
        public string HandleStringWithEncoding(string message, int m, int r, double errorProbability)
        {
            string binaryString = StringHandler.ConvertStringToBinary(message);
            (List<Vector>, int) resultTuple = StringHandler.ConvertBinaryStringToVectors(binaryString, m, r);
            List<Vector> vectorsList = resultTuple.Item1;
            int appendedBits = resultTuple.Item2;

            List<Vector> encodedVectors = vectorsList.Select(vector => Encoder.Encode(vector)).ToList();
            List<Vector> encodedPassedVectors = encodedVectors.Select(vector => Channel.SendThroughNoisyChannel(vector, errorProbability)).ToList();
            List<Vector> decodedVectors = encodedPassedVectors.Select(vector => Decoder.Decode(vector)).ToList();

            string decodedString = StringHandler.ConvertVectorsToBinaryString(decodedVectors, appendedBits);

            return StringHandler.ConvertBinaryToString(decodedString);
        }

        public string HandleString(string message, double errorProbability)
        {
            string binaryString = StringHandler.ConvertStringToBinary(message);
            string passedString = Channel.SendThroughNoisyChannel(binaryString, errorProbability);

            return StringHandler.ConvertBinaryToString(passedString);
        }
    }
}
