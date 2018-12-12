using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace theHostComputer
{
    class C_Code
    {

        private const int BOARD_NUM = 2;    //两列
        private const int CHAN_PER_BOARD = 6;  //每列6个探头
        private const int CHAN_TOTAL = BOARD_NUM * CHAN_PER_BOARD;   //探头总数             
        //磁场AD值解码
        public bool Ethernet_Can_Decode(Byte[] Rev, int revSize, long[,] dataBuf)
        {
             int b;
             int k=0;
             if(revSize == BOARD_NUM * CHAN_PER_BOARD * 3 + 4)
             {
                 b=1;
                 k = 4;
             }
             else if (revSize == BOARD_NUM * CHAN_PER_BOARD * 3 + 3)
             {
                 b = 2;
                 k = 3;
             }
             else
             {
                 b = 0;
             }

             if (b != 0)
             {
                 for (int i = 0; i <= BOARD_NUM - 1; i++)
                 {
                     for (int j = 0; j <= CHAN_PER_BOARD - 1; j++)
                     {
                         int Index = i * CHAN_PER_BOARD + j;
                         dataBuf[i, j] = Rev[3 * Index + k] * 256 * 256 + Rev[3 * Index + k + 1] * 256 + Rev[3 * Index + k + 2];
                         if (dataBuf[i, j] > 0x800000)
                         {
                             dataBuf[i, j] = dataBuf[i, j] - 0x1000000;
                         }
                     }
                 }
             }
             return (b != 0);
        }
        //距离AD值解码
        public bool ReceiveDistData(Byte[] Rev,  long[] Dist,int revSize)
        {
            if (revSize >= (4 + 14))
            {
                Dist[0] = Rev[1] * 256 * 256 + Rev[2] * 256 + Rev[3];
            }
            return (revSize >= (4 + 14));
        }
    }
}
