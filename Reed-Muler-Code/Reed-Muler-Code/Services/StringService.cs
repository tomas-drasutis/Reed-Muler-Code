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
        /// <summary>
        /// Pavercia tekstine eilute i dvejetaine eilute, tuomet i vektoriu sarasa, ji uzkoduoja, persiuncia per kanala ir dekoduoja 
        /// </summary>
        /// <param name="message"> Tekstine eilute su kuria bus dirbama</param>
        /// <param name="m">M parametras naudojamas kode</param>
        /// <param name="r">R parametras naudojamas kode</param>
        /// <param name="errorProbability">Klaidos tikimybe</param>
        /// <returns>Grazinama uzkoduota, persiusta per kanala ir dekoduota eilute</returns>
        public string HandleStringWithEncoding(string message, int m, int r, double errorProbability)
        {
            string binaryString = StringHandler.ConvertStringToBinary(message);
            (List<Vector>, int) resultTuple = StringHandler.ConvertBinaryStringToVectors(binaryString, m, r);
            List<Vector> vectorsList = resultTuple.Item1;
            int appendedWords = resultTuple.Item2;

            List<Vector> encodedVectors = vectorsList.Select(vector => Encoder.Encode(vector)).ToList();
            List<Vector> encodedPassedVectors = encodedVectors.Select(vector => Channel.SendThroughNoisyChannel(vector, errorProbability)).ToList();
            List<Vector> decodedVectors = encodedPassedVectors.Select(vector => Decoder.Decode(vector)).ToList();

            string decodedString = StringHandler.ConvertVectorsToBinaryString(decodedVectors, appendedWords);

            return StringHandler.ConvertBinaryStringToString(decodedString);
        }


        /// <summary>
        /// Pavercia eilute i dvejetaine eilute ir persiuncia ja per kanala
        /// </summary>
        /// <param name="message">Tekstine eilute kuri bus persiusta per kanala</param>
        /// <param name="errorProbability">Klaidos tikimybe</param>
        /// <returns>Persiusta per kanala tekstine eilute</returns>
        public string HandleString(string message, double errorProbability)
        {
            string binaryString = StringHandler.ConvertStringToBinary(message);
            string passedString = Channel.SendThroughNoisyChannel(binaryString, errorProbability);

            return StringHandler.ConvertBinaryStringToString(passedString);
        }
    }
}
