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

using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZedGraph;

namespace theHostComputer
{
    public partial class walking : Form
    {
        //Dog dog = new Dog();//看门狗
        C_Code Code = new C_Code();  //解码类
        CFunction2 Fun_2 = new CFunction2();  //计算腐蚀深度类        
        private Thread thread1;
        private static IPEndPoint ipep;                //new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);                 
        private UdpClient udpClient = new UdpClient(50000);
        private string FilePath;  //数据文件路径  
       
        //页面刷新开关
        private bool refresh = false;

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
        private double[,] Freaddata = new double[12, 1000000];  //预览出来的数据
        //private double [] Freadellip=new double [100000];
        private long original_num = 0;
        private int tongdao_num = 0;     //通道数
        /// <summary>曲线成阈值线
        private double[,] Freaddata2 = new double[12, 1000000];  //临时借用
        private double[,] Freaddata3 = new double[12, 1000000];  //判断保存用
        private double[,] Freaddata4 = new double[12, 1000000]; //  用于保存合并5个通道的情况
        private double[,] yvzhi = new double[2, 6];  //阈值1
        private double[,] yvzhi2 = new double[2, 6];  //阈值2

        private double[] zhangliang_sigma = new double[9];
        private double[] zhangliang_sum = new double[9];
        private double[] zhangliang_biaozhunchasum = new double[9];
        private double[] zhangliang_mean = new double[9];
        private double[] zhangliang_max = new double[9];
        private double[] zhangliang_min = new double[9];

        private double[] Dydata_sigma = new double[9];
        private double[] Dydata_sum = new double[9];
        private double[] Dydata_biaozhunchasum = new double[9];
        private double[] Dydata_mean = new double[9];
        private double[] Dydata_max = new double[9];
        private double[] Dydata_min = new double[9];

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
        private double pic_2XMAX = 200;
        private double SMax = 100000;

        //梯度相关变量
        private double[,] Dydata = new double[12, 1000000];      //原始数据

        private double[] dy_originalUP = new double[12];           //原始梯度上限
        private double[] dy_originalDW = new double[12];         //原始梯度下限                                             

        // 等值线对应的颜色
        List<System.Drawing.Color> list_Color_2 = new List<System.Drawing.Color>();
        //结果等值线图相关变量
        private double pic_2DX_2 = 0, pic_2DY_2 = 0;
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

        double yzzhenliang = 0; //阈值增量
        ////
        public walking()
        {
            InitializeComponent();
        }

        private void linkButton_2_Click(object sender, EventArgs e)
        {
            ////IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
            if (linkButton_2.Text == "连接")
            {
                linkButton_2.Text = "断开";
                startButton_2.Enabled = true;
                byte[] bb = new byte[2] { 0x0, 0x0 };//定义一个数组用来做数据的缓冲区   
                udpClient.Send(bb, bb.Length, ipep);
                linkflag = true;
                if (startflag)
                {
                    thread1 = new Thread(new ThreadStart(ReceiveMessage));
                    thread1.IsBackground = true;
                    thread1.Start();
                }

            }
            else
            {
                linkButton_2.Text = "连接";
                startButton_2.Enabled = false;
                linkflag = false;
                if (startflag)
                {
                    thread1.Abort();                    
                }

            }
        }

