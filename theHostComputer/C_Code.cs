using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace theHostComputer
{
    /// <summary>
    /// 解码
    /// </summary>
    class C_Code
    {
        private bool isPass = true;
        private int k = 0;
        private int index = 0;
        //列数
        private const int BOARD_NUM = 2;
        //每列探头数量
        private const int CHAN_PER_BOARD = 6;
        private const int CHAN_TOTAL = BOARD_NUM * CHAN_PER_BOARD;            
        //磁场AD值解码
        public bool Ethernet_Can_Decode(Byte[] Rev, int revSize, long[,] dataBuf)
        {
            if (revSize == CHAN_TOTAL * 3 + 4)
                k = 4;
            else if (revSize == CHAN_TOTAL * 3 + 3)
                k = 3;
            else
                isPass = false;

            if (isPass)
            {
                for (int i = 0; i < BOARD_NUM; i++)
                {
                    for (int j = 0; j < CHAN_PER_BOARD; j++)
                    {
                        index = i * CHAN_PER_BOARD + j;
                        dataBuf[i, j] = Rev[3 * index + k] * 256 * 256 + Rev[3 * index + k + 1] * 256 + Rev[3 * index + k + 2];
                        if (dataBuf[i, j] > 0x800000)
                        {
                            dataBuf[i, j] = dataBuf[i, j] - 0x1000000;
                        }
                    }
                }
            }
            return isPass;
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
