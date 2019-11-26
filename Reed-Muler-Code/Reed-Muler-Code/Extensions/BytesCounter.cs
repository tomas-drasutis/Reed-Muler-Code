using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.Extensions
{
    public static class BytesCounter
    {
        /// <summary>
        /// Gauna baitus 
        /// </summary>
        /// <param name="lengthOfBytesList">Nusako kiek tų baitų bus</param>
        /// <param name="lengthOfByte">Nusako kokio ilgio 'baitas'</param>
        /// <returns></returns>
        public static List<string> GetBytes(int lengthOfBytesList, int lengthOfByte)
        {
            List<string> bytes = Cache.GetBytes(lengthOfBytesList, lengthOfByte);

            if (bytes != null)
                return bytes;

            bytes = new List<string>();

            for (int i = 0; i < lengthOfBytesList; i++)
            {
                var binary = Convert.ToString(i, 2).PadLeft(lengthOfByte, '0');
                Console.WriteLine("binary: " + binary);
                bytes.Add(binary);
            }

            Cache.AddBytesList(lengthOfBytesList, lengthOfByte, bytes);
            return bytes;
        }
    }
}
