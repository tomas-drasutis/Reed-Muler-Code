using Reed_Muler_Code.Channels;
using Reed_Muler_Code.Handlers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Encoder = Reed_Muler_Code.Encoders.Encoder;
using Decoder = Reed_Muler_Code.Decoders.Decoder;
using System.Threading.Tasks;

namespace Reed_Muler_Code.Services
{
    public class ImageService
    {
        /// <summary>
        /// Gauta nuotrauka pavercia i dvejetaine tekstine eilute, ja pavercia i vektoriu sarasa,
        /// tuomet vektoriu sarasas uzkoduojamas, persiunciamas per kanala, ir dekoduojamas
        /// </summary>
        /// <param name="image">Nuotrauka su kuria bus dirbama</param>
        /// <param name="m">M parametras naudojamas RM kode</param>
        /// <param name="r">R parametras naudojamas RM kode</param>
        /// <param name="errorProbability">Klaidos tikimybe</param>
        /// <returns>Grazinama uzkoduota, per kanala persiusta ir dekoduota nuotrauka</returns>
        public Image HandlePictureWithEncoding(Image image, int m, int r, double errorProbability)
        {
            string binaryImageString = ImageHandler.ConvertImageToBinaryString(image);
            (string, string) imageStringTuple = ImageHandler.RemoveBmpHeaderFromBitArray(binaryImageString);
            
            string header = imageStringTuple.Item1;
            binaryImageString = imageStringTuple.Item2;

            (List<Vector>, int) resultTuple = StringHandler.ConvertBinaryStringToVectors(binaryImageString, m, r);
            List<Vector> vectorsList = resultTuple.Item1;
            int appendedWords = resultTuple.Item2;

            Vector[] encodedVectors = new Vector[vectorsList.Count];
            Parallel.For(0, encodedVectors.Length, i => { encodedVectors[i] = Encoder.Encode(vectorsList[i]); });

            Vector[] encodedPassedVectors = new Vector[encodedVectors.Length];
            for (int i = 0; i < encodedVectors.Length; i++)
                encodedPassedVectors[i] = Channel.SendThroughNoisyChannel(encodedVectors[i], errorProbability);

            Vector[] decodedVectors = new Vector[encodedPassedVectors.Length];
            Parallel.For(0, decodedVectors.Length, i => { decodedVectors[i] = Decoder.Decode(encodedPassedVectors[i]); });

            binaryImageString = StringHandler.ConvertVectorsToBinaryString(decodedVectors.ToList(), appendedWords);
            return ImageHandler.ConvertBinaryStringToImage(header + binaryImageString);
        }

        /// <summary>
        /// Gauta nuotrauka pavercia i dvejetaine eilute ir ja persiuncia per kanala
        /// </summary>
        /// <param name="image">Nuotrauka su kuria bus dirbama</param>
        /// <param name="errorProbability">Klaidos tikimybe</param>
        /// <returns>Grazinama nuotrauka persiusta per kanala</returns>
        public Image HandlePicture(Image image, double errorProbability)
        {
            string binaryImageString = ImageHandler.ConvertImageToBinaryString(image);
            (string, string) imageStringTuple = ImageHandler.RemoveBmpHeaderFromBitArray(binaryImageString);

            string header = imageStringTuple.Item1;
            binaryImageString = imageStringTuple.Item2;

            string passedBinaryString = Channel.SendThroughNoisyChannel(binaryImageString, errorProbability);
            return ImageHandler.ConvertBinaryStringToImage(header + passedBinaryString);
        }
    }
}
