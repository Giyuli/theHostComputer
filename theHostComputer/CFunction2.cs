using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theHostComputer
{
    class CFunction2
    {
        CDigitalFilter F = new CDigitalFilter();

        private double[,] x;
        private int tongdao_num;
        private long ioriginal_num;
        private double Liftoff=5;    //提离高度
        private double ThickOftube; //钢管壁厚 
        //private double YUZHI = 1.8;
        private double YUZHI = 1;
        //private double YUZHI2 = 3;
        //private double[,] xx = new double[12, 100000]; 
        public double[,] UPDW = new double[12,2]; //阈值
        public double[,] dy_original = new double[12, 100000];   //阈值去噪2
        public double[,] y_cidao = new double[12, 100000];   //阈值去噪2
        public double[,] yshuzhi = new double[12, 100000];   //阈值去噪2
        public int [,] xshuzhi = new int [12, 100000];   //阈值去噪2
        //输入参数
        public void Function(double[,] x,int n,long m,double t)
        {
            this.x = x;
            this.tongdao_num = n;
            this.ioriginal_num = m;            
            this.ThickOftube = t;
        }

        ////计算梯度曲线
        //private void calculate_dyoriginal()
        //{
        //    double dy_originalsum = 0, dy_originalmean = 0;
        //    double dy_originalstdsum = 0, dy_originalstd = 0;
        //    double temp = 0;

        //    for (int col = 0; col < this.tongdao_num; col++)
        //    {
        //        for (int i = 0; i < this.ioriginal_num - 1; i++)
        //        {
        //            dy_original[col, i] = x[col, i + 1] - x[col, i];
        //            dy_originalsum += dy_original[col, i];
        //        }
        //        dy_originalmean = dy_originalsum / (this.ioriginal_num - 1);
        //        dy_originalsum = 0;
        //        for (int i = 0; i < this.ioriginal_num - 1; i++)
        //        {
        //            temp = dy_original[col, i] - dy_originalmean;
        //            dy_originalstdsum += Math.Pow(temp, 2);
        //        }
        //        dy_originalstd = Math.Sqrt(dy_originalstdsum / (this.ioriginal_num - 1));
        //        dy_originalstdsum = 0;
        //        UPDW[col, 0] = dy_originalmean + 1.8 * dy_originalstd;
        //        UPDW[col, 1] = dy_originalmean - 1.8 * dy_originalstd;
        //    }
        //}


        // 滤波
        private double[,] Lvbo_fft()
        {   
            double[,] y_lvbo = new double[this.tongdao_num, this.ioriginal_num];
            //double [] ytemp=new double[this.ioriginal_num];
            //double [] xtemp=new double[this.ioriginal_num];
            //double[] b = new double[10]{0.0044914, -0.010589, -0.044215, 0.10915, 0.44117, 0.44117, 0.10915, -0.044215, -0.010589, 0.0044914 };
            //double[] a = new double[1]{1};

            //double dy_originalsum = 0, dy_originalmean = 0;
            //double dy_originalstdsum = 0, dy_originalstd = 0;
            //double temp = 0;

            //for (int col = 0; col < this.tongdao_num; col++)
            //{
            //    for (int i = 0; i < this.ioriginal_num - 1; i++)
            //    {
            //        dy_original[col, i] = x[col, i + 1] - x[col, i];
            //        dy_originalsum += dy_original[col, i];
            //    }
            //    dy_originalmean = dy_originalsum / (this.ioriginal_num - 1);
            //    dy_originalsum = 0;
            //    for (int i = 0; i < this.ioriginal_num - 1; i++)
            //    {
            //        temp = dy_original[col, i] - dy_originalmean;
            //        dy_originalstdsum += Math.Pow(temp, 2);
            //    }
            //    dy_originalstd = Math.Sqrt(dy_originalstdsum / (this.ioriginal_num - 1));
            //    dy_originalstdsum = 0;
            //    UPDW[col, 0] = dy_originalmean + YUZHI * dy_originalstd;
            //    UPDW[col, 1] = dy_originalmean - YUZHI  * dy_originalstd;
            //}
            ////for (int i = 1; i < this.tongdao_num; i++)
            ////{
            ////    for (long j = 0; j < this.ioriginal_num; j++)
            ////    {
            ////        xx[i,j] = x[i, j] - x[i - 1, j];
            ////    }
            ////}
            ////for (long j = 0; j < this.ioriginal_num; j++)
            ////{
            ////    xx[0, j] = x[0, j] - x[this.tongdao_num-1, j];
            ////}

            for (int i = 0; i < this.tongdao_num; i++)
            {
                for(long j=0;j<this.ioriginal_num;j++)
                {
                    //xtemp[j] = x[i, j];
                    y_lvbo[i,j] = x[i, j];
                }
                //F.DigitalFilter(b,a,xtemp);
                //ytemp = F.zeroFilter();
                //for (long j = 0; j < this.ioriginal_num; j++)
                //{
                //    y_lvbo[i, j] = ytemp[j];
                //}                
            }
            return y_lvbo;
        }

        ////计算腐蚀深度
        //private double[,] DepthOfCorrosion()
        //{                
        //    double[] yy_lvboUPDW = new double[2];
        //    double[,] y_lvbo = new double[this.tongdao_num, this.ioriginal_num];
        //    double[] yy_lvbo = new double[this.ioriginal_num];    //原始曲线与滤波曲线插值  
        //    double[] yy_lvbo_zl = new double[this.ioriginal_num];    // 
        //    double[] fengdian_y = new double[this.ioriginal_num];   //原始曲线与滤波曲线插值峰值点   
        //    long [] fengdian_x = new long[this.ioriginal_num];   //原始曲线与滤波曲线插值峰值点 
        //    long k = 0;
        //    long fengdian_num = 0;
        //    double[] yy_lvbo_yz = new double[this.ioriginal_num];    //原始曲线与滤波曲线插值 
        //    double[,] DepthOfCorr = new double[this.tongdao_num, this.ioriginal_num]; //腐蚀深度
        //    y_lvbo = this.Lvbo_fft();
        //    for (int col = 0; col < this.tongdao_num; col++)
        //    {
        //        //求原始曲线与滤波曲线插值
        //        for (long i = 0; i < this.ioriginal_num; i++)
        //        {
        //            yy_lvbo[i] = xx[col, i] - y_lvbo[col, i];
        //        }
        //        //求原始曲线与滤波曲线插值上下限1
        //        yy_lvboUPDW = dbdt(yy_lvbo, YUZHI1);
        //        //求峰值点               
        //        fengdian_y[0] = yy_lvbo[0];
        //        fengdian_x[0] = 0;
        //        k = 1;
        //        for (long i = 1; i < this.ioriginal_num-1; i++)
        //        {
        //            if ((yy_lvbo[i] >= yy_lvbo[i - 1] && yy_lvbo[i] >= yy_lvbo[i + 1]) | (yy_lvbo[i] <= yy_lvbo[i - 1] && yy_lvbo[i] <= yy_lvbo[i + 1]))
        //            {
        //                fengdian_y[k] = yy_lvbo[i];
        //                fengdian_x[k] = i;
        //                k = k + 1;
        //            }
        //        }
        //        fengdian_y[k] = yy_lvbo[this.ioriginal_num-1];
        //        fengdian_x[k] = this.ioriginal_num-1;
        //        fengdian_num = k;
        //        //保留阈值线内的点
        //        k = 0;
        //        for (long i = 0; i < fengdian_num-1; i++)
        //        {
        //            if ((fengdian_y[i] > yy_lvboUPDW[0] | fengdian_y[i + 1] < yy_lvboUPDW[1]) | (fengdian_y[i] < yy_lvboUPDW[1] | fengdian_y[i + 1] > yy_lvboUPDW[0]))
        //            {
        //                for (long j = fengdian_x[i]; j <= fengdian_x[i + 1];j++ )
        //                {
        //                    yy_lvbo[j] = 0;
        //                }
        //            }
        //            else
        //            {
        //                for (long j = fengdian_x[i]; j < fengdian_x[i + 1]; j++)
        //                {
        //                    yy_lvbo_zl[k] = yy_lvbo[j];
        //                    k = k + 1;
        //                }
        //            }
        //        }
        //        //再次求上下限                
        //        yy_lvboUPDW = dbdt(yy_lvbo_zl,YUZHI2);
        //        UPDW[col, 0] = yy_lvboUPDW[0];
        //        UPDW[col, 1] = yy_lvboUPDW[1]; 
        //        //保留阈值外的点
        //        for (long i = 0; i < this.ioriginal_num; i++)
        //        {
        //            if (yy_lvbo[i] > yy_lvboUPDW[0])
        //            {
        //                y_cidao[col, i] = yy_lvbo[i];
        //                yy_lvbo_yz[i] = yy_lvbo[i] - yy_lvboUPDW[0];
        //            }
        //            else if (yy_lvbo[i] < yy_lvboUPDW[1])
        //            {
        //                y_cidao[col, i] = yy_lvbo[i];
        //                yy_lvbo_yz[i] = yy_lvboUPDW[1] - yy_lvbo[i];
        //            }
        //            else
        //            {
        //                y_cidao[col, i] = yy_lvbo[i];
        //                yy_lvbo_yz[i] = 0;
        //            }
        //        }
               
        //        //腐蚀深度
        //        for (long i = 0; i < this.ioriginal_num; i++)
        //        {
        //            DepthOfCorr[col, i] = 0.061484 * (Math.Exp(0.00104*yy_lvbo_yz[i])-1) * this.ThickOftube;
        //            DepthOfCorr[col, i] = DepthOfCorr[col, i] <= 0 ? 0 : DepthOfCorr[col, i];
        //            DepthOfCorr[col, i] = DepthOfCorr[col, i] >= this.ThickOftube ? this.ThickOftube : DepthOfCorr[col, i];
        //        }

        //    }
                
        //    return DepthOfCorr;                        
        //}        


        //计算腐蚀深度
        private double[,] DepthOfCorrosion()
        {
            //double Liftoff = 5;     //提离高度为5mm
            double[] yy_lvboUPDW = new double[2];
            //long fengdian_num = 0;
            //double dy_lvboall = 0, dy_lvbomean = 0, dy_lvbostdall = 0, dy_lvbostd = 0, dy_lvboUP = 0, dy_lvboDW = 0; //求滤波导数上下限相关变量
            //long x1 = 0, x2 = 0;
            //double y1 = 0, y2 = 0;
            double[,] y_cidao = new double[this.tongdao_num, this.ioriginal_num - 1];
            double[,] DepthOfCorr = new double[this.tongdao_num, this.ioriginal_num];
            double[,] DepthOfCorr1 = new double[this.tongdao_num, this.ioriginal_num]; //用于缓存数据
            double[,] y_lvbo = new double[this.tongdao_num, this.ioriginal_num - 1];
            double[] dy_lvbo = new double[this.ioriginal_num - 1];
            long[] dy_lvbox = new long[this.ioriginal_num - 1];
            long[] fengdian_x = new long[this.ioriginal_num - 1]; ;
            double[] fengdian_y = new double[this.ioriginal_num - 1]; ;
            y_lvbo = this.Lvbo_fft();
            //int yvzhi = 13;

            double[] paixuy = new double[2];   // 排序后的数字
            y_lvbo = this.Lvbo_fft();
            //paixuy = this.sortdescend();
            double[,] yvzhi = new double[2, this.tongdao_num];
            yvzhi = this.caculateyvzhixian();

            int ydgs = 7;  //移动个数
            ////统计个数超阈值线连续续
            int[,] linshiarry = new int[this.tongdao_num, this.ioriginal_num];  //临时存储0,1用于判断用的
            //int changdu1 = (int)(0.041 * this.ioriginal_num);  //6cm长度标准，比例长度是148cm
            //int changdu2 = (int)(0.095 * this.ioriginal_num);   //14cm长度标准，比例长度是148cm
            int changdu1 = (int)(0.03 * this.ioriginal_num);  //6cm长度标准，比例长度是148cm
            int changdu2 = (int)(0.05 * this.ioriginal_num);   //14cm长度标准，比例长度是148cm
            //int changdu1 = 4;  //10cm长度标准，比例长度是138cm
            //int changdu2 = 8;   //20cm长度标准，比例长度是138cm
            int before = 0; //前一个数据
            int[,] lianxucount_x1 = new int[this.tongdao_num, 100]; //连续出现的个数超出长度1的x坐标
            int[,] lianxucountgeshu_x1 = new int[this.tongdao_num, 100];  //用于存储超出的个数
            int[,] lianxucount_x2 = new int[this.tongdao_num, 100]; //连续出现的个数超出长度2的x坐标
            int[,] lianxucountgeshu_x2 = new int[this.tongdao_num, 100];
            int tempcount = 0;//临时缓存个数
            int temp = 0; //临时缓存用于比较
            int[] k1 = new int[this.tongdao_num];  //用于统计每个通道符合长度1的个数
            int[] k2 = new int[this.tongdao_num];   // 用于统计每个通道符合长度2的个数
            ////

            //for (int col = 0; col < this.tongdao_num; col++)
            //{
            //    //求滤波曲线一阶导数
            //    for (long i = 0; i < this.ioriginal_num - 1; i++)
            //    {
            //        //dy_lvbo[i] = y_lvbo[col, i + 1] - y_lvbo[col, i];  
            //        dy_lvbo[i] = y_lvbo[col, i + 1];   //不做差
            //        dy_lvbox[i] = i;
            //    }
            //    //  //求原始曲线与滤波曲线插值上下限
            //    //yy_lvboUPDW = dbdt(dy_lvbo, YUZHI);
            //    //UPDW[col, 0] = yy_lvboUPDW[0];
            //    //UPDW[col, 1] = yy_lvboUPDW[1];
            //    //滤波曲线峰值点
            //    int k = 0;
            //    for (long i = 1; i < this.ioriginal_num - 2; i++)
            //    {
            //        if ((dy_lvbo[i] >= dy_lvbo[i - 1] && dy_lvbo[i] >= dy_lvbo[i + 1]) || (dy_lvbo[i] <= dy_lvbo[i - 1] && dy_lvbo[i] <= dy_lvbo[i + 1]))
            //        {
            //            fengdian_x[k] = i;
            //            fengdian_y[k] = dy_lvbo[i];
            //            k++;
            //        }
            //    }
            //    fengdian_num = k;
            //    //求滤波上下限
            //    dy_lvboall = 0;
            //    dy_lvbomean = 0;
            //    for (long i = 0; i < this.ioriginal_num - 1; i++)
            //    {
            //        dy_lvboall += dy_lvbo[i];
            //    }
            //    dy_lvbomean = dy_lvboall / (this.ioriginal_num - 1);
            //    dy_lvbostdall = 0;
            //    dy_lvbostd = 0;
            //    for (long i = 0; i < this.ioriginal_num - 1; i++)
            //    {
            //        dy_lvbostdall = dy_lvbostdall + Math.Pow((dy_lvbo[i] - dy_lvbomean), 2);
            //    }
            //    dy_lvbostd = Math.Sqrt(dy_lvbostdall / (this.ioriginal_num - 1));
            //    dy_lvboUP = dy_lvbomean + YUZHI * dy_lvbostd;
            //    dy_lvboDW = dy_lvbomean - YUZHI * dy_lvbostd;
            //    //UPDW[col, 0] = dy_lvboUP;
            //    //UPDW[col, 1] = dy_lvboDW;
            //    //求成像的滤波曲线梯度
            //    for (long i = 0; i <= fengdian_x[0]; i++)   //首
            //    {
            //        y_cidao[col, i] = 1;
            //    }

            //    for (long i = 0; i < fengdian_num - 1; i++)   //中
            //    {
            //        if (fengdian_y[i] >= dy_lvboUP && fengdian_y[i + 1] <= dy_lvboDW)
            //        {
            //            x1 = fengdian_x[i] + 1;
            //            x2 = fengdian_x[i + 1];
            //            y1 = fengdian_y[i] - dy_lvboUP;
            //            y2 = dy_lvboDW - fengdian_y[i + 1];
            //            for (long j = x1; j <= x2; j++)
            //            {
            //                y_cidao[col, j] = y1 + (y2 - y1) * (j - x1) / (x2 - x1) + 1;
            //            }
            //        }
            //        else if (fengdian_y[i] <= dy_lvboDW && fengdian_y[i + 1] >= dy_lvboUP)
            //        {
            //            x1 = fengdian_x[i] + 1;
            //            x2 = fengdian_x[i + 1];
            //            y1 = dy_lvboDW - fengdian_y[i];
            //            y2 = fengdian_y[i + 1] - dy_lvboUP;
            //            for (long j = x1; j <= x2; j++)
            //            {
            //                y_cidao[col, j] = y1 + (y2 - y1) * (j - x1) / (x2 - x1) + 1;
            //            }
            //        }
            //        else
            //        {
            //            for (long j = fengdian_x[i] + 1; j <= fengdian_x[i + 1]; j++)
            //            {
            //                if (dy_lvbo[j] >= dy_lvboUP)
            //                {
            //                    y_cidao[col, j] = dy_lvbo[j] - dy_lvboUP + 1;
            //                }
            //                else if (dy_lvbo[j] <= dy_lvboDW)
            //                {
            //                    y_cidao[col, j] = dy_lvboDW - dy_lvbo[j] + 1;
            //                }
            //                else
            //                {
            //                    y_cidao[col, j] = 1;
            //                }
            //            }
            //        }
            //    }

            //    for (long i = fengdian_x[fengdian_num - 1] + 1; i < this.ioriginal_num - 1; i++) //尾
            //    {
            //        y_cidao[col, i] = 1;
            //    }
            //    //腐蚀深度
            //    for (long i = 0; i < this.ioriginal_num - 1; i++)
            //    {
            //        //DepthOfCorr[col, i] = 0.0171 * Math.Log(0.0228 * y_cidao[col, i]) * this.Liftoff;
            //        DepthOfCorr[col, i] = 0.0171 * Math.Log(0.5 * y_cidao[col, i]) * this.Liftoff;
            //        DepthOfCorr[col, i] = DepthOfCorr[col, i] <= 0 ? 0 : DepthOfCorr[col, i];
            //        DepthOfCorr[col, i] = DepthOfCorr[col, i] >= this.ThickOftube ? this.ThickOftube : DepthOfCorr[col, i];
            //    }
            //}

            for (int col = 0; col < this.tongdao_num; col++)
            {
                //for (int i = 0; i < (int)(0.067 * this.ioriginal_num); i++)   //端头10为0
                //{
                //    //DepthOfCorr1[col, i] = 0;
                //    DepthOfCorr[col, i] = 0;
                //}

                //for (int i = (int)(0.93 * this.ioriginal_num); i < this.ioriginal_num; i++)  //端尾为10为0
                //{
                //    //DepthOfCorr1[col, i] = 0;
                //    DepthOfCorr[col, i] = 0;
                //}

                //for (long i = 0; i < this.ioriginal_num; i++)
                //for (int i = (int)(0.067 * this.ioriginal_num); i < (int)(0.93 * this.ioriginal_num); i++)   //端头10cm不需要
                  for (int i = 0; i < (int)( this.ioriginal_num); i++)   //端头10cm不需要
                {


                    DepthOfCorr[col, i] = Math.Abs(y_lvbo[col, i]);   //不存在对调通道


                }  
                //for (int i = (int)(0.18 * this.ioriginal_num); i < (int)(0.22 * this.ioriginal_num); i++)   //30cm处
                //{

                //    DepthOfCorr[col, i] = Math.Abs(y_lvbo[col, i]);   

                //}
                //for (int i = (int)(0.35 * this.ioriginal_num); i < (int)(0.39 * this.ioriginal_num); i++)   //55cm处
                //{

                //    DepthOfCorr[col, i] = Math.Abs(y_lvbo[col, i]);   

                //}
                //for (int i = (int)(0.49 * this.ioriginal_num); i < (int)(0.53 * this.ioriginal_num); i++)   //75cm处
                //{

                //    DepthOfCorr[col, i] = Math.Abs(y_lvbo[col, i]);   

                //}
                //for (int i = (int)(0.62 * this.ioriginal_num); i < (int)(0.66 * this.ioriginal_num); i++)   //95cm处
                //{

                //    DepthOfCorr[col, i] = Math.Abs(y_lvbo[col, i]);   

                //}
                //for (int i = (int)(0.77 * this.ioriginal_num); i < (int)(0.81 * this.ioriginal_num); i++)   //118cm处
                //{

                //    DepthOfCorr[col, i] = Math.Abs(y_lvbo[col, i]);  

                //}  
            }
            ///从这里开始注释6.4号
            ////将超出阈值线的和没有超出阈值线的赋予不同的值，便于区分
            //for (int col = 0; col < this.tongdao_num; col++)
            //{

            //    //for (int i = 0; i < this.ioriginal_num; i++)   //去端头
            //    //{
            //    //    if ((DepthOfCorr1[col, i] > yvzhi) || DepthOfCorr1[col, i] < -yvzhi)   //超阈值线的数为0
            //    //    {
            //    //        linshiarry[col, i] = 1;
            //    //    }
            //    //    else
            //    //    {
            //    //        linshiarry[col, i] = 0;
            //    //    }
            //    //}
            //    for (int i = 1; i < this.ioriginal_num - 1; i++)   //去端头
            //    {
            //        //if (((DepthOfCorr1[col, i - 1] > yvzhi) || DepthOfCorr1[col, i-1] < yvzhi) && ((DepthOfCorr1[col, i + 1] > yvzhi) || DepthOfCorr1[col, i+1] < -yvzhi))   //格点判断
            //        if (((DepthOfCorr1[col, i - 1] > yvzhi[1,col]) || DepthOfCorr1[col, i - 1] < yvzhi[0,col]) && ((DepthOfCorr1[col, i + 1] > yvzhi[1,col]) || DepthOfCorr1[col, i + 1] < yvzhi[0,col]))   //格点判断
            //        {
            //            linshiarry[col, i - 1] = 1;  ///三个都赋予大于阈值
            //            linshiarry[col, i] = 1;
            //            linshiarry[col, i + 1] = 1;
            //            i++; //这里要自加1，应为要跳过避免出现0
            //        }
            //        //else if (((DepthOfCorr1[col, i - 1] > yvzhi) || DepthOfCorr1[col, i - 1] < -yvzhi))  //没必要这些代码，这样反而会加大运算
            //        //{
            //        //    linshiarry[col, i-1] = 1;
            //        //}
            //        //else if (((DepthOfCorr1[col, i ] > yvzhi) || DepthOfCorr1[col, i ] < -yvzhi))
            //        //{
            //        //    linshiarry[col, i ] = 1;
            //        //}
            //        //else if (((DepthOfCorr1[col, i + 1] > yvzhi) || DepthOfCorr1[col, i + 1] < -yvzhi))
            //        //{
            //        //    linshiarry[col, i + 1] = 1;
            //        //}
            //        else
            //        {
            //            linshiarry[col, i] = 0;
            //        }
            //    }
            //}
            ///////
            //////判断连续超出规定个数的起点x坐标,并且记下超过多少个
            //for (int col = 0; col < this.tongdao_num; col++)
            //{
            //    lianxucount_x1[col, 0] = 0;  //连续出现超过指定长度的起始x坐标位置
            //    lianxucount_x2[col, 0] = 0;
            //    int xk = 0;  //长度1的临时系数x
            //    int xk2 = 0;  //长度2的临时系数x
            //    before = 1; //需要before 为1时，才认为是连续
            //    tempcount = 0;
            //    int xzuo1 = 0;//临时存储x坐标的位置
            //    for (int i = 0; i < this.ioriginal_num; i++)
            //    {
            //        temp = linshiarry[col, i];
            //        if (temp == before)
            //        {
            //            tempcount++;
            //            if ((tempcount >= changdu1) && (tempcount < changdu2))  //长度在长度1和长度2之间
            //            {
            //                xzuo1 = i - tempcount;  //
            //                if (xk > 0)
            //                {
            //                    if (lianxucount_x1[col, xk - 1] != xzuo1)  //满足重新一次统计长度的情况
            //                    {
            //                        lianxucount_x1[col, xk] = xzuo1;
            //                        lianxucountgeshu_x1[col, xk] = tempcount;
            //                        xk++;
            //                        k1[col] = xk;    //因为这是从0开始计算，所以要在++之后开始计算
            //                    }
            //                    else if ((lianxucount_x1[col, xk - 1] == xzuo1) && (lianxucountgeshu_x1[col, xk - 1] != tempcount ))//仍然满足上一次统计，且长度在增长
            //                    {
            //                        lianxucountgeshu_x1[col, xk-1] = tempcount;
            //                    }
                               
            //                }
            //                else  //第一次统计的时候，即为0的时候
            //                {
                                
            //                        lianxucount_x1[col, 0] = xzuo1; //特殊情况当其为0的时候
            //                        lianxucountgeshu_x1[col, xk] = tempcount;
            //                        xk++;
            //                        k1[col] = xk;
                                 
            //                }
            //            }
            //            /////判断大于长度2时的情况
            //            if (tempcount >= changdu2)
            //            {
            //                xzuo1 = i - tempcount;
            //                if (xk2 > 0)
            //                {
            //                    if (lianxucount_x2[col, xk2 - 1] != xzuo1)
            //                    {
            //                        lianxucount_x2[col, xk2] = xzuo1;
            //                        lianxucountgeshu_x2[col, xk2] = tempcount;
            //                        xk2++;
            //                        k2[col] = xk2;
            //                        xk--;  //长度1的缺陷就要减掉一个
            //                        lianxucount_x1[col, xk] = 0; //将其清0
            //                        lianxucountgeshu_x1[col, xk] = 0;
            //                        k1[col] = xk;
            //                    }
            //                    else if ((lianxucount_x2[col, xk2 - 1] == xzuo1) && (lianxucountgeshu_x2[col, xk2 - 1] != tempcount))//仍然满足上一次统计，且长度在增长
            //                    {
            //                        lianxucountgeshu_x2[col, xk2 - 1] = tempcount;
            //                    }
            //                }
            //                else
            //                {
            //                    lianxucount_x2[col, 0] = xzuo1; //特殊情况当其为0的时候
            //                    lianxucountgeshu_x2[col, xk2] = tempcount;
            //                    xk2++;
            //                    k2[col] = xk2;
            //                    xk--;  //长度1的缺陷就要减掉一个
            //                    lianxucount_x1[col, xk] = 0;
            //                    lianxucountgeshu_x1[col, xk] = 0;
            //                    k1[col] = xk;
            //                }

            //            }

            //        }
            //        else  //当一有临时数值为0，就令tempcount为0
            //        {
            //            tempcount = 0;

            //        }
            //    }
            //}
            ///////////
            //////////将满足长度1要求的赋予实际值
            //for (int col = 0; col < this.tongdao_num; col++)
            //{
            //    for (int i = 0; i < k1[col]; i++)
            //    {
            //        for (int j = (lianxucount_x1[col, i] + (int)(0.4 * lianxucountgeshu_x1[col, i])); j < (lianxucount_x1[col, i] + (int)(0.6 * lianxucountgeshu_x1[col, i])); j++) //给长度1部分的赋予一定的值
            //        {
            //            //if (DepthOfCorr1[col, j] != 0)
            //            //{
            //            //    DepthOfCorr[col, j] = Math.Log(Math.Abs(DepthOfCorr1[col, j]));
            //            //}
            //            DepthOfCorr[col, j] = 1;  
            //        }
            //    }
            //}

            ///////将满足长度1要求的赋予实际值
            //////////将满足长度2要求的赋予实际值
            //for (int col = 0; col < this.tongdao_num; col++)
            //{
            //    for (int i = 0; i < k2[col]; i++)
            //    {
            //        for (int j = (int)((lianxucount_x2[col, i] + 0.5 * lianxucountgeshu_x2[col, i] - 0.5 * changdu2)); j < (int)((lianxucount_x2[col, i] + 0.5 * lianxucountgeshu_x2[col, i] + 0.5 * changdu2)); j++) //当为长度2时，并且使其居中
            //        {
            //            //if (DepthOfCorr1[col, j] != 0)
            //            //{
            //            //    DepthOfCorr[col, j] = Math.Log(Math.Abs(DepthOfCorr1[col, j]));
            //            //}
            //           DepthOfCorr[col, j] = 1;
            //        }

            //    }
            //}

            ///////将满足长度2要求的赋予实际值

            //////

            ////for (int col = 0; col < this.tongdao_num; col++)
            ////{
            ////    for (int i = 0; i < k-1; i++)
            ////    {
            ////        //if (((yshuzhi[col, i] > yvzhi[1, col]) && (yshuzhi[col, i + 1] < yvzhi[0, col])) || ((yshuzhi[col, i] < yvzhi[0, col]) && (yshuzhi[col, i + 1] > yvzhi[1, col])))
            ////        if (((yshuzhi[col, i] > yvzhi ) && (yshuzhi[col, i + 1] < -yvzhi )) || ((yshuzhi[col, i] < -yvzhi ) && (yshuzhi[col, i + 1] > yvzhi )))
            ////        {
            ////            if (Math.Abs(xshuzhi[col, i] - xshuzhi[col, i + 1]) < 10)//波峰与波谷相隔10个数据，也就是1秒采集的数据量
            ////            {
            ////                for (int j = (int)xshuzhi[col, i]; j < xshuzhi[col, i + 1]; j++)
            ////                {
            ////                    if (y_lvbo[col, (int)xshuzhi[col, i]] != 0)
            ////                    {
            ////                        DepthOfCorr[col, j] = Math.Log(Math.Abs((y_lvbo[col, (int)xshuzhi[col, i]])));
            ////                    }
            ////                    else
            ////                    {
            ////                        DepthOfCorr[col, j] = 0;
            ////                    }
            ////                }
            ////            }
            ////        }
            ////    }
            ////}

           /////6.20号注释


            return DepthOfCorr;
        }






        ////求原始曲线与滤波曲线插值上下限
        //private double[] dbdt(double[] yy, double yuzhi)
        //{
        //    double y_all = 0, y_mean = 0, y_stdall = 0, y_std = 0; //求滤波导数上下限相关变量            
        //    double[] y_UPDW = new double[2];
        //    //求原始曲线与滤波曲线插值上下限
        //    y_all = 0;
        //    y_mean = 0;
        //    for (long i = 0; i < yy.Length; i++)
        //    {
        //        y_all += yy[i];
        //    }
        //    y_mean = y_all / yy.Length;
        //    y_stdall = 0;
        //    y_std = 0;
        //    for (long i = 0; i < this.ioriginal_num - 1; i++)
        //    {
        //        y_stdall = y_stdall + Math.Pow((yy[i] - y_mean), 2);
        //    }
        //    y_std = Math.Sqrt(y_stdall / yy.Length);
        //    y_UPDW[0] = y_mean + yuzhi * y_std;
        //    y_UPDW[1] = y_mean - yuzhi * y_std;
        //    return y_UPDW;
        //}
                    
        ////线性插值
        //public double[,] col_lip()
        //{            
        //    double y1temp=0,y2temp=0;
        //    int x1temp=0,x2temp=0;
        //    double [,] DepthOfC=new double [this.tongdao_num,this.ioriginal_num];
        //    double[,] V = new double[this.tongdao_num*10, this.ioriginal_num];
        //    DepthOfC=this.DepthOfCorrosion();
        //    for (long line = 0; line < this.ioriginal_num; line++)
        //    {
        //        for (int i = 0; i < this.tongdao_num-1; i++)
        //        {
        //            y1temp = DepthOfC[i, line];
        //            y2temp = DepthOfC[i+1, line];
        //            x1temp=i*10;
        //            x2temp=(i+1)*10-1;
        //            for (int j = 0; j < 10; j++)
        //            {
        //                V[x1temp + j, line] = y1temp + j * (y2temp - y1temp) / (x2temp - x1temp);
        //            }
        //        }
        //    }
        //    return V;
        //}

        //线性插值
        public double[,] col_lip()
        {
            double y1temp = 0, y2temp = 0;
            int x1temp = 0, x2temp = 0;
            double[,] DepthOfC = new double[this.tongdao_num, this.ioriginal_num];
            double[,] V = new double[this.tongdao_num * 10, this.ioriginal_num];
            //double[,] V = new double[this.tongdao_num * 20, this.ioriginal_num];
            DepthOfC = this.DepthOfCorrosion();
            double Vmax = 0 ;  //定义0为double的最大值
            ////
            //double zuixiaobiaozun = 3;  //定义最小标准，防止出现特定参数空采也出彩图
            ///////
            ///////
            //for (long j = 0; j < this.ioriginal_num - 1; j++)
            //{
            //    for (int i = 0; i < this.tongdao_num - 1; i++)
            //    {
            //         if(DepthOfC [i,j]<zuixiaobiaozun ) //如果小于最小标准，这其值为0
            //         {
            //             DepthOfC[i, j] = 0;
            //         }
            //    }
            //}
            ////////

            for (long line = 0; line < this.ioriginal_num-1; line++)
            {
                for (int i = 0; i < this.tongdao_num - 1; i++)
                {
                    y1temp = DepthOfC[i, line];
                    y2temp = DepthOfC[i + 1, line];
                    x1temp = i * 10;
                    x2temp = (i + 1) * 10 - 1;
                    for (int j = 0; j < 10; j++)
                    //x1temp = i * 20;
                    //x2temp = (i + 1) * 20 - 1;
                    //for (int j = 0; j < 20; j++)
                    {
                        V[x1temp + j, line] = y1temp + j * (y2temp - y1temp) / (x2temp - x1temp);
                    }
                }
            }
            /////查找最大值
            for (long line = 0; line < this.ioriginal_num - 1; line++)
            {
                for (int i = 0; i < 10*this.tongdao_num - 1; i++)
                {
                    if (Math.Abs(V[i, line]) > Vmax)
                    {
                        Vmax = Math.Abs(V[i, line]);
                    }
                }
            }
            V[0, 0] = Vmax;  //另其起点问为最大方便调用
            /////
            return V;
        }

        ////降序排列
        //private double[] sortdescend()  //将每个通道中最大的数按从大到小排列
        //{
        //    double[,] y_lvbo = new double[this.tongdao_num, this.ioriginal_num];
        //    double[,] sorty = new double[this.tongdao_num, this.ioriginal_num];
        //    double[] sortyuanshi = new double[this.ioriginal_num];
        //    double[] sorttongdaot = new double[this.tongdao_num];  //通道开头的元素排序
        //    double[] sorttongdaow = new double[this.tongdao_num];
        //    double[] tongdao = new double[this.tongdao_num]; //通道头和尾的数组
        //    double[] sorttw = new double[2];  //排序后的最大最小值
        //    //double [ ] sortarry = new double [this .ioriginal_num ];
        //    y_lvbo = this.Lvbo_fft();
        //    for (int i = 0; i < this.tongdao_num; i++)
        //    {
        //        for (int j = 0; j < this.ioriginal_num; j++)
        //        {
        //            sortyuanshi[j] = y_lvbo[i, j];
        //        }
        //        Array.Sort(sortyuanshi);
        //        for (int j = 0; j < this.ioriginal_num; j++)
        //        {
        //            sorty[i, j] = sortyuanshi[j];   //使的每行的开头最大
        //        }
        //    }
        //    //寻求最小值
        //    for (int col = 0; col < this.tongdao_num; col++)
        //    {
        //        tongdao[col] = sorty[col, 0];
        //    }
        //    Array.Sort(tongdao);
        //    for (int col = 0; col < this.tongdao_num; col++)
        //    {
        //        sorttongdaot[col] = tongdao[col];  //最小值的序列
        //    }
        //    ////
        //    ////寻求最大值
        //    for (int col = 0; col < this.tongdao_num; col++)
        //    {
        //        tongdao[col] = sorty[col, this.ioriginal_num - 1];
        //    }
        //    Array.Sort(tongdao);
        //    for (int col = 0; col < this.tongdao_num; col++)
        //    {
        //        sorttongdaow[col] = tongdao[col];  //最大值的序列
        //    }
        //    /////
        //    sorttw[0] = sorttongdaot[0];
        //    sorttw[1] = sorttongdaow[this.tongdao_num - 1];
        //    return sorttw;

        //}

        private double [ ,] caculateyvzhixian()   //计算与阈值线
        {
            double[,] y_lvbo = new double[this.tongdao_num, this.ioriginal_num];
            double[] yvzhi_up = new double[this.tongdao_num];
            double[] yvzhi_dw = new double[this.tongdao_num];
            double y_lvbosum=0;
            //double y_lvbomean = 0;
            double[,] y_lvbomean = new double[1, this.tongdao_num];
            double ystdsum = 0;
            double ystd = 0;
            double yvzhixian=0.3;
            double [,] yvzhi=new double [2,this.tongdao_num ];
            y_lvbo = this.Lvbo_fft();

            for (int i = 0; i < this.tongdao_num; i++)
            {
                y_lvbosum = 0;//每次循法前都清零
                for (int j = 0; j < this.ioriginal_num; j++)
                {
                    y_lvbosum +=Math.Abs (y_lvbo[i, j]);
                }
                y_lvbomean[0,i]= y_lvbosum / this.ioriginal_num;
                //for (int j = 0; j < this.ioriginal_num; j++)
                //{
                //    ystdsum += Math.Pow((y_lvbo[i,j] - y_lvbomean), 2);
                //}
                //  ystd = Math .Sqrt ( ystdsum / this.ioriginal_num);
                //  yvzhi_dw[i] = y_lvbomean - yvzhixian * ystd;
                //  yvzhi_up[i] = y_lvbomean + yvzhixian * ystd;
                //  yvzhi[0, i] = yvzhi_dw[i];  //下阈值线
                //  yvzhi[1, i] = yvzhi_up[i];  //上阈值线
                yvzhi[0, i] = -y_lvbomean[0, i];  //下阈值线
                yvzhi[1, i] = y_lvbomean[0, i];  //上阈值线
            }

            //return yvzhi;
            return yvzhi;

        }



    }
}
