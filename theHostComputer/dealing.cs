using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

using ZedGraph;
using System.Threading;

namespace theHostComputer
{
    public partial class dealing : Form
    {
  
        //Dog dog = new Dog();//看门狗
        C_Code Code = new C_Code();  //解码类
        CFunction2 Fun_2 = new CFunction2();  //计算腐蚀深度类        
        private Thread thread1;

        private string FilePath_3;  //数据文件路径         

        private long[,] FADData = new long[2, 6];    //解码后的各个探头磁场数据
        private double[] FRealData = new double[5];    //计算得出的最终可用的磁场数据

        private long[] FRealDist = new long[1];          //探头移动距离              
        private C_Pubdef.TCalData FCalData = new C_Pubdef.TCalData();   // 各个探头的标定数据

        /// <summary>
        private double[,] FRealData1 = new double[2, 6];    //过度用的
        private double[] B = new double[9];
        private double[,] zhangliang = new double[9, 1000000];
        private double[] Bni = new double[9];//磁梯度张量梯度的逆矩阵
        private double[] R1jul = new double[3];//一号探头的3个距离
        private double[,] distxyz = new double[16, 1000000];  //每个探头保存4列数据，即4*4=16列
        private double[,] Dydata2 = new double[12, 1000000]; //原始数据保存

        private double[,] ysdata = new double[12, 1000000]; //读原始数据并保存
        private double[,] ysdata1 = new double[12, 1000000]; //读原始数据并保存
        private double mean = 0;
        private double mean1 = 0;
        private double[,] ysdatanew = new double[4, 1000000];//四向下的通道新值
        private double F;
        private double F1;
        private double F2;
        private double F3;
        private double F4;

        /// </summary>

        PointPairList list1 = new PointPairList();
        PointPairList list2 = new PointPairList();
        PointPairList list3 = new PointPairList();
        PointPairList list4 = new PointPairList();
        PointPairList list5 = new PointPairList();
        PointPairList list6 = new PointPairList();
        PointPairList list7 = new PointPairList();
        PointPairList list8 = new PointPairList();
        PointPairList list9 = new PointPairList();
        PointPairList list10 = new PointPairList();
        PointPairList list11 = new PointPairList();
        PointPairList list12 = new PointPairList();
        PointPairList list13 = new PointPairList();
        private double[,] cidao = new double[12, 1000000];
        private double[,] Freaddata = new double[12, 100000];  //预览出来的数据
        //private double [] Freadellip=new double [1000000];
        private long original_num = 0;
        private int tongdao_num = 0;     //通道数
        /// <summary>曲线成阈值线
        private double[,] Freaddata2 = new double[12, 100000];  //临时借用
        private double[,] Freaddata3 = new double[12, 100000];  //判断保存用
        private double[,] Freaddata4 = new double[12, 100000]; //  用于保存合并5个通道的情况
        private double[,] yvzhi = new double[2, 6];  //阈值1
        private double[,] yvzhi2 = new double[2, 6];  //阈值2
        /// </summary>

        private long Row = 0;
        private long RRow = 0;

        private bool linkflag = false;
        private bool startflag = false;
        private bool chselectokflag = false;
        private bool browseflag = false;
        private bool runflag = false;
        private bool[] chflag = new bool[12]; //通道选取标志       

        private double lenofpiece = 0;    //工件长度(实际扫查长度)
        private double widthOfPiece = 0;    //工件宽 
        private double ThickOftube = 0;    //管道厚度   
        ////////
        private double lenofpiece1 = 0;  //工件的实际长度
        private double kuangjiachangdu = 0;// 框架的长度0cm
        ///////

        private double DXMAX = 200;    //动态图X刻度最大值
        private double DYMAX = 100;      //动态图Y刻度最大值
        private double DXMAXStep = 20;  //动态图X刻度最小值
        private double picXMAX = 200;
        private double SMax = 1000000;

        //梯度相关变量
        private double[,] Dydata = new double[12, 100000];      //原始数据
        private double[] dy_originalUP = new double[12];           //原始梯度上限
        private double[] dy_originalDW = new double[12];         //原始梯度下限                                             

        // 等值线对应的颜色
        List<System.Drawing.Color> list_Color_2 = new List<System.Drawing.Color>();
        //结果等值线图相关变量
        private double picDX_2 = 0, picDY_2 = 0;
        //缺陷定位相关变量
        private Point currentPoint = new Point(0, 0);
        private bool pointflag = false;
        private bool readflag = false;


