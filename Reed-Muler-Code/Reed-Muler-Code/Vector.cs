using Reed_Muler_Code.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code
{
    public class Vector
    {
        public int[] Bits { get; set; }
        public int M { get; set; }
        public int R { get; set; }
        public override string ToString() => string.Join("", Bits);
        public static int GetExpectedVectorLength(int m, int r) => m.CountCombination(r);
        public Vector( int m, int r, string bits) : this(m, r, bits.Select(c => int.Parse(c.ToString())).ToArray()) { }

        public Vector(int m, int r, int[] bits)
        {
            Bits = bits;
            M = m;
            R = r;
        }        
    }
}
