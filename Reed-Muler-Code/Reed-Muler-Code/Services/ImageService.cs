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
        private readonly StringHandler _stringHandler = new StringHandler();
        private readonly ImageHandler _imageHandler = new ImageHandler();

        public Image HandlePictureWithEncoding(Bitmap image, int m, int r, double errorProbability)
        {
            string binaryImageString = _imageHandler.ConvertImageToBinaryString(image);
            (string, string) imageStringTuple = _imageHandler.RemoveBmpHeaderFromBitArray(binaryImageString);
            
            string header = imageStringTuple.Item1;
            binaryImageString = imageStringTuple.Item2;

            List<Vector> vectorsList = _stringHandler.ConvertBinaryStringToVectors(binaryImageString, m, r, out var appendedBits);

            Vector[] encodedVectors = new Vector[vectorsList.Count];
            Parallel.For(0, encodedVectors.Length, i => { encodedVectors[i] = Encoder.Encode(vectorsList[i]); });

            Vector[] encodedPassedVectors = new Vector[encodedVectors.Length];
            Parallel.For(0, encodedPassedVectors.Length, i => { encodedPassedVectors[i] = Channel.SendThroughNoisyChannel(encodedVectors[i], errorProbability); });

            Vector[] decodedVectors = new Vector[encodedPassedVectors.Length];
            Parallel.For(0, decodedVectors.Length, i => { decodedVectors[i] = Decoder.Decode(encodedPassedVectors[i]); });

            binaryImageString = _stringHandler.ConvertVectorsToBinaryString(decodedVectors.ToList(), appendedBits);
            return _imageHandler.ConvertBinaryStringToImage(header + binaryImageString);
        }

        public Image HandlePicture(Bitmap image, double errorProbability)
        {
            string binaryImageString = _imageHandler.ConvertImageToBinaryString(image);
            (string, string) imageStringTuple = _imageHandler.RemoveBmpHeaderFromBitArray(binaryImageString);

            string header = imageStringTuple.Item1;
            binaryImageString = imageStringTuple.Item2;

            string passedBinaryString = Channel.SendThroughNoisyChannel(binaryImageString, errorProbability);
            return _imageHandler.ConvertBinaryStringToImage(header + passedBinaryString);
        }
    }
}
