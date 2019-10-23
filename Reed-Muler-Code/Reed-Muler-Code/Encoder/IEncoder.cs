using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed_Muler_Code.Encoder
{
    public interface IEncoder
    {
        int[][] EncodeVector(int[] vector);
    }
}
