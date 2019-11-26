using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code
{
    public static class Cache
    {
        private static ConcurrentDictionary<(int, int), int[][]> _generatorMatrices = new ConcurrentDictionary<(int, int), int[][]>();
        private static ConcurrentDictionary<(int, int, string), int[]> _encodedVectors = new ConcurrentDictionary<(int, int, string), int[]>();
        private static ConcurrentDictionary<(int, int), List<string>> _bytes = new ConcurrentDictionary<(int, int), List<string>>();

        public static void AddGeneratorMatrix(int m, int r, int[][] generatorMatrix) => _generatorMatrices.TryAdd((m, r), generatorMatrix);
        public static void AddEncodedVector(int m, int r, int[] vector, int[] encodedVector) => _encodedVectors.TryAdd((m, r, vector.ArrayToString()), encodedVector);
        public static void AddBytesList(int lengthOfBytesList, int lengthOfByte, List<string> bytesList) => _bytes.TryAdd((lengthOfBytesList, lengthOfByte), bytesList);

        public static int[][] GetGeneratorMatrix(int m, int r)
        {
            if (_generatorMatrices.TryGetValue((m, r), out var generatorMatrix))
                return generatorMatrix;

            return null;
        }
        public static int[] GetEncodedVector(int m, int r, int[] vector)
        {
            if (_encodedVectors.TryGetValue((m, r, vector.ArrayToString()), out var encodedVectorCached))
                return encodedVectorCached;

            return null;
        }
        public static List<string> GetBytes(int lengthOfBytesList, int lengthOfByte)
        {
            if (_bytes.TryGetValue((lengthOfBytesList, lengthOfByte), out var bytesList))
                return bytesList;

            return null;
        }

    }
}
