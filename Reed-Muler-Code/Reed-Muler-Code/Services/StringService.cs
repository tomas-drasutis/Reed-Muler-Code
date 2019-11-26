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
        private readonly StringHandler _stringHandler = new StringHandler();

        public string HandleStringWithEncoding(string message, int m, int r, double errorProbability)
        {
            string binaryString = _stringHandler.ConvertStringToBinary(message);
            List<Vector> vectorsList = _stringHandler.ConvertBinaryStringToVectors(binaryString, m, r, out var appendedBits);

            List<Vector> encodedVectors = vectorsList.Select(vector => Encoder.Encode(vector)).ToList();
            List<Vector> encodedPassedVectors = encodedVectors.Select(vector => Channel.SendThroughNoisyChannel(vector, errorProbability)).ToList();
            List<Vector> decodedVectors = encodedPassedVectors.Select(vector => Decoder.Decode(vector)).ToList();

            string decodedString = _stringHandler.ConvertVectorsToBinaryString(decodedVectors, appendedBits);

            return _stringHandler.ConvertBinaryToString(decodedString);
        }

        public string HandleString(string message, double errorProbability)
        {
            string binaryString = _stringHandler.ConvertStringToBinary(message);
            string passedString = Channel.SendThroughNoisyChannel(binaryString, errorProbability);

            return _stringHandler.ConvertBinaryToString(passedString);
        }
    }
}