        ////包覆层管道
        private double tiligaodu = 0;
        private double tiligaodu1 = 0;
        private double yangguangleixin = 2;   //样管类型
        private double yangguangcheck = 1;  //yangguanqueding(da or xiao) 默认改为1（即大缺陷的形式）
        private double qccd = 100; //去除长度定义为整体变量(默认为100)
        private double qccd1 = 450; //针对7mm厚管壁
        private double qccd2 = 200; //针对5mm厚管壁
        private int chufa = 0;  //判断是否触发了某控件，默认为0，即未触发。
        private double fangxian = 0;   //5mm管道fangxian,默认为某个方向
        private double diejia7mm = 0;  //默认该参数为0
        Int32 length = 0;
        
        private int browsetime = 0;
       private long[] original_allnum = new long[1000];
      
        

        public dealing()
        {
            InitializeComponent();
            original_allnum[0] = 0;
        }



        private void browseButton_3_Click(object sender, EventArgs e)
        {browsetime++;
        string browsetimeBox2 = browsetime.ToString();
        browsetimeBox.Text = browsetimeBox2+"组";
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "文本文件 (*.txt)|*.txt";
                open.FilterIndex = 1;
                open.RestoreDirectory = true;
                open.Title = "打开数据文件";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    FilePath_3 = open.FileName;
                    FileText_3.Text = FilePath_3;
                    readdata();
                    runButton_3.Enabled = true;
                    //browseflag = true;
                    //runflag = false;
                }
            }
        }
        private void readdata()
        {
            System.IO.StreamReader sr_ScatterData = new System.IO.StreamReader(FilePath_3);
            string temp_str = sr_ScatterData.ReadLine();  //第一行 工件号
            temp_str.TrimStart();
            temp_str.TrimEnd();
            string[] tempData_str = temp_str.Split(' ').ToArray();
            NoText_3.Text = tempData_str[1];

            //readflag = true;

            sr_ScatterData.ReadLine();   //第三行 检测时间
            temp_str = sr_ScatterData.ReadLine();  //第四行 规格和通道数
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            DisText_3.Text = tempData_str[1];

            
            length =length + Convert.ToInt32(DisText_3.Text) ;

            length_all.Text = length.ToString();
            //lenofpiece1 = double.Parse(tempData_str[1]);
            //lenofpiece = lenofpiece1 - kuangjiachangdu;


            DText_3.Text = tempData_str[3];
            TText_3.Text = tempData_str[5];
            TLBox_3.Text = tempData_str[7];
            cdtext_3.Text = tempData_str[9];
            qccdtext_3.Text = tempData_str[11];

            widthOfPiece = Math.PI * double.Parse(tempData_str[3]);
            ThickOftube = double.Parse(tempData_str[5]);
            tongdao_num = 12;
            tiligaodu = double.Parse(tempData_str[7]);
            long k = 0;
            while (!sr_ScatterData.EndOfStream)
            {
                temp_str = sr_ScatterData.ReadLine();
                temp_str.TrimStart();
                temp_str.TrimEnd();
                tempData_str = temp_str.Split(',').ToArray();

                if (tempData_str.Count() == 18)
                {
                    Dydata[0, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[1]);
                    Dydata[1, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[2]);
                    Dydata[2, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[3]);
                    Dydata[3, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[4]);
                    Dydata[4, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[5]);


                    //Freaddata2[2, k] = double.Parse(tempData_str[1]);   //通道改变
                    //Freaddata2[1, k] = double.Parse(tempData_str[2]);
                    //Freaddata2[3, k] = double.Parse(tempData_str[3]);
                    //Freaddata2[4, k] = double.Parse(tempData_str[4]);
                    //Freaddata2[0, k] = double.Parse(tempData_str[5]);


                    //Freaddata2[0, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[1]);   //通道改变
                    //Freaddata2[1, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[2]);
                    //Freaddata2[2, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[3]);
                    //Freaddata2[3, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[4]);
                    //Freaddata2[4, k + original_allnum[browsetime-1]] = double.Parse(tempData_str[5]);

                    ysdata[0, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[6]);
                    ysdata[1, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[7]);
                    ysdata[2, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[8]);
                    ysdata[3, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[9]);
                    ysdata[4, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[10]);
                    ysdata[5, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[11]);
                    ysdata[6, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[12]);
                    ysdata[7, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[13]);
                    ysdata[8, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[14]);
                    ysdata[9, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[15]);
                    ysdata[10, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[16]);
                    ysdata[11, k + original_allnum[browsetime - 1]] = double.Parse(tempData_str[17]);

                    mean = (ysdata[2, k] + ysdata[5, k] + ysdata[8, k] + ysdata[11, k]) / 4;

                    ysdatanew[0, k] = ysdata[2, k] - mean;
                    ysdatanew[1, k] = ysdata[5, k] - mean;
                    ysdatanew[2, k] = ysdata[8, k] - mean;
                    ysdatanew[3, k] = ysdata[11, k] - mean;

                    k++;
                }

                //if (tempData_str.Count() == 2)
                //{
                //    DisText_2.Text = tempData_str[1];


                //} 
            }
            original_num = k;
            original_allnum[browsetime] = original_allnum[browsetime] + k + original_allnum[browsetime-1];
            sr_ScatterData.Close();
        }

        private void SaveButton_3_Click(object sender, EventArgs e)
        {
            if (NoText_3.Text == "")
            {
                MessageBox.Show("请输入工件编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (DisText_3.Text == "")
            {
                MessageBox.Show("请输入检测长度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (TLBox_3.Text == "")
            {
                MessageBox.Show("请输入提离高度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //////

            string timestr;
            string pathtxt;
            FileStream fs;
            StreamWriter swt;

            timestr = DateTime.Now.ToString();
            timestr = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            pathtxt = Application.StartupPath + "\\" + "datafile" + "\\" + "综合数据" + NoText_3.Text + "# at " + timestr + "数据" + ".txt";
            fs = File.Open(pathtxt, FileMode.Create, FileAccess.Write);
            swt = new StreamWriter(fs, Encoding.Default);
            string txt;
            txt = "工件号：" + " " + NoText_3.Text;
            swt.WriteLine(txt);
            timestr = DateTime.Now.ToString();
            txt = "检测时间：" + " " + timestr;
            swt.WriteLine(txt);
            txt = "扫查长度：" + " " + DisText_3.Text + " " + "管道外径：" + " " + DText_3.Text + " " + "管道壁厚：" + " " + TText_3.Text + " " + "管道埋深：" + " " + TLBox_3.Text + " " + "判断长度：" + " " + cdtext_3.Text + " " + "去除长度：" + " " + qccdtext_3.Text;
            swt.WriteLine(txt);

            for (int i = 0; i < original_allnum[browsetime]; i++)  //第一个数据不需要
            {
                swt.Write(i);  //采点序号
                swt.Write(',');

                //5个独立张量值差分后值
                swt.Write(string.Format("{0:0.0}", Dydata[0, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[1, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[2, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[3, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[4, i]));
                swt.Write(',');



                //原始磁场值
                swt.Write(string.Format("{0:0.0}", ysdata[0, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[1, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[2, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[3, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[4, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[5, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[6, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[7, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[8, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[9, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[10, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", ysdata[11, i]));
                swt.Write("\r\n");
            }
            swt.Flush();
            swt.Close();
            fs.Close();
        }



        private void runButton_3_Click(object sender, EventArgs e)
        {
            draw_yoriginalline();
           
        }
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return "横坐标:" + string.Format("{0:0}", pt.X) + " 纵坐标:" + string.Format("{0:0.0}", pt.Y);
        }

        //调用使zedgraph显示坐标
        private void dealing_MouseMove(object sender, MouseEventArgs e)
        {
            zedGraphControl1.IsShowPointValues = true;  //动态磁场
            zedGraphControl2.IsShowPointValues = true;  //实时曲线


            zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);


        }        
        private void draw_yoriginalline()
        {
            //获取引用
            GraphPane myPane1 = zedGraphControl1.GraphPane;
            double x = 0;
            string CHname;

            //清空原图像
            myPane1.CurveList.Clear();
            myPane1.GraphObjList.Clear();
            zedGraphControl1.Refresh();

            //设置标题
            myPane1.Title.Text = "";
            //设置X轴说明文字
            myPane1.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane1.YAxis.Title.Text = "磁场变化/nT";

            myPane1.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane1.XAxis.Scale.Max = length;	//X轴最大值   
            myPane1.XAxis.Scale.MinorStep = 0;
            myPane1.XAxis.Scale.MajorStep = 500;




            PointPairList mylist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;


            
            for (int i = 0; i < original_num - 1; i++)
            {
                x = (float)i * length / (original_num - 1);
                if (Channel.SelectedIndex == 0)
                {
                    mylist0.Add(x, ysdata[0, i]);
                    mylist0.Add(x, ysdata[3, i]);
                    mylist0.Add(x, ysdata[6, i]);
                    mylist0.Add(x, ysdata[9, i]);
                }
                 if (Channel.SelectedIndex == 1)
                 {
                     mylist0.Add(x, ysdata[1, i]);
                     mylist0.Add(x, ysdata[4, i]);
                     mylist0.Add(x, ysdata[7, i]);
                     mylist0.Add(x, ysdata[10, i]);
                 }
                 if (Channel.SelectedIndex == 2)
                 {
                     mylist0.Add(x, ysdata[2, i]);
                     mylist0.Add(x, ysdata[5, i]);
                     mylist0.Add(x, ysdata[8, i]);
                     mylist0.Add(x, ysdata[11, i]);
                 }

            }

            CHname = "Abnormal Signal";
            myCurve0 = zedGraphControl1.GraphPane.AddCurve(CHname, mylist0, Color.Red, ZedGraph.SymbolType.None);

            myPane1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void Channel_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline();
        }
        //zedgraph显示坐标
        
    }
}


