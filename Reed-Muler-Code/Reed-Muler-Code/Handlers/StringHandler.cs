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
        /// <summary>
        /// Konvertuoja simboliu eilute i dvejetaine eilute
        /// </summary>
        /// <param name="message">Simboliu eilute kuri bus konvertuojama</param>
        /// <returns>Dvejetaine simboliu eilute</returns>
        public static string ConvertStringToBinary(string message) => string.Join("", Encoding.ASCII.GetBytes(message).Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));

        /// <summary>
        /// Konvertuoja dvejetaine simboliu eilute i Vektoriaus objektus 
        /// Vektoriu ilgis apskaiciuojamas pasitelkiant k formule, jei simboliu eilute per trumpa lygiai isdalinti i vektorius
        /// Prie jos galo pridedami nuliai
        /// </summary>
        /// <param name="message">Simboliu eilute kuri bus konvertuojama</param>
        /// <param name="m">M parametras issaugomas Vektoriaus objekte</param>
        /// <param name="r">R parametrtas issaugojamas Vektoriaus objekte</param>
        /// <returns>Vektoriaus objektu sarasas</returns>
        public static (List<Vector>, int) ConvertBinaryStringToVectors(string message, int m, int r)
        {
            List<string> vectors = message.SplitInParts(Vector.GetExpectedVectorLength(m, r)).ToList();

            int appendedWords = Vector.GetExpectedVectorLength(m, r) - vectors.Last().Length;
            vectors[vectors.Count - 1] = AppendWords(vectors[vectors.Count - 1], appendedWords);

            return (vectors.Select(vector => new Vector(m, r, vector)).ToList(), appendedWords);
        }

        /// <summary>
        /// Prideda nulinius bitus prie simboliu eilutes
        /// </summary>
        /// <param name="message">Dvejetainiu simboliu eilute prie kurios pridedami nuliai</param>
        /// <param name="WordsToAppend">Skaicius kiek nuliu reikia prideti</param>
        /// <returns>Simboliu eilute su pridetais nuliais prie galo</returns>
        public static string AppendWords(string message, int WordsToAppend)
        {
            for (int i = 0; i < WordsToAppend; i++)
                message += "0";

            return message;
        }

        /// <summary>
        /// Pavercia vektoriu sarasa i dvejetaine simboliu eilute, taip pat nuimami prideti nuliniai bitai
        /// </summary>
        /// <param name="vectors">Vektoriu objektu sarasas</param>
        /// <param name="appendedWords">Kiek bitu buvo prideta prie dvejetaines simboliu eilutes</param>
        /// <returns>Dvejetaine simboliu eilute</returns>
        public static  string ConvertVectorsToBinaryString(List<Vector> vectors, int appendedWords)
        {
            string message = string.Join("", vectors.Select(x => x.Words.ArrayToString()));
            return message.Substring(0, message.Length - appendedWords);
        }

        /// <summary>
        /// Pavercia dvejetaine simboliu eilute i tekstine simboliu eilute
        /// </summary>
        /// <param name="binaryString">Dvejetaine simboliu eilute</param>
        /// <returns>Tekstine simboliu eilute</returns>
        public static string ConvertBinaryStringToString(string binaryString)
        {
            List<byte> byteList = new List<byte>();

            for (int i = 0; i < binaryString.Length; i += 8)
                byteList.Add(Convert.ToByte(binaryString.Substring(i, 8), 2));

            return Encoding.ASCII.GetString(byteList.ToArray());
        }
    }
}