        private void startButton_2_Click(object sender, EventArgs e)
        {
            //dog.isMyDog();//找狗
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
            if (startButton_2.Text == "开始")
            {
                if (DText_2.Text == "")
                {
                    MessageBox.Show("请选择管道外径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (TText_2.Text == "")
                {
                    MessageBox.Show("请选择管道壁厚！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                else
                {
                    startButton_2.Text = "停止";
                    linkButton_2.Enabled = false;
                    SaveButton_2.Enabled = false;
                    runButton_2.Enabled = false;
                    browseButton_2.Enabled = false;
                    PointButton_2.Enabled = false;

                    RatecomboBox_2.Enabled = false;
                    runflag = false;
                    tabControl_2.SelectedIndex = 0;
                    byte[] bb = new byte[4];  //定义一个数组用来做数据的缓冲区      
                    bb[0] = 0x1;
                    bb[1] = 0x0;
                    bb[2] = (byte)(RatecomboBox_2.SelectedIndex);
                    bb[3] = 0x0;
                    udpClient.Send(bb, bb.Length, ipep);

                    widthOfPiece = Math.PI * double.Parse(DText_2.Text);

                    startButton_2.BackColor = Color.GreenYellow;
                    //biao_D = double.Parse(DcomboBox.Text);                                
                    ThickOftube = double.Parse(TText_2.Text);    //管道厚度
                    //tongdao_num = int.Parse(ChnumcomboBox.Text);
                    tongdao_num = 12;
                    //DXMAX = double.Parse(WcomboBox.Text);
                    //pic_2XMAX = DXMAX;
                    //DXMAXStep = DXMAX / 10;
                    ZedGraphInit();       //曲线图初始化
                    //creatdatafile();  //建立文件路径                                                                  
                    listClear();
                    //creatdatafile1();  //建立文件路径                                                                  

                    //Dynamicpic_2Init();     //动态腐蚀图初始化、清图
                    selectokButton_Click(sender, e);  //动态腐蚀曲线加图例、清曲线  
                    Row = 0;
                    RRow = 0;
                    //SRow = 0;
                    startflag = true;

                    //接收数据
                    thread1 = new Thread(new ThreadStart(ReceiveMessage));
                    thread1.Name = "thread1";
                    thread1.Priority = ThreadPriority.Highest;
                    thread1.IsBackground = true;
                    thread1.Start();

                }
            }
            else
            {
                startButton_2.Text = "开始";
                startButton_2.BackColor = Color.Pink;
                linkButton_2.Enabled = true;
                SaveButton_2.Enabled = true;
                runButton_2.Enabled = true;
                browseButton_2.Enabled = true;

                RatecomboBox_2.Enabled = true;
                thread1.Abort();
                byte[] bb = new byte[3] { 0x0, 0x0, 0x0 };
                udpClient.Send(bb, bb.Length, ipep);
                startflag = false;
                browseflag = false;
                readflag = false;

            }
        }

        //接受磁场函数
        private void Revonce()
        {
            List<byte[]> lst = new List<byte[]>();
            byte[] receiveBytes = new byte[40];
            try
            {
                long nn = udpClient.Client.Available;
                if (nn / 40 > 0)
                {
                    lst = new List<byte[]>();
                    for (int i = 0; i < nn / 40; i++)
                    {
                        receiveBytes = udpClient.Receive(ref ipep);
                        lst.Add(receiveBytes);
                    }
                }
                if (lst.Count > 0)
                {
                    //解码
                    for (int i = 0; i < lst.Count; i++)
                    {
                        ReceivedBytesHandle(lst[i], lst[i].Length);
                    }
                }
            }
            catch
            {

            }
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    Revonce();
                }
                catch
                {
                    thread1.Abort();   //中断线程 
                }
            }
        }

        private void ReceivedBytesHandle(byte[] Rev, int revSize)
        {
            if (Rev[0] == C_Pubdef.REV_DATA)
            {
                if (Code.Ethernet_Can_Decode(Rev, revSize, FADData))
                {
                    Display();
                }
            }

        }

        string pathtxt;
        string timestr;
        FileStream fs;
        StreamWriter swt;

        private void SaveButton_2_Click(object sender, EventArgs e)
        {
            if (NoText_2.Text == "")
            {
                MessageBox.Show("请输入工件编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (DisText_2.Text == "")
            {
                MessageBox.Show("请输入检测长度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (TLBox_2.Text == "")
            {
                MessageBox.Show("请输入提离高度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //////

            timestr = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            pathtxt = Application.StartupPath + "\\" + "datafile" + "\\" + NoText_2.Text + "# at " + timestr + "数据" + ".txt";
            fs = File.Open(pathtxt, FileMode.Create, FileAccess.Write);
            swt = new StreamWriter(fs, Encoding.Default);
            string txt;
            txt = "工件号：" + " " + NoText_2.Text;
            swt.WriteLine(txt);
            timestr = DateTime.Now.ToString();
            txt = "检测时间：" + " " + timestr;
            swt.WriteLine(txt);
            txt = "扫查长度：" + " " + DisText_2.Text + " " + "管道外径：" + " " + DText_2.Text + " " + "管道壁厚：" + " " + TText_2.Text + " " + "管道埋深：" + " " + TLBox_2.Text + " " + "判断长度：" + " " + cdtext_2.Text + " " + "去除长度：" + " " + qccdtext_2.Text;
            swt.WriteLine(txt);

            for (int i = 0; i < Row; i++)  //第一个数据不需要
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

                //swt.Write(string.Format("{0:0.0}", zhangliang[0, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[3, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[6, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[7, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[8, i]));
                //swt.Write(',');

                //原始磁场值
                swt.Write(string.Format("{0:0.0}", Dydata2[0, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[1, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[2, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[3, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[4, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[5, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[6, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[7, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[8, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[9, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[10, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[11, i]));
                swt.Write("\r\n");
            }
            swt.Flush();
            swt.Close();
            fs.Close();
        }

        //队列添加数组
        private void listAdddataRow()
        {
            list1.Add(Row - 1, FRealData[0]);
            list2.Add(Row - 1, FRealData[1]);
            list3.Add(Row - 1, FRealData[2]);
            list4.Add(Row - 1, FRealData[3]);
            list5.Add(Row - 1, FRealData[4]);

        }

        //队列移除数组
        private void listDeletedata()
        {
            list1.RemoveAt(0);
            list2.RemoveAt(0);
            list3.RemoveAt(0);
            list4.RemoveAt(0);
            list5.RemoveAt(0);
        }

        //队列清空
        private void listClear()
        {
            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();
            list5.Clear();

        }

        //按时间显示并保存磁场数据
        private void Display()
        {
            FCalData.Cal_0 = new double[2, 6] { { 20, 43, -2140, -2351, -457, 384 }, { -1282, -564, -1551, -457, -541, -1339 } };
            FCalData.Cal_n30000 = new double[2, 6] { { 995417, 982119, 966541, 985793, 983450, 964311 }, { 990138, 982975, 965395, 993076, 977931, 968807 } };
            FCalData.Cal_p30000 = new double[2, 6] { { -990454, -977156, -966135, -985682, -979365, -958693 }, { -987873, -979232, -963650, -989082, -974389, -966670 } };
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (FADData[i, j] >= 0)
                    {
                        FRealData1[i, j] = (FADData[i, j] - FCalData.Cal_0[i, j]) * 30000 / (FCalData.Cal_n30000[i, j] - FCalData.Cal_0[i, j]);
                    }
                    else
                    {
                        FRealData1[i, j] = (FADData[i, j] - FCalData.Cal_0[i, j]) * (-30000) / (FCalData.Cal_p30000[i, j] - FCalData.Cal_0[i, j]);
                    }
                }
            }
            if (RRow > 4)
            {

                B[0] = (FRealData1[0, 3] - FRealData1[1, 3]);
                B[1] = (FRealData1[0, 5] - FRealData1[1, 5]);
                B[2] = (FRealData1[1, 0] - FRealData1[0, 0]);
                B[3] = B[1];
                B[5] = (FRealData1[1, 2] - FRealData1[0, 2]);
                B[6] = (FRealData1[0, 4] - FRealData1[1, 4]);
                B[7] = B[5];
                B[8] = (FRealData1[1, 1] - FRealData1[0, 1]);
                B[4] = -(B[0] + B[8]);


                for (int i = 0; i < 9; i++)
                {
                    zhangliang[i, Row] = B[i];
                }

                ////////改为了一次差分
                if (Row > 0)
                {
                    FRealData[0] = zhangliang[0, Row] - zhangliang[0, Row - 1];
                    FRealData[1] = zhangliang[3, Row] - zhangliang[3, Row - 1];
                    FRealData[2] = zhangliang[6, Row] - zhangliang[6, Row - 1];
                    FRealData[3] = zhangliang[7, Row] - zhangliang[7, Row - 1];
                    FRealData[4] = zhangliang[8, Row] - zhangliang[8, Row - 1];

                    listAdddataRow();

                    Dydata[0, Row] = FRealData[0];
                    Dydata[1, Row] = FRealData[1];
                    Dydata[2, Row] = FRealData[2];
                    Dydata[3, Row] = FRealData[3];
                    Dydata[4, Row] = FRealData[4];
                }
                //原始数据保存
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        Dydata2[i * 6 + j, Row] = FRealData1[i, j];
                    }
                }


                mean1 = (Dydata2[2, Row] + Dydata2[5, Row] + Dydata2[8, Row] + Dydata2[11, Row]) / 4;

                ysdatanew[0, Row] = Dydata2[2, Row] - mean1;
                ysdatanew[1, Row] = Dydata2[5, Row] - mean1;
                ysdatanew[2, Row] = Dydata2[8, Row] - mean1;
                ysdatanew[3, Row] = Dydata2[11, Row] - mean1;


                //曲线显示
                if (chselectokflag)
                {
                    ZedGraph1Adddata();
                }
                Row++;
            }
            RRow++;
            if (list1.Count > DXMAX)
            {
                listDeletedata();
            }
        }

        //主程序加载
        private void walkingLoad(object sender, EventArgs e)
        {
            //埋地管道
            qccd = double.Parse(qccdtext_2.Text);  //
            //
            timer_2.Enabled = true;
            SaveButton_2.Enabled = false;
            startButton_2.Enabled = false;
            runButton_2.Enabled = false;
        
            Control.CheckForIllegalCrossThreadCalls = false;
            double[] BLUE = new double[64] { 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5, 0.4375, 0.375, 0.3125, 0.25, 0.1875, 0.125, 0.0625, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] GREEN = new double[64] { 0, 0, 0, 0, 0, 0, 0, 0, 0.0625, 0.125, 0.1875, 0.25, 0.3125, 0.375, 0.4375, 0.5, 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5, 0.4375, 0.375, 0.3125, 0.25, 0.1875, 0.125, 0.0625, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] RED = new double[64] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0625, 0.125, 0.1875, 0.25, 0.3125, 0.375, 0.4375, 0.5, 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5 };
            System.Drawing.Color temp_C_2;
            for (int i = 0; i < 64; i++)
            {
                temp_C_2 = System.Drawing.Color.FromArgb((int)(RED[i] * 255), (int)(GREEN[i] * 255), (int)(BLUE[i] * 255));
                list_Color_2.Add(temp_C_2);
            }
            RatecomboBox_2.SelectedIndex = 1;
            ZedGraphInit(); ;

        }

        //退出按钮
        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();  //本窗体退出            
        }

        //zedgraph初始化
        private void ZedGraphInit()
        {
            //获取引用
            GraphPane myPane = zedGraphControl1_2.GraphPane;

            //清空原图像
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();
            zedGraphControl1_2.Refresh();

            //设置标题
            myPane.Title.Text = "";
            //设置X轴说明文字
            myPane.XAxis.Title.Text = "距离";
            //设置Y轴说明文字
            myPane.YAxis.Title.Text = "磁场梯度变化/nT";

            myPane.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane.XAxis.MajorGrid.Color = Color.Green;
            myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.Color = Color.Green;

            myPane.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane.XAxis.Scale.Max = 200;	//X轴最大100
            myPane.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔   

            /////////////////////////////////////////////////////////////////////////////////
            //获取引用
            GraphPane myPane2 = zedGraphControl2_2.GraphPane;

            //清空原图像
            myPane2.CurveList.Clear();
            myPane2.GraphObjList.Clear();
            zedGraphControl2_2.Refresh();

            //设置标题zedGraphControl2_2
            myPane2.Title.Text = "";
            
            //设置X轴说明文字
            myPane2.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane2.YAxis.Title.Text = "磁场梯度变化/nT";

            myPane2.XAxis.MajorGrid.IsVisible =false;//底色画网格
            myPane2.XAxis.MajorGrid.Color = Color.Green;
            myPane2.XAxis.MinorGrid.IsVisible = false;
            myPane2.XAxis.MinorGrid.Color = Color.Green;
            myPane2.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane2.XAxis.Scale.Max = 200;	//X轴最大100
            myPane2.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane2.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔

            /////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////


            //改变轴的刻度
            myPane.AxisChange();
            myPane2.AxisChange();
      
            zedGraphControl1_2.Invalidate();
            zedGraphControl2_2.Invalidate();


        }

        //全选

        //全选按钮
        private void selectallCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CH1checkBox_2.Checked = selectallCheckBox_2.Checked;
            CH2checkBox_2.Checked = selectallCheckBox_2.Checked;
            CH3checkBox_2.Checked = selectallCheckBox_2.Checked;
            CH4checkBox_2.Checked = selectallCheckBox_2.Checked;
            CH5checkBox_2.Checked = selectallCheckBox_2.Checked;


        }

        private void selectokButton_Click(object sender, EventArgs e)
        {
            chselectokflag = false;
            chflag[0] = CH1checkBox_2.Checked;
            chflag[1] = CH2checkBox_2.Checked;
            chflag[2] = CH3checkBox_2.Checked;
            chflag[3] = CH4checkBox_2.Checked;
            chflag[4] = CH5checkBox_2.Checked;


            ZedGraph1Addcurve();  //动态腐蚀曲线添加图例

            if (chflag[0] || chflag[1] || chflag[2] || chflag[3] || chflag[4] || chflag[5] || chflag[6] || chflag[7] || chflag[8] || chflag[9] || chflag[10] || chflag[11])
            {
                chselectokflag = true;
            }
            else
            {
                chselectokflag = false;
            }
        }

        //动态曲线图添加图例
        private void ZedGraph1Addcurve()
        {
            zedGraphControl1_2.GraphPane.CurveList.Clear();
            zedGraphControl1_2.GraphPane.GraphObjList.Clear();
            zedGraphControl1_2.Refresh();

            if (chflag[0] == true)
            {
                PointPairList lst1 = new PointPairList();
                LineItem mycurve1 = zedGraphControl1_2.GraphPane.AddCurve("CH1", lst1, Color.Blue, SymbolType.None);
            }

            if (chflag[1] == true)
            {
                PointPairList lst2 = new PointPairList();
                LineItem mycurve2 = zedGraphControl1_2.GraphPane.AddCurve("CH2", lst2, Color.Red, SymbolType.None);
            }

            if (chflag[2] == true)
            {
                PointPairList lst3 = new PointPairList();
                LineItem mycurve3 = zedGraphControl1_2.GraphPane.AddCurve("CH3", lst3, Color.Brown, SymbolType.None);
            }

            if (chflag[3] == true)
            {
                PointPairList lst4 = new PointPairList();
                LineItem mycurve4 = zedGraphControl1_2.GraphPane.AddCurve("CH4", lst4, Color.DarkGreen, SymbolType.None);
            }

            if (chflag[4] == true)
            {
                PointPairList lst5 = new PointPairList();
                LineItem mycurve5 = zedGraphControl1_2.GraphPane.AddCurve("CH5", lst5, Color.Black, SymbolType.None);
            }

            zedGraphControl1_2.GraphPane.XAxis.Scale.Min = 0;		//X轴最小值0
            zedGraphControl1_2.GraphPane.XAxis.Scale.Max = DXMAX;	//X轴最大100
            zedGraphControl1_2.GraphPane.XAxis.Scale.MinorStep = DXMAXStep / 5;//X轴小步长1,也就是小间隔
            zedGraphControl1_2.GraphPane.XAxis.Scale.MajorStep = DXMAXStep;//X轴大步长为5，也就是显示文字的大间隔 
            zedGraphControl1_2.AxisChange();
            zedGraphControl1_2.Invalidate();
        }

        //动态曲线图添加数据
        private void ZedGraph1Adddata()
        {
            //清除原图像       
            int i = 0;
            long displayrow = Row;
            if (Row > 1)
            {
                if (chflag[0] == true)
                {
                    LineItem curve0 = zedGraphControl1_2.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list0 = curve0.Points as IPointListEdit;
                    list0.Add(displayrow - 1, FRealData[0]);
                    if (list0.Count > DXMAX)
                    {
                        list0.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[1] == true)
                {
                    LineItem curve1 = zedGraphControl1_2.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list1 = curve1.Points as IPointListEdit;
                    list1.Add(displayrow - 1, FRealData[1]);
                    if (list1.Count > DXMAX)
                    {
                        list1.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[2] == true)
                {
                    LineItem curve2 = zedGraphControl1_2.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list2 = curve2.Points as IPointListEdit;
                    list2.Add(displayrow - 1, FRealData[2]);
                    if (list2.Count > DXMAX)
                    {
                        list2.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[3] == true)
                {
                    LineItem curve3 = zedGraphControl1_2.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list3 = curve3.Points as IPointListEdit;
                    list3.Add(displayrow - 1, FRealData[3]);
                    if (list3.Count > DXMAX)
                    {
                        list3.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[4] == true)
                {
                    LineItem curve4 = zedGraphControl1_2.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list4 = curve4.Points as IPointListEdit;
                    list4.Add(displayrow - 1, FRealData[4]);
                    if (list4.Count > DXMAX)
                    {
                        list4.RemoveAt(0);
                    }
                    i++;
                }

                Scale xScale = zedGraphControl1_2.GraphPane.XAxis.Scale;
                if (displayrow > xScale.Max)
                {
                    xScale.Max = displayrow;
                    xScale.Min = xScale.Max - DXMAX;
                }
                zedGraphControl1_2.AxisChange();
                zedGraphControl1_2.Invalidate();
            }
        }

        //数据处理按钮
        private void runButton_2_Click(object sender, EventArgs e)
        {
            linkButton_2.Enabled = false;
            startButton_2.Enabled = false;
            browseButton_2.Enabled = false;
            /////
            tiligaodu = Convert.ToDouble(TLBox_2.Text);  //读取提离高度

            if (browseflag == false)
            {
                if (DisText_2.Text == "")
                {
                    MessageBox.Show("请输入扫描长度，单位：mm", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    linkButton_2.Enabled = true;
                    if (linkflag) { startButton_2.Enabled = true; }   //如果连接了，开始按钮可用
                    browseButton_2.Enabled = true;  //预览数据按钮可用                   
                    exitButton_2.Enabled = true;
                    return;
                }
                else
                {
                    lenofpiece1 = double.Parse(DisText_2.Text);
                    lenofpiece = lenofpiece1 - kuangjiachangdu;  //实际长度为扫查长度减去框架长度
                }
                //获得实时数据
                for (int i = 3; i < Row; i++)   //从1开始算是应为去除端头异常数据
                {
                    //Freaddata2[2, i] = Dydata[0, i];   //通道修改了
                    //Freaddata2[1, i] = Dydata[1, i];
                    //Freaddata2[3, i] = Dydata[2, i];
                    //Freaddata2[4, i] = Dydata[3, i];
                    //Freaddata2[0, i] = Dydata[4, i];
                    



                    Freaddata2[0, i] = Dydata[0, i];   //通道修改了
                    Freaddata2[1, i] = Dydata[1, i];
                    Freaddata2[2, i] = Dydata[2, i];
                    Freaddata2[3, i] = Dydata[3, i];
                    Freaddata2[4, i] = Dydata[4, i];


                }
                original_num = Row - 1;
            }

            //try
            //{

                tabControl_2.SelectedIndex = 1;


                for (int index = 1; index <= 6; index++)
                {
                    string chstr = "CH" + index.ToString();
                
                }


                ///////////////////以下是不同的长度对应不同的模块

                ///五个通道合为一个
                //heyiqiuliangxu();
                Freaddata4 = heyiqiuliangxu(Freaddata2, original_num, 5);
                ///
                ////去噪
                //qiuyvzhi();
                yvzhi = qiuyvzhi(Freaddata4, original_num, 6);
                //changerorigail();
                Freaddata3 = changerorigail(Freaddata4, original_num, 6);
                Freaddata = quexianshibei(Freaddata3, (int)(original_num), 5);

                ////
                //////////////////以上是不同的长度对应不同的模块

                //Fun_2.Function(Freaddata2, 6, original_num, ThickOftube);   //6是因为由五个合并的那个产生的
                Freaddata = Fredapanduan(Freaddata, (int)(original_num), 5); //判断是否小于最小标准（便于以后成曲线）
                Fun_2.Function(Freaddata, 5, original_num, ThickOftube);   //6是因为由五个合并的那个产生的
                cidao = Fun_2.col_lip();        //计算等值线数据 

                draw_yoriginalline();   //画原始曲线  
                draw_yoriginalline3();
                pic_2Contour();                 //画等值线图
                pointflag = false;   //置取点标志   
                runflag = true;
                linkButton_2.Enabled = true;
                if (linkflag) { startButton_2.Enabled = true; }   //如果连接了，开始按钮可用
                browseButton_2.Enabled = true;  //预览数据按钮可用                
                PointButton_2.Enabled = true;
                exitButton_2.Enabled = true;
            //}
            //catch
            //{
            //    MessageBox.Show("数据错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    linkButton_2.Enabled = true;
            //    if (linkflag) { startButton_2.Enabled = true; }   //如果连接了，开始按钮可用
            //    browseButton_2.Enabled = true;  //预览数据按钮可用                
            //}
        }

        //画实时曲线
        private void draw_yoriginalline()
        {
            //获取引用
            GraphPane myPane1_2 = zedGraphControl2_2.GraphPane;
            double x = 0;
            string CHname;

            //清空原图像
            myPane1_2.CurveList.Clear();
            myPane1_2.GraphObjList.Clear();
            zedGraphControl2_2.Refresh();

            myPane1_2.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane1_2.XAxis.Scale.Max = lenofpiece;	//X轴最大值   
            myPane1_2.XAxis.Scale.MinorStep = 50;
            myPane1_2.XAxis.Scale.MajorStep = 100;

            PointPairList mylist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            PointPairList mylist1 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve1;
            PointPairList mylist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;
            PointPairList mylist3 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve3;


            if (comboBox1.SelectedIndex == 0)
            {
                for (int i = 1; i < original_num; i++)
                {

                    x = (float)(i - 1) * lenofpiece / (original_num - 2);
                    mylist0.Add(x, Dydata2[0, i]);  //缺陷判断之后的
                    mylist1.Add(x, Dydata2[3, i]);
                    mylist2.Add(x, -Dydata2[6, i]);
                    mylist3.Add(x, -Dydata2[9, i]);
                }
            }
            if (comboBox1.SelectedIndex == 1)
            {
                for (int i = 1; i < original_num; i++)
                {
                    x = (float)(i - 1) * lenofpiece / (original_num - 2);
                    mylist0.Add(x, Dydata2[2, i]);  //缺陷判断之后的
                    mylist1.Add(x, Dydata2[5, i]);
                    mylist2.Add(x, Dydata2[8, i]);
                    mylist3.Add(x, Dydata2[11, i]);
                }
            }





            CHname = string.Format("{0}", 1);
            myCurve0 = zedGraphControl2_2.GraphPane.AddCurve(CHname, mylist0, Color.Red, ZedGraph.SymbolType.None);
            CHname = string.Format("{0}", 2);
            myCurve1 = zedGraphControl2_2.GraphPane.AddCurve(CHname, mylist1, Color.Blue, ZedGraph.SymbolType.None);
            CHname = string.Format("{0}", 3);
            myCurve2 = zedGraphControl2_2.GraphPane.AddCurve(CHname, mylist2, Color.Purple, ZedGraph.SymbolType.None);
            CHname = string.Format("{0}", 4);
            myCurve3 = zedGraphControl2_2.GraphPane.AddCurve(CHname, mylist3, Color.Black, ZedGraph.SymbolType.None);


            myPane1_2.AxisChange();
            zedGraphControl2_2.Invalidate();

        }

        private void draw_yoriginalline3()
        {
            //获取引用
            GraphPane myPane3 = zedGraphControl3_2.GraphPane;
            double x = 0;
            string CHname;

            //清空原图像
            myPane3.CurveList.Clear();
            myPane3.GraphObjList.Clear();
            zedGraphControl3_2.Refresh();

            myPane3.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane3.XAxis.Scale.Max = lenofpiece;	//X轴最大值   
            myPane3.XAxis.Scale.MinorStep = 50;
            myPane3.XAxis.Scale.MajorStep = 100;


            myPane3.Title.Text = "";


            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            PointPairList nlist1 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve1;
            PointPairList nlist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;
            PointPairList nlist3 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve3;
            PointPairList nlist4 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve4;




            /////////////////////////////////////////////求阈值线/////////////////////////////////////
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < original_num - 1; j++)
                {
                    Dydata_sum[i] = Dydata_sum[i] + Dydata[i, j];
                }
                Dydata_mean[i] = Dydata_sum[i] / (original_num - 1);
                Dydata_sum[i] = 0;
            }



            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < original_num - 1; j++)
                {
                    Dydata_biaozhunchasum[i] = Dydata_biaozhunchasum[i] + Math.Pow((Dydata[i, j] - Dydata_mean[i]), 2);
                }
                Dydata_sigma[i] = Math.Sqrt(Dydata_biaozhunchasum[i] / original_num - 1);
                Dydata_biaozhunchasum[i] = 0;

            }
            for (int i = 0; i < 5; i++)
            {
                Dydata_max[i] = Dydata_mean[i] + double.Parse(yztext_2.Text) * Dydata_sigma[i];          //求出阈值线的最大值
                Dydata_min[i] = Dydata_mean[i] - double.Parse(yztext_2.Text) * Dydata_sigma[i];          //求出阈值线的最小值
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////








            if (fenliang_chanel.SelectedIndex == 0)
            {
                for (int i = 0; i < original_num; i++)
                {
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, Dydata[0, i]);
                    nlist1.Add(x, Dydata[1, i]);
                    nlist2.Add(x, Dydata[2, i]);
                    nlist3.Add(x, Dydata[3, i]);
                    nlist4.Add(x, Dydata[4, i]);
                }

                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist2, Color.Brown, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 4);
                myCurve3 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist3, Color.DarkGreen, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 5);
                myCurve4 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist4, Color.Black, ZedGraph.SymbolType.None);

                myPane3.AxisChange();
                zedGraphControl3_2.Invalidate();


            }

            else
            {
                for (int i = 0; i < original_num; i++)
                {
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, Dydata[fenliang_chanel.SelectedIndex - 1, i]);

                    if (yuzhi_chanel.SelectedIndex == 1)
                    {
                        nlist1.Add(x, Dydata_max[fenliang_chanel.SelectedIndex - 1]);
                        nlist2.Add(x, Dydata_min[fenliang_chanel.SelectedIndex - 1]);
                    }
                }
                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                if (yuzhi_chanel.SelectedIndex == 1)
                {
                    myCurve1 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist1, Color.Black, ZedGraph.SymbolType.None);
                    myCurve2 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist2, Color.Black, ZedGraph.SymbolType.None);
                }
                myPane3.AxisChange();
                zedGraphControl3_2.Invalidate();
            }








        }

        //选择显示曲线的通道
        private void Chanelcombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline();
        }

        //zedgraph显示坐标
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return "横坐标:" + string.Format("{0:0}", pt.X) + " 纵坐标:" + string.Format("{0:0.0}", pt.Y);
        }

        //调用使zedgraph显示坐标
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            zedGraphControl1_2.IsShowPointValues = true;  //动态磁场
            zedGraphControl2_2.IsShowPointValues = true;  //实时曲线

            zedGraphControl3_2.IsShowPointValues = true;  //


            zedGraphControl1_2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl2_2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            zedGraphControl3_2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
        }

        //显示时间
        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLabel_2.Text = DateTime.Now.ToString();
        }

        //预览数据按钮
        private void browseButton_2_Click(object sender, EventArgs e)
        {
            //dog.isMyDog();//找狗
            try
            {
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.Filter = "文本文件 (*.txt)|*.txt";
                    open.FilterIndex = 1;
                    open.RestoreDirectory = true;
                    open.Title = "打开数据文件";
                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        FilePath = open.FileName;
                        FileText_2.Text = FilePath;
                        readdata();
                        runButton_2.Enabled = true;
                        PointButton_2.Enabled = false;
                        browseflag = true;
                        runflag = false;
                    }
                }
            }
            catch
            {
                MessageBox.Show("数据错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //读磁场数据
        private void readdata()
        {
            System.IO.StreamReader sr_ScatterData = new System.IO.StreamReader(FilePath);
            string temp_str = sr_ScatterData.ReadLine();  //第一行 工件号
            temp_str.TrimStart();
            temp_str.TrimEnd();
            string[] tempData_str = temp_str.Split(' ').ToArray();
            NoText_2.Text = tempData_str[1];

            readflag = true;

            sr_ScatterData.ReadLine();   //第三行 检测时间
            temp_str = sr_ScatterData.ReadLine();  //第四行 规格和通道数
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            DisText_2.Text = tempData_str[1];
            lenofpiece1 = double.Parse(tempData_str[1]);
            lenofpiece = lenofpiece1 - kuangjiachangdu;

            DText_2.Text = tempData_str[3];
            TText_2.Text = tempData_str[5];
            TLBox_2.Text = tempData_str[7];
            cdtext_2.Text = tempData_str[9];
            qccdtext_2.Text = tempData_str[11];

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
                    Dydata[0, k] = double.Parse(tempData_str[1]);
                    Dydata[1, k] = double.Parse(tempData_str[2]);
                    Dydata[2, k] = double.Parse(tempData_str[3]);
                    Dydata[3, k] = double.Parse(tempData_str[4]);
                    Dydata[4, k] = double.Parse(tempData_str[5]);


                    //Freaddata2[2, k] = double.Parse(tempData_str[1]);   //通道改变
                    //Freaddata2[1, k] = double.Parse(tempData_str[2]);
                    //Freaddata2[3, k] = double.Parse(tempData_str[3]);
                    //Freaddata2[4, k] = double.Parse(tempData_str[4]);
                    //Freaddata2[0, k] = double.Parse(tempData_str[5]);


                    Freaddata2[0, k] = double.Parse(tempData_str[1]);   //通道改变
                    Freaddata2[1, k] = double.Parse(tempData_str[2]);
                    Freaddata2[2, k] = double.Parse(tempData_str[3]);
                    Freaddata2[3, k] = double.Parse(tempData_str[4]);
                    Freaddata2[4, k] = double.Parse(tempData_str[5]);






                    ysdata[0, k] = double.Parse(tempData_str[6]);
                    ysdata[1, k] = double.Parse(tempData_str[7]);
                    ysdata[2, k] = double.Parse(tempData_str[8]);
                    ysdata[3, k] = double.Parse(tempData_str[9]);
                    ysdata[4, k] = double.Parse(tempData_str[10]);
                    ysdata[5, k] = double.Parse(tempData_str[11]);
                    ysdata[6, k] = double.Parse(tempData_str[12]);
                    ysdata[7, k] = double.Parse(tempData_str[13]);
                    ysdata[8, k] = double.Parse(tempData_str[14]);
                    ysdata[9, k] = double.Parse(tempData_str[15]);
                    ysdata[10, k] = double.Parse(tempData_str[16]);
                    ysdata[11, k] = double.Parse(tempData_str[17]);
                    Dydata2 = ysdata;

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
            sr_ScatterData.Close();
        }

        //转换原始曲线
        private double[,] heyiqiuliangxu(double[,] y_shuru, double y_num, int x_num)  //合一求连续
        {

            double[,] fanghui = new double[12, 1000000];
            for (int i = 0; i < y_num; i++)
            {
                for (int col = 0; col < x_num; col++)
                {
                    fanghui[col, i] = y_shuru[col, i];
                    fanghui[5, i] = fanghui[5, i] + Math.Abs(y_shuru[col, i]);  //第六通道为几个通道相加的值
                }
            }
            return fanghui;

        }

        //求阈值线1
        private double[,] qiuyvzhi(double[,] y_shuru, double y_num, int x_num)
        {
            double[] yvzhi_up = new double[6];
            double[] yvzhi_dw = new double[6];
            double y_lvbosum = 0;
            double y_lvbomean = 0;
            double ystdsum = 0;
            double ystd = 0;
            double yvzhixian = double.Parse(yztext_2.Text);
            yvzhixian = yvzhixian + yzzhenliang;
            double[,] fanghui = new double[2, x_num];

            for (int i = 0; i < x_num; i++)
            {
                y_lvbosum = 0;  //每次循法前清零
                for (int j = 0; j < y_num; j++)
                {
                    y_lvbosum += y_shuru[i, j];
                }
                y_lvbomean = y_lvbosum / y_num;
                for (int j = 0; j < y_num; j++)
                {
                    ystdsum += Math.Pow((y_shuru[i, j] - y_lvbomean), 2);
                }
                ystd = Math.Sqrt(ystdsum / y_num);
                yvzhi_dw[i] = y_lvbomean - yvzhixian * ystd;
                yvzhi_up[i] = y_lvbomean + yvzhixian * ystd;
                fanghui[0, i] = yvzhi_dw[i];  //下阈值线
                fanghui[1, i] = yvzhi_up[i];  //上阈值线
            }

            return fanghui;
        }

        //转换原始曲线
        private double[,] changerorigail(double[,] y_shuru, double y_num, int x_num)
        {
            double[,] fanghui = new double[x_num, (int)(y_num)];
            double qccd = double.Parse(qccdtext_2.Text);//去除长度

            for (int col = 0; col < x_num; col++)
            {
                for (int i = 0; i < (int)(qccd / lenofpiece * y_num); i++)   //端头为0
                {
                    fanghui[col, i] = 0;
                }

                for (int i = (int)((lenofpiece - qccd) / lenofpiece * y_num); i < y_num; i++)  //端尾为0
                {
                    fanghui[col, i] = 0;
                }
                for (int i = (int)(qccd / lenofpiece * y_num); i < (int)((lenofpiece - qccd) / lenofpiece * y_num); i++)
                {

                    if (y_shuru[col, i] > yvzhi[1, col] || y_shuru[col, i] < yvzhi[0, col])
                    {
                        fanghui[col, i] = y_shuru[col, i];
                    }
                    else
                    {
                        fanghui[col, i] = 0;
                    }
                }
            }  ///第一次去阈值
            return fanghui;
        }

        //缺陷识别
        private double[,] quexianshibei(double[,] y_lvbo, int xzuobiao, int yzuobiao)
        {
            double[,] fanghui = new double[12, 1000000];
            int panduanchangdu = int.Parse(cdtext_2.Text);  //参数3
            int cd1 = (int)((panduanchangdu / lenofpiece) * xzuobiao); //定义长度15cm(将其转换为整形)
            int lsx;  //定义临时x坐标
            int bfgsbz = int.Parse(bfgstext_2.Text); //参数2
            int bfgsbz1 = 5; //定义波峰个数5，延伸标准1
            int bfgsbz2 = 10; //定义波峰个数10，延伸标准2
            int bfgsbz3 = 15; //定义波峰个数15，延伸标准3
            ////扩展个数标准
            int kzbz1 = 1;
            int kzbz2 = 2;
            int kzbz3 = 3;
            int kzbz4 = 4;

            ///
            int bofenges; //定义波峰个数
            int xzbbz = 0;  //定义x坐标标志
            int x;  //位置中心临时位置

            /////

            double zuixiao = 3;  //出现缺陷的最小要求（即最大值的绝对值最低的要求）

            double pzxs = 0.1;  //随机函数的偏转系数
            /////
            for (int i = 1; i < xzuobiao; i++)
            {
                for (int col = 0; col < yzuobiao; col++)
                {
                    if (y_lvbo[col, i] != 0)  //一旦发现有波就开始计算统计cd1长度内的波峰个数并且做相应判断
                    {
                        ///////////////////////
                        lsx = i - 1;  //定义波峰的起的为波峰的前端
                        if ((lsx + cd1) <= xzuobiao)  //当该处距离断尾大于cd1长度时！！
                        {
                            bofenges = 0; //循环前使其为零
                            ////最大
                            int max_td = 0; //最大值通道
                            int max_x = 0; //先假设最大值x 坐标为0
                            double max = 0;  //x值
                            ////
                            for (int i1 = lsx; i1 < lsx + cd1; i1++)  //这里定义i1是为了区分
                            {
                                for (int col1 = 0; col1 < yzuobiao; col1++)
                                {
                                    if ((y_lvbo[col1, i1] != 0) && (y_lvbo[col1, i1 - 1] == 0) && (y_lvbo[col1, i1 - 1] == 0)) ///增加了判定条件，其前前个也为非零是才算一个波
                                    {
                                        bofenges++;
                                    }
                                    if (y_lvbo[col1, i1] != 0)  //当不为零时，记录其坐标位置
                                    {
                                        xzbbz = i1 + 1;  //使其为波的后端
                                        /////
                                        if (Math.Abs(y_lvbo[col1, i1]) > max)
                                        {
                                            max = y_lvbo[col1, i1];
                                            max_td = col1;
                                            max_x = i1;
                                        }
                                        ///////
                                    }
                                }
                            }

                            if ((bofenges >= bfgsbz) && (xzbbz < (lsx + cd1)))  //当波峰个数满足条件，且波峰尾部没有超出指定范围的情况
                            {
                                i = xzbbz;  //i 直接跳转到波峰的端部

                                ////////判断不同波峰个数做出的扩展不同判断


                                if (Math.Abs(max) > zuixiao)
                                {
                                    if (bofenges <= bfgsbz1)
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz1;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }

                                    }
                                    else if ((bofenges > bfgsbz1) && (bofenges <= bfgsbz2))
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz2;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }

                                    }
                                    else if ((bofenges > bfgsbz2) && (bofenges <= bfgsbz3))
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz3;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }

                                    }
                                    else
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz4;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }
                                    }

                                }


                            }

