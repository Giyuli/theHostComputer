using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theHostComputer
{
    class C_Pubdef
    {             
        public const byte REV_DATA = 0x11;    //下位机测磁应答
        public const byte REV_DIST = 0x14;               

        public  struct TCalData
        {
            public double[,] Cal_0;
            public double[,] Cal_p30000;
            public double[,] Cal_n30000;
        }             
    }
}
