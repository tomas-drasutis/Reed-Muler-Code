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
        /// Sugeneruoja baitu kombinacijas (Pvz: 00, 01, 10, 11)
        /// </summary>
        /// <param name="lengthOfBytesList">Nusako kiek tų baitų bus</param>
        /// <param name="lengthOfByte">Nusako kokio ilgio 'baitas'</param>
        /// <returns>Baitu kombinaciju sarasas</returns>
        public static List<string> GetBytes(int lengthOfBytesList, int lengthOfByte)
        {
            List<string> bytes = Cache.GetBytes(lengthOfBytesList, lengthOfByte);

            if (bytes != null)
                return bytes;

            bytes = new List<string>();

            for (int i = 0; i < lengthOfBytesList; i++)
                bytes.Add(Convert.ToString(i, 2).PadLeft(lengthOfByte, '0'));

            Cache.AddBytesList(lengthOfBytesList, lengthOfByte, bytes);
            return bytes;
        }
    }
}