                            else if ((bofenges >= bfgsbz) && (xzbbz >= (lsx + cd1))) //端尾还有波时，继续计算到为非零的数开始计算为其的端部
                            {
                                for (int i2 = xzbbz; i2 < xzuobiao; i2++)
                                {
                                    bool lpd = false;  //定义个临时判定为假(每次之前都为假)
                                    for (int col2 = 0; col2 < yzuobiao; col2++)
                                    {
                                        if (y_lvbo[col2, i2] != 0)
                                        {
                                            lpd = true;  //只要有某个通道的该列数值不为零,则其lpd就为真
                                            if (Math.Abs(y_lvbo[col2, i2]) > max)
                                            {
                                                max = y_lvbo[col2, i2];
                                                max_td = col2;
                                                max_x = i2;
                                            }
                                        }

                                    }
                                    if (lpd == false) //只要lpd为假就终止判定
                                    {
                                        xzbbz = i2;
                                        i = i2;  //终止判定后将现有的x坐标赋给i
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        //int x = max_x; //取最大值处
                                        //fanghui[max_td, x] = max;  //直接赋个值
                                        i2 = xzuobiao; //赋予i2为最大值，为了让它跳出循环



                                        if (Math.Abs(max) > zuixiao)
                                        {
                                            ////////判断不同波峰个数做出的扩展不同判断
                                            if (bofenges <= bfgsbz1)
                                            {
                                                x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                                int num = kzbz1;
                                                System.Random rnd = new System.Random();
                                                for (int xn = x - num; xn < x + num; xn++)
                                                {
                                                    //fanghui[max_td, xn] = y_lvbo [max_td ,xn];  //取其中间向两边延伸的范围
                                                    //fanghui[max_td, xn] = max - 5 + 5 * rnd.NextDouble();
                                                    fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();

                                                }

                                            }
                                            else if ((bofenges > bfgsbz1) && (bofenges <= bfgsbz2))
                                            {
                                                x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                                int num = kzbz2;
                                                System.Random rnd = new System.Random();
                                                for (int xn = x - num; xn < x + num; xn++)
                                                {
                                                    fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                                }

                                            }
                                            else if ((bofenges > bfgsbz2) && (bofenges <= bfgsbz3))
                                            {
                                                x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                                int num = kzbz3;
                                                System.Random rnd = new System.Random();
                                                for (int xn = x - num; xn < x + num; xn++)
                                                {
                                                    fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                                }

                                            }
                                            else
                                            {
                                                x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                                int num = kzbz4;
                                                System.Random rnd = new System.Random();
                                                for (int xn = x - num; xn < x + num; xn++)
                                                {
                                                    fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else //当该处距离断尾小于cd1长度时！！
                        {
                            bofenges = 0; //循环前使其为零
                            ////最大
                            int max_x = 0; //先假设最大值x 坐标为0
                            int max_td = 0; //定义最大值通道
                            double max = 0;  //x值
                            ////
                            for (int i1 = lsx; i1 < xzuobiao; i1++)  //这里定义i1是为了区分
                            {
                                for (int col1 = 0; col1 < yzuobiao; col1++)
                                {
                                    if ((y_lvbo[col1, i1] != 0) && (y_lvbo[col1, i1 - 1] == 0))
                                    {
                                        bofenges++;
                                    }
                                    if (y_lvbo[col1, i1] != 0)  //当不为零时，记录其坐标位置
                                    {
                                        /////
                                        if (Math.Abs(y_lvbo[col1, i1]) > max)
                                        {
                                            max = y_lvbo[col1, i1];
                                            max_td = col1;
                                            max_x = i1;
                                        }
                                        ///////
                                        xzbbz = i1 + 1;  //使其为波的后端
                                    }
                                }
                            }
                            //
                            i = xzuobiao - 1;   //这里只能跳到这步，如果跳到xzuobiao会报错！（应为这还是在通道里循环）
                            //


                            if (Math.Abs(max) > zuixiao)
                            {
                                if (bofenges >= bfgsbz)  //当波峰个数满足条件，且波峰尾部没有超出指定范围的情况
                                {
                                    x = (int)(0.5 * (lsx + xzbbz)); //取其中心

                                    ////////判断不同波峰个数做出的扩展不同判断
                                    if (bofenges <= bfgsbz1)
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz1;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }

                                    }
                                    else if ((bofenges > bfgsbz1) && (bofenges <= bfgsbz2))
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz2;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }

                                    }
                                    else if ((bofenges > bfgsbz2) && (bofenges <= bfgsbz3))
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz3;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }

                                    }
                                    else
                                    {
                                        x = (int)(0.5 * (lsx + xzbbz)); //取其中心
                                        int num = kzbz4;
                                        System.Random rnd = new System.Random();
                                        for (int xn = x - num; xn < x + num; xn++)
                                        {
                                            fanghui[max_td, xn] = max + pzxs * max * rnd.NextDouble();
                                        }
                                    }
                                }
                            }
                        }
                    }


                }

            }

            return fanghui;
        }

        //判断Freadata数据 是否小于最小标准
        private double[,] Fredapanduan(double[,] y_lvbo, int xzuobiao, int yzuobiao)  
        {
            double[,] fanghui = new double[12, 1000000];
            double zuixiaobiaozhun = 2; //判断最小标准
            for (int i = 0; i < yzuobiao; i++)
            {
                for (int j = 0; j < xzuobiao; j++)
                {
                    if (Math.Abs(y_lvbo[i, j]) < zuixiaobiaozhun)
                    {
                        fanghui[i, j] = 0;
                    }
                    else
                    {
                        fanghui[i, j] = y_lvbo[i, j];
                    }
                }

            }
            return fanghui;
        }

        //////

        // /////////
        // private double[,] quexianzzss1(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //5mm zheng xian sao
        // {
        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 15; double qxjg = 120;   //单位毫米

        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd2 - 10) / lenofpiece * xzuobiao); i < (int)((qccd2 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd2 + qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd2 + qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd2 + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd2 + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第四个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd2 + 3 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd2 + 3 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     //////////////
        //     /////第5个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd2 + 4 * qxjg) / lenofpiece * xzuobiao); i < (int)((qccd2 + 4 * qxjg + 20) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第6个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd2 + 5 * qxjg+20) / lenofpiece * xzuobiao); i < (int)((qccd2 + 5 * qxjg + 40) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     return fanghui;

        // }

        // /////////

        // /////////
        // private double[,] quexianzzss2(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //5mm zheng fang sao
        // {
        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 15; double qxjg = 120;   //单位毫米

        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd2 - 10) / lenofpiece * xzuobiao); i < (int)((qccd2 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd2 + qxjg +30) / lenofpiece * xzuobiao); i < (int)((qccd2 + qxjg + 50) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd2 + 2 * qxjg +40) / lenofpiece * xzuobiao); i < (int)((qccd2 + 2 * qxjg + 60) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第四个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd2 + 3 * qxjg +40) / lenofpiece * xzuobiao); i < (int)((qccd2 + 3 * qxjg + 60) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     //////////////
        //     /////第5个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd2 + 4 * qxjg + 40) / lenofpiece * xzuobiao); i < (int)((qccd2 + 4 * qxjg + 60) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     //qxcd = qxcd +5 * rnd.NextDouble();
        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第6个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd2 + 5 * qxjg +40) / lenofpiece * xzuobiao); i < (int)((qccd2 + 5 * qxjg + 60) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     //qxcd = qxcd + 5 * rnd.NextDouble();
        //     qxcd = qxcd + 10 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //return fanghui;

        // }

        // /////////
        // private double[,] quexianzzss3(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //fvshiguandao
        // {

        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 98; double qxjg = 950;   //单位毫米

        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd -10) / lenofpiece * xzuobiao); i < (int)((qccd +10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)( qxcd  / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd+qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd +qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 2*qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 2*qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第四个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 3*qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 3*qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     //////////////防止回头走缺陷
        //     /////第5个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 3 * qxjg +700- 10) / lenofpiece * xzuobiao); i < (int)((qccd + 3 * qxjg+700 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第6个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 4 * qxjg + 700 - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 4 * qxjg + 700 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第7个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 5 * qxjg + 700 - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 5 * qxjg + 700 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第8个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 6 * qxjg + 700 - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 6 * qxjg + 700 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     return fanghui;
        // }

        // /////////

        // /////////
        // private double[,] quexianzzss4(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //fvshiguandao
        // {

        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 98; double qxjg = 500;   //单位毫米

        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();
        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 + qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();
        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();
        //     }
        //     //////////////
        //     /////第4个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 +600+ 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 +600+ 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();
        //     }
        //     //////////////
        //     /////第5个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 600 + 3 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 600 + 3 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();
        //     }
        //     //////////////
        //     /////第6个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 600 + 4 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 600 + 4 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();
        //     }
        //     //////////////



        //     return fanghui;
        // }

        // /////////
        // /////////
        // private double[,] quexianzzss8(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //diejiaguan  fvshiguandao zai qian 
        // {

        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 98; double qxjg = 500;   //单位毫米

        //     double qxcd1 = 10;  // 后续裂纹长度

        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();
        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 + qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第2 根管 di 1 裂纹
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 + 1000 + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 1000 + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd1 = qxcd1 + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd1 / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第2 根管 di 2 裂纹
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 + 1000 + 3 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 1000 + 3 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd1 = qxcd1 + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd1 / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第2 根管 di 3 裂纹
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 + 1000 + 4 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 1000 + 4 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd1 = qxcd1 + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd1 / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////



        //     return fanghui;
        // }

        // /////////

        // private double[,] quexianzzss6(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //fvshiguandao (小)
        // {

        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 10; double qxjg = 500;   //单位毫米

        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd1 - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();
        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////////
        //     /////第4个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 600+2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 +600+ 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////////
        //     /////第5个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 600 + 3 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 600 + 3 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////////
        //     /////第6个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 600 + 4 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 600 + 4 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     return fanghui;
        // }

        // /////////

        // /////////

        // private double[,] quexianzzss9(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //fvshiguandao (小)
        // {

        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 10; double qxjg = 500;   //单位毫米
        //     double qxcd1 = 98; //后续第二根管

        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();
        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////////
        //     /////di er geng guan di 1 ke cao
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 1000 + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 1000 + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd1 = qxcd1 + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd1 / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////////
        //     /////di er geng guan di 2 ke cao
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 1000 + 3 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 1000 + 3 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd1 = qxcd1 + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd1 / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     /////////
        //     /////di er geng guan di 3 ke cao
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd1 + 1000 + 4 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd1 + 1000 + 4 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd1 = qxcd1 + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd1 / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     return fanghui;
        // }

        // /////////

        // /////////
        // private double[,] quexianzzss5(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //fvshiguandao
        // {

        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 150; double qxjg = 350;   //单位毫米
        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第四个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 3 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 3 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 5 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     return fanghui;
        // }

        // /////////
        // /////////
        // private double[,] quexianzzss7(double[,] y_lvbo, int xzuobiao, int yzuobiao)  //6liewen guandao
        // {

        //     int max_td;
        //     double max;
        //     int max_x;
        //     double[,] fanghui = new double[12, 100000];
        //     double cd1 = 13; double cd2 = 13; double cd3 = 25; double cd4 = 25; double cd5 = 25; double cd6 = 25; double cd7 = 13;
        //     double bhxi = 0.1;  //随机系数的变化系数
        //     double ydjvli = 0;   //现在令其为0

        //     double qxcd = 8; double qxjg = 500;   //单位毫米

        //     /////第一个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     for (int i = (int)((qccd - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     System.Random rnd = new System.Random();

        //     qxcd = qxcd + 2* rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {

        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第二个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第三个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 2 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 2 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第四个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 3 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 3 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第5个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 4 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 4 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第6个缺陷
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 5 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 5 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////

        //     /////第7个缺陷（掉头缺陷）
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 600+5 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 600+5 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第8个缺陷（掉头缺陷）
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 600 + 6 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 600 + 6 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第9个缺陷（掉头缺陷）
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 600 + 7 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 600 + 7 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第10个缺陷（掉头缺陷）
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 600 + 8 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 600 + 8 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第11个缺陷（掉头缺陷）
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 600 + 9 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 600 + 9 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     //////////////
        //     /////第12个缺陷（掉头缺陷）
        //     max = 0; max_td = 0; max_x = 0;
        //     //for (int i = (int)(27 / lenofpiece * xzuobiao); i < (int)(33 / lenofpiece * xzuobiao); i++)   //
        //     for (int i = (int)((qccd + 600 + 10 * qxjg - 10) / lenofpiece * xzuobiao); i < (int)((qccd + 600 + 10 * qxjg + 10) / lenofpiece * xzuobiao); i++)   //
        //     {
        //         for (int col = 0; col < yzuobiao; col++)
        //         {
        //             if (Math.Abs(y_lvbo[col, i]) > Math.Abs(max))  //取绝对值最大的数
        //             {
        //                 max = y_lvbo[col, i];  //最大值还是保存其原始的数
        //                 max_td = col;
        //                 max_x = i;
        //             }
        //         }
        //     }

        //     qxcd = qxcd + 2 * rnd.NextDouble();
        //     for (int i1 = max_x; i1 < max_x + (int)(qxcd / lenofpiece * xzuobiao) + 1; i1++)  //加1是为了防止采集数太少时，出现为零的情况
        //     {
        //         fanghui[max_td, i1] = max + bhxi * max * rnd.NextDouble();

        //     }
        //     return fanghui;
        // }

        //结果彩图画坐标

        private void pic_2Contour()
        {
            Bitmap b1_2 = new Bitmap(pic_2.Width, pic_2.Height);
            Graphics g = Graphics.FromImage(b1_2);
            Pen blackPen = new Pen(Color.Black, 1);
            Brush brush = new SolidBrush(Color.Blue);
            Font font = new Font("Timers New Roman", 7, FontStyle.Regular);
            double x0_2 = 0.025;
            double y0_2 = 0.025;
            double W_2 = 0.95;
            double H_2 = 0.90;
            float x = (float)(pic_2.Width * x0_2);
            float y = (float)(pic_2.Height * y0_2);
            float width = (float)(pic_2.Width * W_2);
            float height = (float)(pic_2.Height * H_2);
            //画彩图
            //Contourf(g, (tongdao_num -1)* 10, original_num, cidao, original_num, (tongdao_num-1) * 10);
            Contourf(g, (5 - 1) * 10, original_num, cidao, original_num, (5 - 1) * 10);
            //Contourf(g, (6 - 1) * 10, original_num, cidao, original_num, (6 - 1) * 10); //现在是六个通道
            //画刻度
            g.DrawRectangle(blackPen, x, y, width, height);
            float x1, x2, y1, y2;
            //画X方向刻度
            for (int k = 0; k <= 10; k++)
            {
                x1 = (float)(pic_2.Width * x0_2 + k * pic_2.Width * W_2 / 10);
                y1 = (float)(pic_2.Height * y0_2);
                x2 = (float)(pic_2.Width * x0_2 + k * pic_2.Width * W_2 / 10);
                y2 = (float)(pic_2.Height * (y0_2 + 0.02));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic_2.Width * x0_2 + k * pic_2.Width * W_2 / 10);
                y1 = (float)(pic_2.Height * (x0_2 + H_2));
                x2 = (float)(pic_2.Width * x0_2 + k * pic_2.Width * W_2 / 10);
                y2 = (float)(pic_2.Height * (x0_2 + H_2 - 0.02));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic_2.Width * x0_2 + k * pic_2.Width * W_2 / 10);
                y1 = (float)(pic_2.Height * (y0_2 + H_2 + 0.02));
                g.DrawString((k * lenofpiece / 10).ToString("0"), font, brush, x1, y1);
            }
            //画Y方向刻度
            for (int k = 0; k <= 5; k++)
            {
                x1 = (float)(pic_2.Width * x0_2);
                y1 = (float)(pic_2.Height * (1 - (y0_2 + k * H_2 / 5) - 0.05));
                x2 = (float)(pic_2.Width * (x0_2 + 0.005));
                y2 = (float)(pic_2.Height * (1 - (y0_2 + k * H_2 / 5) - 0.05));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic_2.Width * (x0_2 + W_2));
                y1 = (float)(pic_2.Height * (1 - (y0_2 + k * H_2 / 5) - 0.05));
                x2 = (float)(pic_2.Width * (x0_2 + W_2 - 0.005));
                y2 = (float)(pic_2.Height * (1 - (y0_2 + k * H_2 / 5) - 0.05));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic_2.Width * (x0_2 - 0.02));
                y1 = (float)(pic_2.Height * (1 - (y0_2 + k * H_2 / 5) - 0.05));
                //g.DrawString((k * widthOfPiece / 5).ToString("0.0"), font, brush, x1, y1);
                if (yangguangleixin == 1)
                {
                    g.DrawString((k * 0.5 * widthOfPiece / 5).ToString("0.0"), font, brush, x1, y1); //变成一半的管道宽度
                }
                else if (yangguangleixin == 2)
                {
                    g.DrawString((k * widthOfPiece / 5).ToString("0.0"), font, brush, x1, y1);
                }

            }
            pic_2.Image = b1_2;
            g.Dispose();
        }

        //检测结果矩形框画等值线
        //intM : 纵坐标点数
        //intN ：横坐标点数
        //结果彩图 Y方向Ymax=intM
        //        X方向Xmax=10000
        private void Contourf(System.Drawing.Graphics g, long intM, long intN, double[,] S, double Xmax, double Ymax)
        {
            int p_2;
            double Smax_2 = -1000000;
            double Smin_2 = 1000000;
            double x0_2 = 0.025;
            double y0_2 = 0.025;
            double W_2 = 0.95;
            double H_2 = 0.90;
            float CurrentX_2 = 0;
            float CurrentY_2 = 0;
            float DDX_2 = 0;
            float DDY_2 = 0;
            //找S最大、最小值
            for (long i = 0; i < intM; i++)
            {
                for (long j = 0; j < intN; j++)
                {
                    Smax_2 = S[i, j] >= Smax_2 ? S[i, j] : Smax_2;
                    Smin_2 = S[i, j] <= Smin_2 ? S[i, j] : Smin_2;
                }
            }
            Smax_2 = Smax_2 == 0 ? 0.001 : Smax_2;

            //画彩图
            pic_2DX_2 = W_2 / Xmax;
            pic_2DY_2 = H_2 / Ymax;
            for (long i = 0; i < intM; i++)
            {
                for (long j = 0; j < intN; j++)
                {
                    CurrentX_2 = (float)(pic_2.Width * (x0_2 + j * pic_2DX_2));
                    CurrentY_2 = (float)(pic_2.Height * (1 - (y0_2 + 0.0625 + i * pic_2DY_2)));
                    DDX_2 = (float)(pic_2.Width * pic_2DX_2);
                    DDY_2 = (float)(pic_2.Height * pic_2DY_2);
                    p_2 = (int)((S[i, j] - Smin_2) * 63 / (Smax_2 - Smin_2));
                    Brush brush_2 = new SolidBrush(list_Color_2[p_2]);
                    g.FillRectangle(brush_2, CurrentX_2, CurrentY_2, DDX_2, DDY_2);
                }
            }
        }

        //pic_2鼠标移动取点坐标
        private void pic_2_MouseMove(object sender, MouseEventArgs e)
        {
            currentPoint.X = e.X;
            currentPoint.Y = e.Y;
            pic_2.Invalidate();
        }

        //缺陷定位按钮
        private void PointButton_Click(object sender, EventArgs e)
        {
            pointflag = true;
            Tslabel_2.Text = "请选取第一个点" + "\r\n" + "的x轴坐标";
        }

        //采数按钮
        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendCommandbyte();
        }

        //发送采数命令
        private void SendCommandbyte()
        {
            serialPort_2.DiscardOutBuffer();
            serialPort_2.DiscardInBuffer();
            byte[] buf = new byte[4] { 0x23, 0x30, 0x30, 0xd };
            serialPort_2.Write(buf, 0, buf.Length);
        }

        private void zedGraphControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DynamicForm DyForm1 = new DynamicForm(zedGraphControl1_2);
            DyForm1.Show();
        }

        //private void ChnumcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch (ChnumcomboBox.SelectedIndex)
        //    {
        //        case 0:
        //            CH1checkBox.Checked = true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = false;
        //            CH5checkBox.Checked = false;
        //            CH6checkBox.Checked = false;
        //            CH7checkBox.Checked = false;
        //            CH8checkBox.Checked = false;
        //            CH9checkBox.Checked = false;
        //            CH10checkBox.Checked = false;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;

        //            CH4checkBox.Enabled = false;
        //            CH5checkBox.Enabled = false;
        //            CH6checkBox.Enabled = false;
        //            CH7checkBox.Enabled = false;
        //            CH8checkBox.Enabled = false;
        //            CH9checkBox.Enabled = false;
        //            CH10checkBox.Enabled = false;
        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        case 1:
        //            CH1checkBox.Checked = true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;
        //            CH5checkBox.Checked = false;
        //            CH6checkBox.Checked = false;
        //            CH7checkBox.Checked = false;
        //            CH8checkBox.Checked = false;
        //            CH9checkBox.Checked = false;
        //            CH10checkBox.Checked = false;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;

        //            CH5checkBox.Enabled = false;
        //            CH6checkBox.Enabled = false;
        //            CH7checkBox.Enabled = false;
        //            CH8checkBox.Enabled = false;
        //            CH9checkBox.Enabled = false;
        //            CH10checkBox.Enabled = false;
        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        case 2:
        //            CH1checkBox.Checked = true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;
        //            CH5checkBox.Checked = true;
        //            CH6checkBox.Checked = false;
        //            CH7checkBox.Checked = false;
        //            CH8checkBox.Checked = false;
        //            CH9checkBox.Checked = false;
        //            CH10checkBox.Checked = false;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;

        //            CH6checkBox.Enabled = false;
        //            CH7checkBox.Enabled = false;
        //            CH8checkBox.Enabled = false;
        //            CH9checkBox.Enabled = false;
        //            CH10checkBox.Enabled = false;
        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        case 3:
        //            CH1checkBox.Checked = true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;
        //            CH5checkBox.Checked = true;
        //            CH6checkBox.Checked = true;
        //            CH7checkBox.Checked = false;
        //            CH8checkBox.Checked = false;
        //            CH9checkBox.Checked = false;
        //            CH10checkBox.Checked = false;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;

        //            CH7checkBox.Enabled = false;
        //            CH8checkBox.Enabled = false;
        //            CH9checkBox.Enabled = false;
        //            CH10checkBox.Enabled = false;
        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        case 4:                                      
        //            CH1checkBox.Checked=  true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;                     
        //            CH5checkBox.Checked=  true;
        //            CH6checkBox.Checked = true;      
        //            CH7checkBox.Checked = true;
        //            CH8checkBox.Checked = false;
        //            CH9checkBox.Checked = false;
        //            CH10checkBox.Checked = false;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;                 

        //            CH8checkBox.Enabled = false;
        //            CH9checkBox.Enabled = false;
        //            CH10checkBox.Enabled = false;
        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        case 5:
        //            CH1checkBox.Checked = true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;
        //            CH5checkBox.Checked = true;
        //            CH6checkBox.Checked = true;
        //            CH7checkBox.Checked = true;
        //            CH8checkBox.Checked = true;
        //            CH9checkBox.Checked = false;
        //            CH10checkBox.Checked = false;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;

        //            CH9checkBox.Enabled = false;
        //            CH10checkBox.Enabled = false;
        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        case 6:
        //            CH1checkBox.Checked=true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;                     
        //            CH5checkBox.Checked=true;
        //            CH6checkBox.Checked = true;
        //            CH7checkBox.Checked = true;
        //            CH8checkBox.Checked = true;
        //            CH9checkBox.Checked = true;
        //            CH10checkBox.Checked = false;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;

        //            CH10checkBox.Enabled = false;
        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false; 
        //            break;
        //        case 7:
        //            CH1checkBox.Checked = true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;
        //            CH5checkBox.Checked = true;
        //            CH6checkBox.Checked = true;
        //            CH7checkBox.Checked = true;
        //            CH8checkBox.Checked = true;
        //            CH9checkBox.Checked = true;
        //            CH10checkBox.Checked = true;
        //            CH11checkBox.Checked = false;
        //            CH12checkBox.Checked = false;

        //            CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        case 8:
        //            CH1checkBox.Checked = true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;
        //            CH5checkBox.Checked = true;
        //            CH6checkBox.Checked = true;
        //            CH7checkBox.Checked = true;
        //            CH8checkBox.Checked = true;
        //            CH9checkBox.Checked = true;
        //            CH10checkBox.Checked = true;
        //            CH11checkBox.Checked = true;
        //            CH12checkBox.Checked = false;

        //            //CH7checkBox.Enabled = false;
        //            //CH8checkBox.Enabled = false;
        //            //CH9checkBox.Enabled = false;
        //            //CH10checkBox.Enabled = false;
        //            //CH11checkBox.Enabled = false;
        //            CH12checkBox.Enabled = false;
        //            break;
        //        default:
        //            CH1checkBox.Checked=true;
        //            CH2checkBox.Checked = true;
        //            CH3checkBox.Checked = true;
        //            CH4checkBox.Checked = true;                     
        //            CH5checkBox.Checked=true;
        //            CH6checkBox.Checked = true;
        //            CH7checkBox.Checked = true;
        //            CH8checkBox.Checked = true;
        //            CH9checkBox.Checked = true;
        //            CH10checkBox.Checked = true;
        //            CH11checkBox.Checked = true;
        //            CH12checkBox.Checked = true;

        //            CH5checkBox.Enabled = true;
        //            CH6checkBox.Enabled = true;
        //            CH7checkBox.Enabled = true;
        //            CH8checkBox.Enabled = true;
        //            CH9checkBox.Enabled = true;
        //            CH10checkBox.Enabled = true;
        //            CH11checkBox.Enabled = true;
        //            CH12checkBox.Enabled = true;
        //            break;                                      
        //    }
        //}

        private void Dynamicpic_2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DynamicpicForm df = new DynamicpicForm(Dynamicpic);
                df.Show();
            }
            catch
            {

            }
        }

        private void zedGraphControl2_DoubleClick(object sender, EventArgs e)
        {
            zed2Form DyForm2 = new zed2Form(zedGraphControl2_2);
            DyForm2.Show();
        }

        //绘画损失初始图
        //private void pic_2_Paint(object sender, PaintEventArgs e)
        //{
        //    //缺陷定位
        //    double cx = 0;
        //    double cy = 0;
        //    double cz = 0;
        //    int i = 0;
        //    double x0_2 = 0.025;
        //    double y0_2 = 0.025;
        //    double W_2 = 0.95;
        //    double H_2 = 0.90;
        //    float x = (float)(pic_2.Width * x0_2);
        //    float y = (float)(pic_2.Height * y0_2);
        //    float width = (float)(pic_2.Width * W_2);
        //    float height = (float)(pic_2.Height * H_2);
        //    int n = 0;
        //    double C;
        //    lenofpiece = double.Parse(DisText_2.Text);
        //    double Sen = double.Parse(yztext_2.Text);//灵敏度
        //    tiligaodu = double.Parse(TLBox_2.Text);
        //    double D = double.Parse(DText_2.Text);

        //    double max1 = 0;
        //    double min1 = 0;
        //    double max2 = 0;
        //    double min2 = 0;
        //    double max3 = 0;
        //    double min3 = 0;
        //    double max4 = 0;
        //    double min4 = 0;

        //    double up1 = 0;
        //    double down1 = 0;
        //    double up2 = 0;
        //    double down2 = 0;
        //    double up3 = 0;
        //    double down3 = 0;
        //    double up4 = 0;
        //    double down4 = 0;

        //    double value1 = 0;
        //    double value2 = 0;
        //    double value3 = 0;
        //    double value4 = 0;

        //    double sum_1 = 0;
        //    double mean_1 = 0;
        //    double biaozhuncha_sum_1 = 0;
        //    double biaozhuncha_1 = 0;
        //    double[] ysdata_new_1 = new double[1000000];
        //    double[] Dydata_new_1 = new double[1000000];

        //    double sum_3 = 0;
        //    double mean_3 = 0;
        //    double biaozhuncha_sum_3 = 0;
        //    double biaozhuncha_3 = 0;
        //    double[] ysdata_new_3 = new double[1000000];
        //    double[] Dydata_new_3 = new double[1000000];

        //    double[] ysdata_new = new double[1000000];
        //    double[] Dydata_new = new double[1000000];

        //    System.Random rnd = new System.Random();
        //    if (runflag && pointflag && readflag)
        //    {
        //        Graphics gg = e.Graphics;
        //        cx = (currentPoint.X - x) * lenofpiece / width;
        //        cy = (pic_2.Height - currentPoint.Y - 0.075 * pic_2.Height) * widthOfPiece / height;

        //        n = Convert.ToInt16(Math.Abs(cx) / (lenofpiece / (original_num - 1)));
        //        C = ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n];
        //        F = (ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n]) / (4 * Math.Pow(tiligaodu * 0.000000001, 2));
        //        //cz=Math.Abs ( -1.0212 * F - 2.5295)+0.2*rnd.NextDouble ();

        //        ////////////////////////////////张量///////////////////////////////////////////
        //        //if (Math.Abs(Freaddata[0, n]) > 0)
        //        //{
        //        //    if (Math.Abs(Freaddata[0, n]) > 0 && Math.Abs(Freaddata[0, n]) <= 100)
        //        //    {
        //        //        F1 = (Freaddata[0, n] + 400) * 100;
        //        //    }
        //        //    if (Math.Abs(Freaddata[0, n]) > 100 && Math.Abs(Freaddata[0, n]) <= 200)
        //        //    {
        //        //        F1 = (Freaddata[0, n] + 300) * 100;
        //        //    }
        //        //    if (Math.Abs(Freaddata[0, n]) > 200 && Math.Abs(Freaddata[0, n]) <= 300)
        //        //    {
        //        //        F1 = (Freaddata[0, n] + 200) * 100;
        //        //    }
        //        //    if (Math.Abs(Freaddata[0, n]) > 300 && Math.Abs(Freaddata[0, n]) <= 400)
        //        //    {
        //        //        F1 = (Freaddata[0, n] + 300) * 100;
        //        //    }
        //        //    if (Math.Abs(Freaddata[0, n]) > 400 && Math.Abs(Freaddata[0, n]) <= 500)
        //        //    {
        //        //        F1 = Freaddata[0, n] * 100;
        //        //    }
        //        //    if (Math.Abs(Freaddata[0, n]) > 500 && Math.Abs(Freaddata[0, n]) <= 600)
        //        //    {
        //        //        F1 = (Freaddata[0, n] - 100) * 100;
        //        //    }
        //        //    if (Math.Abs(Freaddata[0, n]) > 0 && Math.Abs(Freaddata[0, n]) <= 100)
        //        //    {
        //        //        F1 = (Freaddata[0, n] + 300) * 100;
        //        //    }
        //        //}
        //        //////////////////////////////////////////////////////////////////////////////
        //        max1 = ysdata[2, 0];
        //        min1 = ysdata[2, 0];
        //        max2 = ysdata[5, 0];
        //        min2 = ysdata[5, 0];
        //        max3 = ysdata[8, 0];
        //        min3 = ysdata[8, 0];
        //        max4 = ysdata[11, 0];
        //        min4 = ysdata[11, 0];
        //        for (i = 0; i < original_num - 1; i++)
        //        {
        //            if (max1 < ysdata[2, i + 1])
        //            {
        //                max1 = ysdata[2, i + 1];
        //            }
        //            if (min1 > ysdata[2, i + 1])
        //            {
        //                min1 = ysdata[2, i + 1];
        //            }
        //        }
        //        for (i = 0; i < original_num - 1; i++)
        //        {
        //            if (max2 < ysdata[5, i + 1])
        //            {
        //                max2 = ysdata[5, i + 1];
        //            }
        //            if (min2 > ysdata[5, i + 1])
        //            {
        //                min2 = ysdata[5, i + 1];
        //            }
        //        }
        //        for (i = 0; i < original_num - 1; i++)
        //        {
        //            if (max3 < ysdata[8, i + 1])
        //            {
        //                max3 = ysdata[8, i + 1];
        //            }
        //            if (min3 > ysdata[8, i + 1])
        //            {
        //                min3 = ysdata[8, i + 1];
        //            }
        //        }
        //        for (i = 0; i < original_num - 1; i++)
        //        {
        //            if (max4 < ysdata[11, i + 1])
        //            {
        //                max4 = ysdata[11, i + 1];
        //            }
        //            if (min4 > ysdata[11, i + 1])
        //            {
        //                min4 = ysdata[11, i + 1];
        //            }
        //        }

        //        //if (ysdata[2, n] >= 0 && ysdata[2, n] < 10000)
        //        //{
        //        //    ysdata[2, n] = ysdata[2, n] * 10;
        //        //}
        //        //if (ysdata[5, n] >= 0 && ysdata[5, n] < 10000)
        //        //{
        //        //    ysdata[5, n] = ysdata[5, n] * 10;
        //        //}
        //        //if (ysdata[8, n] >= 0 && ysdata[8, n] < 10000)
        //        //{
        //        //    ysdata[8, n] = ysdata[8, n] * 10;
        //        //}
        //        //if (ysdata[11, n] >= 0 && ysdata[11, n] < 10000)
        //        //{
        //        //    ysdata[11, n] = ysdata[11, n] * 10;
        //        //}

        //        //if (ysdata[2, n] >= 10000 && ysdata[2, n] < 20000)
        //        //{
        //        //    ysdata[2, n] = (ysdata[2, n] - 10000) * 10;
        //        //}
        //        //if (ysdata[5, n] >= 10000 && ysdata[5, n] < 20000)
        //        //{
        //        //    ysdata[5, n] = (ysdata[5, n] - 10000) * 10;
        //        //}
        //        //if (ysdata[8, n] >= 10000 && ysdata[8, n] < 20000)
        //        //{
        //        //    ysdata[8, n] = (ysdata[8, n] - 10000) * 10;
        //        //}
        //        //if (ysdata[11, n] >= 10000 && ysdata[11, n] < 20000)
        //        //{
        //        //    ysdata[11, n] = (ysdata[11, n] - 10000) * 10;
        //        //}

        //        //if (ysdata[2, n] >= 20000 && ysdata[2, n] < 30000)
        //        //{
        //        //    ysdata[2, n] = (ysdata[2, n] - 20000) * 10;
        //        //}
        //        //if (ysdata[5, n] >= 20000 && ysdata[5, n] < 30000)
        //        //{
        //        //    ysdata[5, n] = (ysdata[5, n] - 20000) * 10;
        //        //}
        //        //if (ysdata[8, n] >= 20000 && ysdata[8, n] < 30000)
        //        //{
        //        //    ysdata[8, n] = (ysdata[8, n] - 20000) * 10;
        //        //}
        //        //if (ysdata[11, n] >= 20000 && ysdata[11, n] < 30000)
        //        //{
        //        //    ysdata[11, n] = (ysdata[11, n] - 20000) * 10;
        //        //}


        //        //if (tiligaodu >= 1000 && tiligaodu < 1800)
        //        //{
        //        //    tiligaodu1 = 150;
        //        //}
        //        //if (tiligaodu >= 1800 && tiligaodu < 2500)
        //        //{
        //        //    tiligaodu1 = 200;
        //        //}
        //        //if (tiligaodu >= 2500 && tiligaodu < 3000)
        //        //{
        //        //    tiligaodu1 = 250;
        //        //}
        //        //if(tiligaodu1==150)
        //        //{
        //        //    if (ysdata[2, n] > 30000 && ysdata[2, n] < 50000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] - 20000;
        //        //    }
        //        //    if (ysdata[5, n] > 30000 && ysdata[5, n] < 50000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] - 20000;
        //        //    }
        //        //    if (ysdata[8, n] > 30000 && ysdata[8, n] < 50000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] - 20000;
        //        //    }
        //        //    if (ysdata[11, n] > 30000 && ysdata[11, n] < 50000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] - 20000;
        //        //    }

        //        //    if (ysdata[2, n] > 40000 && ysdata[2, n] < 60000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] - 30000;
        //        //    }
        //        //    if (ysdata[5, n] > 40000 && ysdata[5, n] < 60000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] - 30000;
        //        //    }
        //        //    if (ysdata[8, n] > 40000 && ysdata[8, n] < 60000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] - 30000;
        //        //    }
        //        //    if (ysdata[11, n] > 40000 && ysdata[11, n] < 60000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] - 30000;
        //        //    }

        //        //    if (ysdata[2, n] > 60000 && ysdata[2, n] < 80000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] - 50000;
        //        //    }
        //        //    if (ysdata[5, n] > 60000 && ysdata[5, n] < 80000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] - 50000;
        //        //    }
        //        //    if (ysdata[8, n] > 60000 && ysdata[8, n] < 80000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] - 50000;
        //        //    }
        //        //    if (ysdata[11, n] > 60000 && ysdata[11, n] < 80000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] - 50000;
        //        //    }

        //        //    if (ysdata[2, n] > 80000 && ysdata[2, n] < 100000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] - 70000;
        //        //    }
        //        //    if (ysdata[5, n] > 80000 && ysdata[5, n] < 100000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] - 70000;
        //        //    }
        //        //    if (ysdata[8, n] > 80000 && ysdata[8, n] < 100000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] - 70000;
        //        //    }
        //        //    if (ysdata[11, n] > 80000 && ysdata[11, n] < 100000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] - 70000;
        //        //    }
        //        //    //ysdata[2, n] = (((ysdata[2, n] - min1) / (max1 - min1))*20000) + 10000;
        //        //    //ysdata[5, n] = (((ysdata[5, n] - min2) / (max2 - min2)) * 20000) + 10000;
        //        //    //ysdata[8, n] = (((ysdata[8, n] - min3) / (max3 - min3)) * 20000) + 10000;
        //        //    //ysdata[11, n] = (((ysdata[11, n] - min4) / (max4 - min4)) * 20000) + 10000;
        //        //}
        //        //if(tiligaodu1==200)
        //        //{
        //        //    if (ysdata[2, n] > 40000 && ysdata[2, n] < 60000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] - 20000;
        //        //    }
        //        //    if (ysdata[5, n] > 40000 && ysdata[5, n] < 60000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] - 20000;
        //        //    }
        //        //    if (ysdata[8, n] > 40000 && ysdata[8, n] < 60000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] - 20000;
        //        //    }
        //        //    if (ysdata[11, n] > 40000 && ysdata[11, n] < 60000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] - 20000;
        //        //    }

        //        //    if (ysdata[2, n] > 0 && ysdata[2, n] < 20000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] + 20000;
        //        //    }
        //        //    if (ysdata[5, n] > 0 && ysdata[5, n] < 20000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] + 20000;
        //        //    }
        //        //    if (ysdata[8, n] > 0 && ysdata[8, n] < 20000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] + 20000;
        //        //    }
        //        //    if (ysdata[11, n] > 0 && ysdata[11, n] < 20000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] + 20000;
        //        //    }
        //        //    //ysdata[2, n] = (((ysdata[2, n] - min1) / (max1 - min1)) * 20000) + 20000;
        //        //    //ysdata[5, n] = (((ysdata[5, n] - min2) / (max2 - min2)) * 20000) + 20000;
        //        //    //ysdata[8, n] = (((ysdata[8, n] - min3) / (max3 - min3)) * 20000) + 20000;
        //        //    //ysdata[11, n] = (((ysdata[11, n] - min4) / (max4 - min4)) * 20000) + 20000;
        //        //}
        //        //if(tiligaodu1==250)
        //        //{
        //        //    if (ysdata[2, n] > 45000 && ysdata[2, n] < 65000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] - 20000;
        //        //    }
        //        //    if (ysdata[5, n] > 45000 && ysdata[5, n] < 65000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] - 20000;
        //        //    }
        //        //    if (ysdata[8, n] > 45000 && ysdata[8, n] < 65000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] - 20000;
        //        //    }
        //        //    if (ysdata[11, n] > 45000 && ysdata[11, n] < 65000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] - 20000;
        //        //    }

        //        //    if (ysdata[2, n] > 500 && ysdata[2, n] < 25000)
        //        //    {
        //        //        ysdata[2, n] = ysdata[2, n] + 20000;
        //        //    }
        //        //    if (ysdata[5, n] > 500 && ysdata[5, n] < 25000)
        //        //    {
        //        //        ysdata[5, n] = ysdata[5, n] + 20000;
        //        //    }
        //        //    if (ysdata[8, n] > 500 && ysdata[8, n] < 25000)
        //        //    {
        //        //        ysdata[8, n] = ysdata[8, n] + 20000;
        //        //    }
        //        //    if (ysdata[11, n] > 500 && ysdata[11, n] < 25000)
        //        //    {
        //        //        ysdata[11, n] = ysdata[11, n] + 20000;
        //        //    }
        //        //    //ysdata[2, n] = (((ysdata[2, n] - min1) / (max1 - min1)) * 20000) + 25000;
        //        //    //ysdata[5, n] = (((ysdata[5, n] - min2) / (max2 - min2)) * 20000) + 25000;
        //        //    //ysdata[8, n] = (((ysdata[8, n] - min3) / (max3 - min3)) * 20000) + 25000;
        //        //    //ysdata[11, n] = (((ysdata[11, n] - min4) / (max4 - min4)) * 20000) + 25000;
        //        //}

        //        up1 = ((max1 / 1000) + 1) * 1000;
        //        down1 = ((min1 / 1000) - 1) * 1000;
        //        value1 = (((ysdata[2, n] - down1) / (up1 - down1)) * 10000) + 10000;

        //        up2 = ((max2 / 1000) + 1) * 1000;
        //        down2 = ((min2 / 1000) - 1) * 1000;
        //        value2 = (((ysdata[5, n] - down2) / (up2 - down2)) * 10000) + 10000;

        //        up3 = ((max3 / 1000) + 1) * 1000;
        //        down3 = ((min3 / 1000) - 1) * 1000;
        //        value3 = (((ysdata[8, n] - down3) / (up3 - down3)) * 10000) + 10000;

        //        up4 = ((max4 / 1000) + 1) * 1000;
        //        down4 = ((min4 / 1000) - 1) * 1000;
        //        value4 = (((ysdata[11, n] - down4) / (up4 - down4)) * 10000) + 10000;

        //        //if(ysdata[2,n]>=10000&&ysdata[2,n]<30000)
        //        //{
        //        //    value1=(((ysdata[2,n]-10000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[5,n]>=10000&&ysdata[5,n]<30000)
        //        //{
        //        //    value2=(((ysdata[5,n]-10000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[8,n]>=10000&&ysdata[8,n]<30000)
        //        //{
        //        //    value3=(((ysdata[8,n]-10000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[11,n]>=10000&&ysdata[11,n]<30000)
        //        //{
        //        //    value4=(((ysdata[11,n]-10000)/20000)*10000)+10000;
        //        //}

        //        //if(ysdata[2,n]>=20000&&ysdata[2,n]<40000)
        //        //{
        //        //    value1=(((ysdata[2,n]-20000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[5,n]>=20000&&ysdata[5,n]<40000)
        //        //{
        //        //    value2=(((ysdata[5,n]-20000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[8,n]>=20000&&ysdata[8,n]<40000)
        //        //{
        //        //    value3=(((ysdata[8,n]-20000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[11,n]>=20000&&ysdata[11,n]<40000)
        //        //{
        //        //    value4=(((ysdata[11,n]-20000)/20000)*10000)+10000;
        //        //}

        //        //if(ysdata[2,n]>=30000&&ysdata[2,n]<50000)
        //        //{
        //        //    value1=(((ysdata[2,n]-30000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[5,n]>=30000&&ysdata[5,n]<50000)
        //        //{
        //        //    value2=(((ysdata[5,n]-30000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[8,n]>=30000&&ysdata[8,n]<50000)
        //        //{
        //        //    value3=(((ysdata[8,n]-30000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[11,n]>=30000&&ysdata[11,n]<50000)
        //        //{
        //        //    value4=(((ysdata[11,n]-30000)/20000)*10000)+10000;
        //        //}

        //        //if(ysdata[2,n]>=40000&&ysdata[2,n]<60000)
        //        //{
        //        //    value1=(((ysdata[2,n]-40000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[5,n]>=40000&&ysdata[5,n]<60000)
        //        //{
        //        //    value2=(((ysdata[5,n]-40000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[8,n]>=40000&&ysdata[8,n]<60000)
        //        //{
        //        //    value3=(((ysdata[8,n]-40000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[11,n]>=40000&&ysdata[11,n]<60000)
        //        //{
        //        //    value4=(((ysdata[11,n]-40000)/20000)*10000)+10000;
        //        //}

        //        //if(ysdata[2,n]>=50000&&ysdata[2,n]<70000)
        //        //{
        //        //    value1=(((ysdata[2,n]-50000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[5,n]>=50000&&ysdata[5,n]<70000)
        //        //{
        //        //    value2=(((ysdata[5,n]-50000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[8,n]>=50000&&ysdata[8,n]<70000)
        //        //{
        //        //    value3=(((ysdata[8,n]-50000)/20000)*10000)+10000;
        //        //}
        //        //if(ysdata[11,n]>=50000&&ysdata[11,n]<70000)
        //        //{
        //        //    value4=(((ysdata[11,n]-50000)/20000)*10000)+10000;
        //        //}



        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            sum_1 = sum_1 + Dydata2[2, j];
        //        }
        //        mean_1 = sum_1 / (original_num - 1);
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((Dydata2[2, j] - mean_1), 2);
        //        }
        //        biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            Dydata_new_1[j] = (Dydata2[2, j] - mean_1) / biaozhuncha_1;
        //        }

        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            sum_3 = sum_3 + Dydata2[8, j];
        //        }
        //        mean_3 = sum_3 / (original_num - 1);
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((Dydata2[8, j] - mean_3), 2);
        //        }
        //        biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            Dydata_new_3[j] = (Dydata2[8, j] - mean_3) / biaozhuncha_3;
        //        }

        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            Dydata_new[j] = (Dydata_new_1[j] + Dydata_new_3[j]) / 2;
        //        }





        //        //F1 = Math.Pow(150, 2) / value1;
        //        //F2 = Math.Pow(150, 2) / value2;
        //        //F3 = Math.Pow(150, 2) / value3;
        //        //F4 = Math.Pow(150, 2) / value4;
        //        if ((Freaddata[0, n] == 0) && (Freaddata[1, n] == 0) && (Freaddata[2, n] == 0) && (Freaddata[3, n] == 0) && (Freaddata[4, n] == 0))
        //        {
        //            cz = 0;

        //        }
        //        if (bdxstext1_2.Text == "")
        //        {
        //            double fushi_canshu_a = 0;
        //            double fushi_canshu_b = 0;

        //            int TLBox1 = Int32.Parse(TLBox_2.Text);
        //            if (tiligaodu >= 0 && tiligaodu < 250)
        //            {
        //                fushi_canshu_a = 9.45E+01;
        //                fushi_canshu_b = -1.346;

        //            }
        //            if (tiligaodu >= 250 && tiligaodu < 480)
        //            {
        //                fushi_canshu_a = 120.6;
        //                fushi_canshu_b = -1.385;
        //            }
        //            if (tiligaodu >= 480 && tiligaodu < 700)
        //            {
        //                fushi_canshu_a = 61.12;
        //                fushi_canshu_b = -1.027;
        //            }
        //            if (tiligaodu >= 700 && tiligaodu < 970)
        //            {
        //                fushi_canshu_a = 260.2;
        //                fushi_canshu_b = -1.5;
        //            }
        //            if (tiligaodu >= 970 && tiligaodu < 1100)
        //            {
        //                fushi_canshu_a = 46.3;
        //                fushi_canshu_b = -1.054;
        //            }
        //            if (tiligaodu >= 1100 && tiligaodu < 1350)
        //            {
        //                fushi_canshu_a = 1.05E+06;
        //                fushi_canshu_b = -9.415;
        //            }
        //            if (tiligaodu >= 1350 && tiligaodu < 1620)
        //            {
        //                fushi_canshu_a = 10.5;
        //                fushi_canshu_b = -1.584;
        //            }
        //            if (tiligaodu >= 1620 && tiligaodu < 1860)
        //            {
        //                fushi_canshu_a = 784.8;
        //                fushi_canshu_b = -3.046;
        //            }
        //            if (tiligaodu > 1860)
        //            {
        //                fushi_canshu_a = 28.22;
        //                fushi_canshu_b = -1.784;
        //            }
        //            //cz = (40 * Math.Exp(-1.5 * F1) + 40 * Math.Exp(-1.5 * F2) + 40 * Math.Exp(-1.5 * F3) + 40 * Math.Exp(-1.5 * F4)) / 4;
        //            // cz = 0.3102 * Math.Exp(1.8 * 1.8 * 0.7031 * Dydata_new[n]) + 0.6342;
        //            cz = ThickOftube * fushi_canshu_a * Math.Exp(fushi_canshu_b * ysdata_new_1[n]) / 100;
        //            if (cz > ThickOftube)
        //            {
        //                cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //            }


        //        }

        //        else
        //        {
        //            double huiguixishu11 = Convert.ToDouble(huiguixishu1_2);
        //            double huiguixishu22 = Convert.ToDouble(huiguixishu2_2);

        //            cz = ThickOftube * huiguixishu11 * Math.Exp(huiguixishu22 * ysdata_new_1[n]) / 100;
        //            if (cz > ThickOftube)
        //            {
        //                cz = ThickOftube * 0.8 + 0.5 * rnd.NextDouble();
        //            }
        //            if (cz == 0)
        //            {
        //                cz = ThickOftube * 0.2 + 0.5 * rnd.NextDouble();
        //            }

        //            if ((Freaddata[0, n] == 0) && (Freaddata[1, n] == 0) && (Freaddata[2, n] == 0) && (Freaddata[3, n] == 0) && (Freaddata[4, n] == 0))
        //            {
        //                cz = 0;
        //            }
        //        }









        //        //else
        //        //{
        //        //    cz = (40 * Math.Exp(-1.5 * F1) + 40 * Math.Exp(-1.5 * F2) + 40 * Math.Exp(-1.5 * F3) + 40 * Math.Exp(-1.5 * F4)) / 4;
        //        //    if (cz > ThickOftube)
        //        //    {
        //        //        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //        //    }
        //        //    //if(tiligaodu1 == 150)
        //        //    //{
        //        //    //    cz = (40 * Math.Exp(-1.5* F1) + 40 * Math.Exp(-1.5 * F2) + 40 * Math.Exp(-1.5 * F3) + 40 * Math.Exp(-1.5 * F4)) / 4;
        //        //    //}
        //        //    //if (tiligaodu1 == 200)
        //        //    //{
        //        //    //    cz = (0.6* Math.Exp(1.5 * F1) + 0.6 * Math.Exp(1.5 * F2) + 0.6 * Math.Exp(1.5 * F3) + 0.6 * Math.Exp(1.5 * F4)) / 4;
        //        //    //}
        //        //    //if (tiligaodu1 == 250)
        //        //    //{
        //        //    //    cz = (0.4 * Math.Exp(1.5 * F1) + 0.4 * Math.Exp(1.5 * F2) + 0.4 * Math.Exp(1.5 * F3) + 0.4 * Math.Exp(1.5 * F4)) / 4;
        //        //    //}
        //        //    //if (cz > ThickOftube)
        //        //    //{
        //        //    //    //cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //        //    //    ysdata[2, n] = ysdata[2, n] + 10000;
        //        //    //    ysdata[5, n] = ysdata[5, n] + 10000;
        //        //    //    ysdata[8, n] = ysdata[8, n] + 10000;
        //        //    //    ysdata[11, n] = ysdata[11, n] + 10000;

        //        //    //    F1 = Math.Pow(tiligaodu1, 2) / ysdata[2, n];
        //        //    //    F2 = Math.Pow(tiligaodu1, 2) / ysdata[5, n];
        //        //    //    F3 = Math.Pow(tiligaodu1, 2) / ysdata[8, n];
        //        //    //    F4 = Math.Pow(tiligaodu1, 2) / ysdata[11, n];

        //        //    //    if (tiligaodu1 == 150)
        //        //    //    {
        //        //    //        cz = (1.2 * Math.Exp(1.5 * F1) + 1.2 * Math.Exp(1.5 * F2) + 1.2 * Math.Exp(1.5 * F3) + 1.2 * Math.Exp(1.5 * F4)) / 4;
        //        //    //    }
        //        //    //    if (tiligaodu1 == 200)
        //        //    //    {
        //        //    //        cz = (0.6 * Math.Exp(1.5 * F1) + 0.6 * Math.Exp(1.5 * F2) + 0.6 * Math.Exp(1.5 * F3) + 0.6 * Math.Exp(1.5 * F4)) / 4;
        //        //    //    }
        //        //    //    if (tiligaodu1 == 250)
        //        //    //    {
        //        //    //        cz = (0.4 * Math.Exp(1.5 * F1) + 0.4 * Math.Exp(1.5 * F2) + 0.4 * Math.Exp(1.5 * F3) + 0.4 * Math.Exp(1.5 * F4)) / 4;
        //        //    //    }
        //        //    //    if (cz > ThickOftube)
        //        //    //    {
        //        //    //        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //        //    //    }
        //        //    //}
        //        //}
        //        //else 
        //        //{
        //        //    if ((D <= 100) && (D > 0))
        //        //    {
        //        //        cz = (2.182 * 0.00000001 * Math.Exp(3.009 * F1) + 1.92 * 0.00000001 * Math.Exp(3.014 * F2) + 0.9927 * 0.00000001 * Math.Exp(3.132 * F3) + 2.978 * 0.00000001 * Math.Exp(3.017 * F4)) / 4;
        //        //    }
        //        //    if ((D > 100) && (D <= 200))
        //        //    {
        //        //        cz = (3.826 * Math.Pow(10, -5) * Math.Exp(1.762 * F1) + 4.291 * Math.Pow(10, -5) * Math.Exp(1.735 * F2) + 3.799 * Math.Pow(10, -5) * Math.Exp(1.767 * F3) + 3.505 * Math.Pow(10, -5) * Math.Exp(1.809 * F4)) / 4;
        //        //    }
        //        //    if ((D > 200) && (D <= 300))
        //        //    {
        //        //        cz = (1.04 * Math.Pow(10, -7) * Math.Exp(3.039 * F1) + 0.9371 * Math.Pow(10, -7) * Math.Exp(3.044 * F2) + 1.083 * Math.Pow(10, -7) * Math.Exp(3.04 * F3) + 1.239 * Math.Pow(10, -7) * Math.Exp(3.067 * F4)) / 4;
        //        //    }
        //        //    if ((D > 300) && (D <= 400))
        //        //    {
        //        //        cz = (7.303 * Math.Pow(10, 4) * Math.Exp(-1.941 * F1) + 7.271 * Math.Pow(10, 4) * Math.Exp(-1.932 * F2) + 7.417 * Math.Pow(10, 4) * Math.Exp(-1.948 * F3) + 7.482 * Math.Pow(10, 4) * Math.Exp(-1.982 * F4)) / 4;
        //        //    }
        //        //    if ((D > 400) && (D <= 500))
        //        //    {
        //        //        cz = (8.494 * Math.Pow(10, 31) * Math.Exp(-10.56 * F1) + 3.215 * Math.Pow(10, 30) * Math.Exp(-10.06 * F2) + 1.375 * Math.Pow(10, 32) * Math.Exp(-10.68 * F3) + 6.185 * Math.Pow(10, 33) * Math.Exp(-11.41 * F4)) / 4;
        //        //    }
        //        //    if (D > 500)
        //        //    {
        //        //        cz = (1.03 * Math.Pow(10, 21) * Math.Exp(-4.3 * F1) + 5.897 * Math.Pow(10, 20) * Math.Exp(-4.241 * F2) + 1.019 * Math.Pow(10, 21) * Math.Exp(-4.317 * F3) + 2.076 * Math.Pow(10, 21) * Math.Exp(-4.44 * F4)) / 4;
        //        //    }
        //        //    if (cz > ThickOftube)
        //        //    {
        //        //        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //        //    }

        //        //}
        //        //else
        //        //{
        //        //    if ((D <= 100) && (D > 0))
        //        //    {
        //        //        cz = (2.468 * Math.Exp(0.8385 * F1) + 2.062 * Math.Exp(1.111 * F2) + 2.113 * Math.Exp(1.079 * F3) + 2.588 * Math.Exp(0.7795 * F4)) / 4;
        //        //    }
        //        //    if ((D > 100) && (D <= 200))
        //        //    {
        //        //        cz = (3.515 * Math.Exp(1.97 * F1) + 3.289 * Math.Exp(1.97 * F2) + 3.401 * Math.Exp(1.98 * F3) + 3.499 * Math.Exp(2.07 * F4)) / 4;
        //        //    }
        //        //    if ((D > 200) && (D <= 300))
        //        //    {
        //        //        cz = (1.904 * Math.Exp(3.042 * F1) + 2.177 * Math.Exp(3.013 * F2) + 1.818 * Math.Exp(3.057 * F3) + 1.514 * Math.Exp(3.132 * F4)) / 4;
        //        //    }
        //        //    if ((D > 300) && (D <= 400))
        //        //    {
        //        //        cz = (4.457 * Math.Exp(1.715 * F1) + 4.64 * Math.Exp(1.714 * F2) + 4.605 * Math.Exp(1.723 * F3) + 4.439 * Math.Exp(1.745 * F4)) / 4;
        //        //    }
        //        //    if ((D > 400) && (D <= 500))
        //        //    {
        //        //        cz = (4.4663 * Math.Exp(2.509 * F1) + 1.952 * Math.Exp(2.37 * F2) + 7.502 * Math.Exp(2.593 * F3) + 2.372 * Math.Exp(2.813 * F4)) / 4;
        //        //    }
        //        //    if (D > 500)
        //        //    {
        //        //        cz = (1.188 * Math.Exp(3.4 * F1) + 2.677 * Math.Exp(3.2 * F2) + 1.54 * Math.Exp(3.4 * F3) + 1.38 * Math.Exp(3.8 * F4)) / 4;
        //        //    }
        //        //    if (cz > ThickOftube)
        //        //    {
        //        //        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //        //    }
        //        //}

        //        gg.DrawLine(Pens.White, x, currentPoint.Y, x + width, currentPoint.Y); //绘制横线  
        //        gg.DrawLine(Pens.White, currentPoint.X, y, currentPoint.X, y + height); //会制纵线 

        //        ZtextBox_2.Text = string.Format("{0:0.00}", cz) + "mm";

        //        ////计算区域面积
        //        if (cishu == 4)
        //        {
        //            double bianchang = Math.Abs(x1 - x2) * lenofpiece / width;  //边长
        //            double kuandu = Math.Abs(y1 - y2) * widthOfPiece / height;   //宽度
        //            double area = bianchang * kuandu / 100;    //面积(平方厘米)
        //            MjtextBox_2.Text = string.Format("{0:0.0}", area) + "cm²";
        //            //MjtextBox_2.Text = Convert .ToString (area);
        //        }
        //        /////
        //    }
        //    else
        //    {
        //        Graphics gg = e.Graphics;
        //        cx = (currentPoint.X - x) * lenofpiece / width;
        //        cy = (pic_2.Height - currentPoint.Y - 0.075 * pic_2.Height) * widthOfPiece / height;

        //        n = Convert.ToInt16(Math.Abs(cx) / (lenofpiece / (original_num - 1)));
        //        C = ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n];
        //        F = (ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n]) / (4 * Math.Pow(tiligaodu * 0.000000001, 2));
        //        //cz=Math.Abs ( -1.0212 * F - 2.5295)+0.2*rnd.NextDouble ();
        //        //max1 = Dydata2[2, 0];
        //        //min1 = Dydata2[2, 0];
        //        //max2 = Dydata2[5, 0];
        //        //min2 = Dydata2[5, 0];
        //        //max3 = Dydata2[8, 0];
        //        //min3 = Dydata2[8, 0];
        //        //max4 = Dydata2[11, 0];
        //        //min4 = Dydata2[11, 0];
        //        //for (i = 0; i < original_num - 1; i++)
        //        //{
        //        //    if (max1 < Dydata2[2, i + 1])
        //        //    {
        //        //        max1 = Dydata2[2, i + 1];
        //        //    }
        //        //    if (min1 > Dydata2[2, i + 1])
        //        //    {
        //        //        min1 = Dydata2[2, i + 1];
        //        //    }
        //        //}
        //        //for (i = 0; i < original_num - 1; i++)
        //        //{
        //        //    if (max2 < Dydata2[5, i + 1])
        //        //    {
        //        //        max2 = Dydata2[5, i + 1];
        //        //    }
        //        //    if (min2 > Dydata2[5, i + 1])
        //        //    {
        //        //        min2 = Dydata2[5, i + 1];
        //        //    }
        //        //}
        //        //for (i = 0; i < original_num - 1; i++)
        //        //{
        //        //    if (max3 < Dydata2[8, i + 1])
        //        //    {
        //        //        max3 = Dydata2[8, i + 1];
        //        //    }
        //        //    if (min3 > Dydata2[8, i + 1])
        //        //    {
        //        //        min3 = Dydata2[8, i + 1];
        //        //    }
        //        //}
        //        //for (i = 0; i < original_num - 1; i++)
        //        //{
        //        //    if (max4 < Dydata2[11, i + 1])
        //        //    {
        //        //        max4 = Dydata2[11, i + 1];
        //        //    }
        //        //    if (min4 > Dydata2[11, i + 1])
        //        //    {
        //        //        min4 = Dydata2[11, i + 1];
        //        //    }
        //        //}

        //        //up1 = ((max1 / 1000) + 1) * 1000;
        //        //down1 = ((min1 / 1000) - 1) * 1000;
        //        //value1 = (((Dydata2[2, n] - down1) / (up1 - down1)) * 10000) + 10000;

        //        //up2 = ((max2 / 1000) + 1) * 1000;
        //        //down2 = ((min2 / 1000) - 1) * 1000;
        //        //value2 = (((Dydata2[5, n] - down2) / (up2 - down2)) * 10000) + 10000;

        //        //up3 = ((max3 / 1000) + 1) * 1000;
        //        //down3 = ((min3 / 1000) - 1) * 1000;
        //        //value3 = (((Dydata2[8, n] - down3) / (up3 - down3)) * 10000) + 10000;

        //        //up4 = ((max4 / 1000) + 1) * 1000;
        //        //down4 = ((min4 / 1000) - 1) * 1000;
        //        //value4 = (((Dydata2[11, n] - down4) / (up4 - down4)) * 10000) + 10000;

        //        //F1 = Math.Pow(150, 2) / value1;
        //        //F2 = Math.Pow(150, 2) / value2;
        //        //F3 = Math.Pow(150, 2) / value3;
        //        //F4 = Math.Pow(150, 2) / value4;


        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            sum_1 = sum_1 + Dydata2[2, j];
        //        }
        //        mean_1 = sum_1 / (original_num - 1);
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((Dydata2[2, j] - mean_1), 2);
        //        }
        //        biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            Dydata_new_1[j] = (Dydata2[2, j] - mean_1) / biaozhuncha_1;
        //        }

        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            sum_3 = sum_3 + Dydata2[8, j];
        //        }
        //        mean_3 = sum_3 / (original_num - 1);
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((Dydata2[8, j] - mean_3), 2);
        //        }
        //        biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            Dydata_new_3[j] = (Dydata2[8, j] - mean_3) / biaozhuncha_3;
        //        }

        //        for (int j = 0; j < original_num - 1; j++)
        //        {
        //            Dydata_new[j] = (Dydata_new_1[j] + Dydata_new_3[j]) / 2;
        //        }
        //        ////////////////////////////////////////////
        //        if ((Freaddata[0, n] == 0) && (Freaddata[1, n] == 0) && (Freaddata[2, n] == 0) && (Freaddata[3, n] == 0) && (Freaddata[4, n] == 0))
        //        {
        //            cz = 0;
        //        }
        //        if (bdxstext1_2.Text == "")
        //        {
        //            double fushi_canshu_a = 0;
        //            double fushi_canshu_b = 0;

        //            int TLBox1 = Int32.Parse(TLBox_2.Text);
        //            if (tiligaodu >= 0 && tiligaodu < 250)
        //            {
        //                fushi_canshu_a = 9.45E+01;
        //                fushi_canshu_b = -1.346;

        //            }
        //            if (tiligaodu >= 250 && tiligaodu < 480)
        //            {
        //                fushi_canshu_a = 120.6;
        //                fushi_canshu_b = -1.385;
        //            }
        //            if (tiligaodu >= 480 && tiligaodu < 700)
        //            {
        //                fushi_canshu_a = 61.12;
        //                fushi_canshu_b = -1.027;
        //            }
        //            if (tiligaodu >= 700 && tiligaodu < 970)
        //            {
        //                fushi_canshu_a = 260.2;
        //                fushi_canshu_b = -1.5;
        //            }
        //            if (tiligaodu >= 970 && tiligaodu < 1100)
        //            {
        //                fushi_canshu_a = 46.3;
        //                fushi_canshu_b = -1.054;
        //            }
        //            if (tiligaodu >= 1100 && tiligaodu < 1350)
        //            {
        //                fushi_canshu_a = 1.05E+06;
        //                fushi_canshu_b = -9.415;
        //            }
        //            if (tiligaodu >= 1350 && tiligaodu < 1620)
        //            {
        //                fushi_canshu_a = 10.5;
        //                fushi_canshu_b = -1.584;
        //            }
        //            if (tiligaodu >= 1620 && tiligaodu < 1860)
        //            {
        //                fushi_canshu_a = 784.8;
        //                fushi_canshu_b = -3.046;
        //            }
        //            if (tiligaodu > 1860)
        //            {
        //                fushi_canshu_a = 28.22;
        //                fushi_canshu_b = -1.784;
        //            }
        //            //cz = (40 * Math.Exp(-1.5 * F1) + 40 * Math.Exp(-1.5 * F2) + 40 * Math.Exp(-1.5 * F3) + 40 * Math.Exp(-1.5 * F4)) / 4;
        //            // cz = 0.3102 * Math.Exp(1.8 * 1.8 * 0.7031 * Dydata_new[n]) + 0.6342;
        //            cz = ThickOftube * fushi_canshu_a * Math.Exp(fushi_canshu_b * Dydata_new_1[n]) / 100;
        //            if (cz > ThickOftube)
        //            {
        //                cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //            }
        //            if ((Freaddata[0, n] == 0) && (Freaddata[1, n] == 0) && (Freaddata[2, n] == 0) && (Freaddata[3, n] == 0) && (Freaddata[4, n] == 0))
        //            {
        //                cz = 0;
        //            }


        //            else
        //            {
        //                cz = (40 * Math.Exp(-1.5 * F1) + 40 * Math.Exp(-1.5 * F2) + 40 * Math.Exp(-1.5 * F3) + 40 * Math.Exp(-1.5 * F4)) / 4;
        //                if (cz > ThickOftube)
        //                {
        //                    cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
        //                }
        //            }

        //            gg.DrawLine(Pens.White, x, currentPoint.Y, x + width, currentPoint.Y); //绘制横线  
        //            gg.DrawLine(Pens.White, currentPoint.X, y, currentPoint.X, y + height); //会制纵线 

        //            ZtextBox_2.Text = string.Format("{0:0.00}", cz) + "mm";

        //            ////计算区域面积
        //            if (cishu == 4)
        //            {
        //                double bianchang = Math.Abs(x1 - x2) * lenofpiece / width;  //边长
        //                double kuandu = Math.Abs(y1 - y2) * widthOfPiece / height;   //宽度
        //                double area = bianchang * kuandu / 100;    //面积(平方厘米)
        //                MjtextBox_2.Text = string.Format("{0:0.0}", area) + "cm²";
        //                //MjtextBox_2.Text = Convert .ToString (area);
        //            }
        //        }
        //    }
        //}

        //private void WcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DXMAX = double.Parse(WcomboBox.Text);
        //    pic_2XMAX = DXMAX;           
        //    DXMAXStep = DXMAX / 10;
        //    ZedGraphInit();       //曲线图初始化
        //}

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpClient.Close();
            Application.Exit(); //程序退出
        }

        private void Dynamicpic_2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void qccdtext_TextChanged(object sender, EventArgs e)
        {

        }

        //获取单击的点x,y坐标位置
        private double x1 = 0;
        private double x2 = 0;
        private double y1 = 0;
        private double y2 = 0;

        private double cishu = 0;  //统计单击的次数
        private void pic_2_MouseClick(object sender, MouseEventArgs e)
        {
            if (cishu >= 4)  //即当超过4次后又重新计算次数
            {
                cishu = 0;
            }

            cishu++;

            if (cishu == 1)   //第一次取点
            {
                x1 = currentPoint.X;
                Tslabel_2.Text = "请选取第二个点" + "\r\n" + "的x轴坐标";
            }
            else if (cishu == 2)
            {
                x2 = currentPoint.X;
                Tslabel_2.Text = "请选取第三个点" + "\r\n" + "的y轴坐标";
            }
            else if (cishu == 3)
            {
                y1 = currentPoint.Y;
                Tslabel_2.Text = "请选取第四个点" + "\r\n" + "的y轴坐标";
            }
            else if (cishu == 4)
            {
                y2 = currentPoint.Y;
                Tslabel_2.Text = "请选取第一个点" + "\r\n" + "的x轴坐标";
            }


        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void keyboard_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");

        }

        //////
        Rectangle rec1 = new Rectangle(618, 74, 75, 23);  //之前button1的位置，控制da que xian ban yuan guan (位置改变时要做相应修改)
        Rectangle rec2 = new Rectangle(618, 99, 75, 23);  //之前button2的位置，控制xiao que xian ban yuan guan  

        Rectangle rec3 = new Rectangle(730, 7, 75, 23);  //qu chu chang du jia 100
        Rectangle rec4 = new Rectangle(730, 34, 75, 23);  //gui wei

        Rectangle rec5 = new Rectangle(691, 63, 53, 23);  // yvzhi bian
        Rectangle rec6 = new Rectangle(743, 63, 53, 23);  // yvzhi gui

        Rectangle rec7 = new Rectangle(803, 35, 82, 10);  // diejia7mm chu xian
        Rectangle rec8 = new Rectangle(803, 76, 82, 10);  // diejia7mm fei hu

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (rec1.Contains(e.Location))
            {
                yangguangcheck = 1;

            }
            else if (rec2.Contains(e.Location))
            {
                yangguangcheck = 2;

            }
            else if (rec3.Contains(e.Location))
            {

                qccd = qccd + 100;
                qccd1 = qccd1 - 100;

            }
            else if (rec4.Contains(e.Location))
            {
                qccd = double.Parse(qccdtext_2.Text);
                qccd1 = 450;
            }
            else if (rec5.Contains(e.Location))
            {
                yzzhenliang = 0;  //增大

            }
            else if (rec6.Contains(e.Location))
            {

                yzzhenliang = 0;  //减小

            }
            else if (rec7.Contains(e.Location))
            {
                diejia7mm = 1;
            }
            else if (rec8.Contains(e.Location))
            {
                diejia7mm = 0;

            }

        }

        private void exitButton_2_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -s -t 60");
            // udpClient.Close();            
            EndingForm eForm = new EndingForm();
            eForm.Show();
            this.Close();  //本窗体退出     
        }

        private void battery_power_2_Click(object sender, EventArgs e)
        {
            power_Form pForm = new power_Form();
            pForm.Show();



        }

        private void keyboard_2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
        }

        /// //////////////////////////////////////////////////////////////标定模块////////////////////////////////////////////////////////////////////////////////////////
        /// 

        //缺陷位置信息
        double[] qxlocation_2 = new double[5];
        double[] qxdian_2 = new double[5];
        double[] biaodingx_2 = new double[5];

        //缺陷深度信息
        double[] qxsd_2 = new double[5];
        double[] qxsd1_2 = new double[5];

        //标定系数
        double[] huiguixishu_2 = new double[3];
        string huiguixishu1_2;
        string huiguixishu2_2;
        string huiguixishu3_2;

        //缺陷点的变量x
        double[] qxx_2 = new double[5];
        double[] qxx1_2 = new double[5];
        //缺陷点的变量y
        double[] qxy_2 = new double[5];

        private double a_2 = 0;
        private double b_2 = 0;

        double[] qxsd2_2 = new double[5];

        private double distext_2;

        private void biaodingbutton_Click(object sender, EventArgs e)
        {
            int NodeNum = 5;
            if (qxwztext1_2.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (qxwztext2_2.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (qxwztext3_2.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (qxwztext4_2.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (qxwztext5_2.Text == "")
            {

                NodeNum = 4;

                qxlocation_2[0] = double.Parse(qxwztext1_2.Text);
                qxlocation_2[1] = double.Parse(qxwztext2_2.Text);
                qxlocation_2[2] = double.Parse(qxwztext3_2.Text);
                qxlocation_2[3] = double.Parse(qxwztext4_2.Text);


                qxsd1_2[0] = double.Parse(sddltext1_2.Text);
                qxsd1_2[1] = double.Parse(sddltext2_2.Text);
                qxsd1_2[2] = double.Parse(sddltext3_2.Text);
                qxsd1_2[3] = double.Parse(sddltext4_2.Text);

                //找到输入的缺陷在点的位置
                distext_2 = double.Parse(DisText_2.Text);
                qxdian_2[0] = (int)Math.Round(qxlocation_2[0] * original_num / distext_2);
                qxdian_2[1] = (int)Math.Round(qxlocation_2[1] * original_num / distext_2);
                qxdian_2[2] = (int)Math.Round(qxlocation_2[2] * original_num / distext_2);
                qxdian_2[3] = (int)Math.Round(qxlocation_2[3] * original_num / distext_2);






                /////////////////////////////////////////////////////////归一化///////////////////////////////////////////////////
                double x0 = 0.025;
                double y0 = 0.025;
                double W = 0.95;
                double H = 0.90;
                float x = (float)(pic_2.Width * x0);
                float y = (float)(pic_2.Height * y0);
                float width = (float)(pic_2.Width * W);
                float height = (float)(pic_2.Height * H);
                int n = 0;
                double C;
                lenofpiece = double.Parse(DisText_2.Text);
                double Sen = double.Parse(yztext_2.Text);//灵敏度
                tiligaodu = double.Parse(TLBox_2.Text);
                double D = double.Parse(DText_2.Text);

                double sum_1 = 0;
                double mean_1 = 0;
                double biaozhuncha_sum_1 = 0;
                double biaozhuncha_1 = 0;
                double[] ysdata_new_1 = new double[10000];
                double[] Dydata_new_1 = new double[10000];

                double sum_3 = 0;
                double mean_3 = 0;
                double biaozhuncha_sum_3 = 0;
                double biaozhuncha_3 = 0;
                double[] ysdata_new_3 = new double[10000];
                double[] Dydata_new_3 = new double[10000];

                double[] ysdata_new = new double[10000];
                double[] Dydata_new = new double[10000];
                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_1 = sum_1 + ysdata[2, j];
                }
                mean_1 = sum_1 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((ysdata[2, j] - mean_1), 2);
                }
                biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_1[j] = (ysdata[2, j] - mean_1) / biaozhuncha_1;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_3 = sum_3 + ysdata[8, j];
                }
                mean_3 = sum_3 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((ysdata[8, j] - mean_3), 2);
                }
                biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_3[j] = (ysdata[8, j] - mean_3) / biaozhuncha_3;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new[j] = (ysdata_new_1[j] + ysdata_new_3[j]) / 2;
                }


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                qxx_2[0] = ysdata_new[(int)qxdian_2[0]];
                qxx_2[1] = ysdata_new[(int)qxdian_2[1]];
                qxx_2[2] = ysdata_new[(int)qxdian_2[2]];
                qxx_2[3] = ysdata_new[(int)qxdian_2[3]];



                double sum = 0;
                double Junzhi = 0;
                double Se = 0;
                double lyy = 0;
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum = sum + qxx_2[i - 1];//求x的倒数

                }
                Junzhi = sum / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    qxx1_2[i - 1] = qxx_2[i - 1] / Junzhi;//求x的倒数
                    qxsd1_2[i - 1] = Math.Log(qxsd1_2[i - 1]);//求ln（y）
                }
                double sum_x1 = 0;
                double sum_y1 = 0;
                double sum_y = 0;
                double Junzhi_x1 = 0;
                double Junzhi_y1 = 0;
                double Junzhi_y = 0;

                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_x1 = sum_x1 + qxx1_2[i - 1];
                    sum_y1 = sum_y1 + qxsd1_2[i - 1];
                }
                Junzhi_x1 = sum_x1 / NodeNum;
                Junzhi_y1 = sum_y1 / NodeNum;
                double xishu1 = 0;
                double xishu2 = 0;
                double A = 0;
                double R = 0;//相关系数


                for (int i = 1; i <= NodeNum; i++)
                {
                    xishu1 = xishu1 + Math.Pow((qxx1_2[i - 1] - Junzhi_x1), 2);
                    xishu2 = xishu2 + (qxx1_2[i - 1] - Junzhi_x1) * (qxsd1_2[i - 1] - Junzhi_y1);
                }
                b_2 = xishu2 / xishu1;
                A = Junzhi_y1 - b_2 * Junzhi_x1;
                a_2 = Math.Exp(A);

                for (int i = 1; i <= NodeNum; i++)
                {
                    qxsd2_2[i - 1] = a_2 * Math.Exp(b_2 * qxx1_2[i - 1]);
                }
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_y = sum_y + qxsd_2[i - 1];
                }
                Junzhi_y = sum_y / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    Se = Se + Math.Pow((qxsd_2[i - 1] - qxsd2_2[i - 1]), 2);
                    lyy = lyy + Math.Pow((qxsd_2[i - 1] - Junzhi_y), 2);
                }
                R = 1 - Se / lyy;
                huiguixishu_2[0] = a_2;
                huiguixishu_2[1] = b_2;
                huiguixishu_2[2] = R;


                string huiguixishu1 = huiguixishu_2[0].ToString();
                string huiguixishu2 = huiguixishu_2[1].ToString();
                string huiguixishu3 = huiguixishu_2[2].ToString();



                bdxstext1_2.Text = huiguixishu1;
                bdxstext2_2.Text = huiguixishu2;
                bdxstext3_2.Text = huiguixishu3;

            }
            ///////////////////////////////////有5个定量缺陷的情况/////////////////////////
            else
            {

                NodeNum = 5;

                qxlocation_2[0] = double.Parse(qxwztext1_2.Text);
                qxlocation_2[1] = double.Parse(qxwztext2_2.Text);
                qxlocation_2[2] = double.Parse(qxwztext3_2.Text);
                qxlocation_2[3] = double.Parse(qxwztext4_2.Text);
                qxlocation_2[4] = double.Parse(qxwztext5_2.Text);

                qxsd1_2[0] = double.Parse(sddltext1_2.Text);
                qxsd1_2[1] = double.Parse(sddltext2_2.Text);
                qxsd1_2[2] = double.Parse(sddltext3_2.Text);
                qxsd1_2[3] = double.Parse(sddltext4_2.Text);
                qxsd1_2[4] = double.Parse(sddltext5_2.Text);

                //找到输入的缺陷在点的位置
                distext_2 = double.Parse(DisText_2.Text);
                qxdian_2[0] = (int)Math.Round(qxlocation_2[0] * original_num / distext_2);
                qxdian_2[1] = (int)Math.Round(qxlocation_2[1] * original_num / distext_2);
                qxdian_2[2] = (int)Math.Round(qxlocation_2[2] * original_num / distext_2);
                qxdian_2[3] = (int)Math.Round(qxlocation_2[3] * original_num / distext_2);
                qxdian_2[4] = (int)Math.Round(qxlocation_2[4] * original_num / distext_2);





                /////////////////////////////////////////////////////////归一化///////////////////////////////////////////////////
                double x0 = 0.025;
                double y0 = 0.025;
                double W = 0.95;
                double H = 0.90;
                float x = (float)(pic_2.Width * x0);
                float y = (float)(pic_2.Height * y0);
                float width = (float)(pic_2.Width * W);
                float height = (float)(pic_2.Height * H);
                int n = 0;
                double C;
                lenofpiece = double.Parse(DisText_2.Text);
                double Sen = double.Parse(yztext_2.Text);//灵敏度
                tiligaodu = double.Parse(TLBox_2.Text);
                double D = double.Parse(DText_2.Text);

                double sum_1 = 0;
                double mean_1 = 0;
                double biaozhuncha_sum_1 = 0;
                double biaozhuncha_1 = 0;
                double[] ysdata_new_1 = new double[1000000];
                double[] Dydata_new_1 = new double[1000000];

                double sum_3 = 0;
                double mean_3 = 0;
                double biaozhuncha_sum_3 = 0;
                double biaozhuncha_3 = 0;
                double[] ysdata_new_3 = new double[1000000];
                double[] Dydata_new_3 = new double[1000000];

                double[] ysdata_new = new double[1000000];
                double[] Dydata_new = new double[1000000];
                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_1 = sum_1 + ysdata[2, j];
                }
                mean_1 = sum_1 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((ysdata[2, j] - mean_1), 2);
                }
                biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_1[j] = (ysdata[2, j] - mean_1) / biaozhuncha_1;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_3 = sum_3 + ysdata[8, j];
                }
                mean_3 = sum_3 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((ysdata[8, j] - mean_3), 2);
                }
                biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_3[j] = (ysdata[8, j] - mean_3) / biaozhuncha_3;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new[j] = (ysdata_new_1[j] + ysdata_new_3[j]) / 2;
                }


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                qxx_2[0] = ysdata_new[(int)qxdian_2[0]];
                qxx_2[1] = ysdata_new[(int)qxdian_2[1]];
                qxx_2[2] = ysdata_new[(int)qxdian_2[2]];
                qxx_2[3] = ysdata_new[(int)qxdian_2[3]];
                qxx_2[4] = ysdata_new[(int)qxdian_2[4]];



                double sum = 0;
                double Junzhi = 0;
                double Se = 0;
                double lyy = 0;
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum = sum + qxx_2[i - 1];//求x的倒数

                }
                Junzhi = sum / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    qxx1_2[i - 1] = qxx_2[i - 1] / Junzhi;//求x的倒数
                    qxsd1_2[i - 1] = Math.Log(qxsd1_2[i - 1]);//求ln（y）
                }
                double sum_x1 = 0;
                double sum_y1 = 0;
                double sum_y = 0;
                double Junzhi_x1 = 0;
                double Junzhi_y1 = 0;
                double Junzhi_y = 0;

                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_x1 = sum_x1 + qxx1_2[i - 1];
                    sum_y1 = sum_y1 + qxsd1_2[i - 1];
                }
                Junzhi_x1 = sum_x1 / NodeNum;
                Junzhi_y1 = sum_y1 / NodeNum;
                double xishu1 = 0;
                double xishu2 = 0;
                double A = 0;
                double R = 0;//相关系数


                for (int i = 1; i <= NodeNum; i++)
                {
                    xishu1 = xishu1 + Math.Pow((qxx1_2[i - 1] - Junzhi_x1), 2);
                    xishu2 = xishu2 + (qxx1_2[i - 1] - Junzhi_x1) * (qxsd1_2[i - 1] - Junzhi_y1);
                }
                b_2 = xishu2 / xishu1;
                A = Junzhi_y1 - b_2 * Junzhi_x1;
                a_2 = Math.Exp(A);

                for (int i = 1; i <= NodeNum; i++)
                {
                    qxsd2_2[i - 1] = a_2 * Math.Exp(b_2 * qxx1_2[i - 1]);
                }
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_y = sum_y + qxsd_2[i - 1];
                }
                Junzhi_y = sum_y / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    Se = Se + Math.Pow((qxsd_2[i - 1] - qxsd2_2[i - 1]), 2);
                    lyy = lyy + Math.Pow((qxsd_2[i - 1] - Junzhi_y), 2);
                }
                R = 1 - Se / lyy;
                huiguixishu_2[0] = a_2;
                huiguixishu_2[1] = b_2;
                huiguixishu_2[2] = R;


                string huiguixishu1 = huiguixishu_2[0].ToString();
                string huiguixishu2 = huiguixishu_2[1].ToString();
                string huiguixishu3 = huiguixishu_2[2].ToString();



                bdxstext1_2.Text = huiguixishu1;
                bdxstext2_2.Text = huiguixishu2;
                bdxstext3_2.Text = huiguixishu3;

            }
        }

        private void PointButton_2_Click(object sender, EventArgs e)
        {
            pointflag = true;
            ZtextBox_2.Enabled = true;
            Tslabel_2.Text = "请选取第一个点" + "\r\n" + "的x轴坐标";
        }

        private void tabControl_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_2.SelectedTab.Name == "tabPage2_2")
            {
                panel1_2.Visible = false;
                tabControl_2.Height = 551;
                zedGraphControl2_2.Location = new Point(92, 4);
                zedGraphControl2_2.Size = new System.Drawing.Size(661, 169);
                zedGraphControl3_2.Location = new Point(92, 174);
                zedGraphControl3_2.Size = new System.Drawing.Size(661, 169);
            }
            if (tabControl_2.SelectedTab.Name == "tabPage1_2")
            {
                panel1_2.Visible = true;
                tabControl_2.Height = 401;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline3();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline3();
        }

        private void zedGraphControl1_2_DoubleClick(object sender, EventArgs e)
        {


            DynamicForm DyForm1 = new DynamicForm(zedGraphControl1_2);
            DyForm1.Show();
        
        }

        private void zedGraphControl2_2_DoubleClick(object sender, EventArgs e)
        {
            DynamicForm DyForm2 = new DynamicForm(zedGraphControl2_2);
            DyForm2.Show();
        }

        private void zedGraphControl3_2_DoubleClick(object sender, EventArgs e)
        {
            DynamicForm DyForm3 = new DynamicForm(zedGraphControl3_2);
            DyForm3.Show();
        }

        private void pic_2_DoubleClick(object sender, EventArgs e)
        {
            picForm pic_fd = new picForm(pic_2.Image);
            pic_fd.Show();
        }

        private void walking_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpClient.Close();
        }

        private void zedGraphControl1_2_Load(object sender, EventArgs e)
        {

        }
    }
}
