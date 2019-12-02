using System;
using System.Drawing;
using System.Linq;

namespace Reed_Muler_Code.Handlers
{
    public class ImageHandler
    {
        /// <summary>
        /// Konvertuoja nuotrauka i dvejetaine simboliu eilute
        /// </summary>
        /// <param name="image">Nuotrauka kuri bus konvertuojama</param>
        /// <returns>Dvejetaine simboliu eilute</returns>
        public static string ConvertImageToBinaryString(Image image)
        {
            ImageConverter converter = new ImageConverter();
            byte[] bytes = (byte[])converter.ConvertTo(image, typeof(byte[]));

            return string.Join("", bytes?.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));
        }

        /// <summary>
        /// Konvertuoja dvejetaine simboliu eilute i nuotrauka
        /// </summary>
        /// <param name="binaryString">Dvejetaine simboliu eiute kuri bus konvertuojama i nuotrauka</param>
        /// <returns>Nuotrauka gauta is dvejetaines simboliu eilutes</returns>
        public static Image ConvertBinaryStringToImage(string binaryString)
        {
            int numOfBytes = binaryString.Length / 8;
            byte[] bytes = new byte[numOfBytes];

            for (var i = 0; i < numOfBytes; i++)
                bytes[i] = Convert.ToByte(binaryString.Substring(8 * i, 8), 2);

            return ((new ImageConverter()).ConvertFrom(bytes)) as Image;
        }

        /// <summary>
        /// Nuo nuortaukos nuima Bitmap formato antraste (pirmieji 54 baitai)
        /// </summary>
        /// <param name="binaryString">Dvejetaine simboliu eilute</param>
        /// <returns>Bitmap formato antraste ir dvejetaine simbolu eilute be antrastes</returns>
        public static (string, string) RemoveBmpHeaderFromBitArray(string binaryString) => (binaryString.Substring(0, 54 * 8), binaryString.Substring(54 * 8));
    }
}
