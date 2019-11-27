using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.Handlers
{
    public class StringHandler
    {
        public static string ConvertStringToBinary(string message) => string.Join("", Encoding.ASCII.GetBytes(message).Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));

        public static (List<Vector>, int) ConvertBinaryStringToVectors(string message, int m, int r)
        {
            List<string> vectors = message.SplitInParts(Vector.GetExpectedVectorLength(m, r)).ToList();

            int appendedBits = Vector.GetExpectedVectorLength(m, r) - vectors.Last().Length;
            vectors[vectors.Count - 1] = AppendBits(vectors[vectors.Count - 1], appendedBits);

            return (vectors.Select(vector => new Vector(m, r, vector)).ToList(), appendedBits);
        }
        public static string AppendBits(string message, int bitsToAppend)
        {
            for (int i = 0; i < bitsToAppend; i++)
                message += "0";

            return message;
        }

        public static  string ConvertVectorsToBinaryString(List<Vector> vectors, int appendedBits)
        {
            string message = string.Join("", vectors.Select(x => x.Bits.ArrayToString()));
            return message.Substring(0, message.Length - appendedBits);
        }

        public static string ConvertBinaryToString(string data)
        {
            List<byte> byteList = new List<byte>();

            for (int i = 0; i < data.Length; i += 8)
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));

            return Encoding.ASCII.GetString(byteList.ToArray());
        }
    }
}
