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

using System.Runtime.InteropServices;

namespace theHostComputer
{
    public partial class biaoding : Form
    {// Dog dog = new Dog();//看门狗
        C_Code Code = new C_Code();  //解码类
        CFunction Fun = new CFunction();  //计算腐蚀深度类        
        private Thread thread1;
        private static IPEndPoint ipep;                //new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);                 
        private UdpClient udpClient = new UdpClient(50000);
        private string FilePath;  //数据文件路径         

        private long[,] FADData = new long[2, 6];    //解码后的各个探头磁场数据
        private double[,] FRealData = new double[12, 1000000];    //计算得出的最终可用的磁场数据

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

        private int delete_points_flag = 0;
        private int defect_recognition_2_flag = 2;
        //private int[,] curve_display_location = new int[4, 10000];
        private int[,] chafen_delete_location = new int[4, 10000];
        private double[,] curve_display_X = new double[4, 10000];                //curve_display_X由通过新3西格玛算法提取出的缺陷信号构成
        private double[,] curve_display_Y = new double[4, 10000];
        private double[,] curve_display_Z = new double[4, 10000];                //curve_display_Z由通过新3西格玛算法提取出的缺陷信号构成
        private double[,] defect_X = new double[4, 10000];                       //defect_X数组是将用新3西格玛算法找到的缺陷进行组合，判断两个缺陷间距小于等于200mm的组合为一个缺陷
        private double[,] defect_Y = new double[4, 10000];
        private double[,] defect_Z = new double[4, 10000];                       //defect_Z数组是将用新3西格玛算法找到的缺陷进行组合，判断两个缺陷间距小于等于200mm的组合为一个缺陷
        private double[,] pic_2_imagination = new double[4, 10000];
        private double[,] pic_2_imagination_another = new double[4, 10000];
        //private double[,] reconstruction_data_display = new double[4, 10000];
        //private double[,] reconstruction_data = new double[4, 10000];
        private double[] reconstruction_data_display = new double[10000];
        private double[,] reconstruction_data = new double[4, 10000];
        //private double[] final_display = new double[10000];
        private double[,] final_imagination = new double[8, 10000];
        private double[,] original_signal = new double[12, 10000];
        private double[,] original_signal_chafen = new double[12, 10000];
        private double[,] original_signal_zuocha = new double[1, 10000];
        private double[,] original_signal_chafenzuocha = new double[1, 10000];
        //private double[,] original_signal_1 = new double[4, 1000000];

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
        //private double [,] cidao=new double [12,1000000];
        private double[,] cidao_1 = new double[8, 1000000];
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
        private int wireless_num = 0;

        private bool linkflag = false;
        private bool startflag = false;
        private bool wireless_stopflag = false;
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
        private double[,] Dydata = new double[12, 100000];      //原始张量数据
        private double[] dy_originalUP = new double[12];           //原始梯度上限
        private double[] dy_originalDW = new double[12];         //原始梯度下限                                             

        // 等值线对应的颜色
        List<System.Drawing.Color> list_Color = new List<System.Drawing.Color>();
        //结果等值线图相关变量
        private double picDX = 0, picDY = 0;
        //缺陷定位相关变量
        private Point currentPoint = new Point(0, 0);
        private bool pointflag = false;
        private bool readflag = false;


        ////包覆层管道
        private double tiligaodu = 0;
        private double tiligaodu1 = 0;
        //private double yangguangleixin = 2;   //样管类型
        private double yangguangcheck = 1;  //yangguanqueding(da or xiao) 默认改为1（即大缺陷的形式）
        private double qccd = 100; //去除长度定义为整体变量(默认为100)
        private double qccd1 = 450; //针对7mm厚管壁
        private double qccd2 = 200; //针对5mm厚管壁
        private int chufa = 0;  //判断是否触发了某控件，默认为0，即未触发。
        private double fangxian = 0;   //5mm管道fangxian,默认为某个方向
        private double diejia7mm = 0;  //默认该参数为0

        double yzzhenliang = 0; //阈值增量
        ////
        /*************************************************************** Battery_Power_defination ********************************************************************/
        UInt32 status1 = 1;
        UInt32 status2 = 1;
        UInt32 status3 = 1;
        UInt32 Value = 0;

        byte battery = 0;
        byte addr = 0;
        byte cmd = 0;
        UInt32 length = 0;

        StringBuilder sb;
        byte[] DataBlock;

        UInt32 status1_2 = 1;
        UInt32 status2_2 = 1;
        UInt32 status3_2 = 1;
        UInt32 Value_2 = 0;

        byte battery_2 = 0;
        byte addr_2 = 0;
        byte cmd_2 = 0;
        UInt32 length_2 = 0;

        StringBuilder sb_2;
        byte[] DataBlock_2;

        UInt32 status1_voltage = 1;
        UInt32 status2_voltage = 1;
        UInt32 status3_voltage = 1;
        UInt32 Value_voltage = 0;

        UInt32 voltage = 0;
        byte addr_voltage = 0;
        byte cmd_voltage = 0;
        UInt32 length_voltage = 0;

        StringBuilder sb_voltage;
        byte[] DataBlock_voltage;

        public const UInt32 SUSI_STATUS_NOT_INITIALIZED = 0xFFFFFFFF;
        public const UInt32 SUSI_STATUS_INITIALIZED = 0xFFFFFFFE;
        public const UInt32 SUSI_STATUS_ALLOC_ERROR = 0xFFFFFFFD;
        public const UInt32 SUSI_STATUS_DRIVER_TIMEOUT = 0xFFFFFFFC;
        public const UInt32 SUSI_STATUS_INVALID_PARAMETER = 0xFFFFFEFF;
        public const UInt32 SUSI_STATUS_INVALID_BLOCK_ALIGNMENT = 0xFFFFFEFE;
        public const UInt32 SUSI_STATUS_INVALID_BLOCK_LENGTH = 0xFFFFFEFD;
        public const UInt32 SUSI_STATUS_INVALID_DIRECTION = 0xFFFFFEFC;
        public const UInt32 SUSI_STATUS_INVALID_BITMASK = 0xFFFFFEFB;
        public const UInt32 SUSI_STATUS_RUNNING = 0xFFFFFEFA;
        public const UInt32 SUSI_STATUS_UNSUPPORTED = 0xFFFFFCFF;
        public const UInt32 SUSI_STATUS_NOT_FOUND = 0xFFFFFBFF;
        public const UInt32 SUSI_STATUS_TIMEOUT = 0xFFFFFBFE;
        public const UInt32 SUSI_STATUS_BUSY_COLLISION = 0xFFFFFBFD;
        public const UInt32 SUSI_STATUS_READ_ERROR = 0xFFFFFAFF;
        public const UInt32 SUSI_STATUS_WRITE_ERROR = 0xFFFFFAFE;
        public const UInt32 SUSI_STATUS_MORE_DATA = 0xFFFFF9FF;
        public const UInt32 SUSI_STATUS_ERROR = 0xFFFFF0FF;
        public const UInt32 SUSI_STATUS_SUCCESS = 0x00000000;

        public const UInt32 SUSI_ID_SMBUS_SUPPORTED = 0x00030000;
        public const UInt32 SUSI_SMBUS_EXTERNAL_SUPPORTED = (1 << 0);
        public const UInt32 SUSI_SMBUS_OEM0_SUPPORTED = (1 << 1);
        public const UInt32 SUSI_SMBUS_OEM1_SUPPORTED = (1 << 2);
        public const UInt32 SUSI_SMBUS_OEM2_SUPPORTED = (1 << 3);
        public const UInt32 SUSI_SMBUS_OEM3_SUPPORTED = (1 << 4);

        public const UInt32 SUSI_SMBUS_MAX_DEVICE = 5;

        public const UInt32 SUSI_ID_SMBUS_EXTERNAL = 0x00000000;
        public const UInt32 SUSI_ID_SMBUS_OEM0 = 0x00000001;
        public const UInt32 SUSI_ID_SMBUS_OEM1 = 0x00000002;
        public const UInt32 SUSI_ID_SMBUS_OEM2 = 0x00000003;
        public const UInt32 SUSI_ID_SMBUS_OEM3 = 0x00000004;

        public const UInt32 SUSI_ID_BASE_GET_NAME_SMB = 0x30000000;

        public static UInt32 SUSI_ID_MAPPING_GET_NAME_SMB(UInt32 Id)
        {
            return (Id | SUSI_ID_BASE_GET_NAME_SMB);
        }

        [DllImport("Susi4.dll", EntryPoint = "SusiLibInitialize")]
        public static extern UInt32 SusiLibInitialize();
        [DllImport("Susi4.dll", EntryPoint = "SusiLibUninitialize")]
        public static extern UInt32 SusiLibUninitialize();
        [DllImport("Susi4.dll", EntryPoint = "SusiBoardGetValue")]
        public static extern UInt32 SusiBoardGetValue(UInt32 Id, ref UInt32 pValue);
        [DllImport("Susi4", EntryPoint = "SusiBoardGetStringA")]
        public static extern UInt32 SusiBoardGetStringA(UInt32 Id, StringBuilder pBuffer, ref UInt32 pBufLen);
        [DllImport("Susi4", EntryPoint = "SusiSMBI2CReadBlock")]
        public static extern UInt32 SusiSMBI2CReadBlock(UInt32 Id, byte Addr, byte Cmd, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] pBuffer, UInt32 Length);
/**************************************************************************************************************************************************/
        public biaoding()
        {
            InitializeComponent();
        }



        private void linkButton1_Click(object sender, EventArgs e)
        {
            //有线//
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
            //无线//
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.8"), 20108);
            if (linkButton1.Text == "连接")
            {
                linkButton1.Text = "断开";
                startButton1.Enabled = true;
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
                linkButton1.Text = "连接";
                startButton1.Enabled = false;
                linkflag = false;
                if (startflag)
                {
                    thread1.Abort();
                }

            }
        }

        private void startButton1_Click(object sender, EventArgs e)
        {
            //  dog.isMyDog();//找狗

            //有线//
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
            //无线//
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.8"), 20108);
            if (startButton1.Text == "开始")
            {
                if (DisText.Text == "")
                {
                    MessageBox.Show("请输入扫描距离！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (DText.Text == "")
                {
                    MessageBox.Show("请输入管道外径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (TText.Text == "")
                {
                    MessageBox.Show("请输入管道壁厚！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (TLBox.Text == "")
                {
                    MessageBox.Show("请输入管道埋深！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                else
                {
                    startButton1.Text = "停止";
                    linkButton1.Enabled = false;
                    SaveButton.Enabled = false;
                    runButton.Enabled = false;
                    browseButton.Enabled = false;

                    RatecomboBox.Enabled = false;
                    startflag = true;
                    wireless_stopflag = false;
                    runflag = false;

                    byte[] bb = new byte[4];  //定义一个数组用来做数据的缓冲区      
                    //bb[0] = 0x1;
                    //bb[1] = 0x0;
                    //bb[2] = (byte)(RatecomboBox.SelectedIndex);
                    //bb[3] = 0x0;
                    bb[0] = 0x1;
                    bb[1] = 0x1;
                    bb[2] = (byte)(RatecomboBox.SelectedIndex);//选择采样频率    最小为6.25，有线方式可以随意更改，无线方式由于空中传播速率与传输数据量的原因，经测试最大只能改为12
                    bb[3] = 0x2;
                    udpClient.Send(bb, bb.Length, ipep);

                    widthOfPiece = Math.PI * double.Parse(DText.Text);     //管道的周长

                    startButton1.BackColor = Color.GreenYellow;
                    //biao_D = double.Parse(DcomboBox.Text);                                
                    ThickOftube = double.Parse(TText.Text);    //管道厚度
                    //tongdao_num = int.Parse(ChnumcomboBox.Text);
                    tongdao_num = 12;
                    //DXMAX = double.Parse(WcomboBox.Text);
                    //picXMAX = DXMAX;
                    //DXMAXStep = DXMAX / 10;
                    ZedGraphInit();       //曲线图初始化
                    //creatdatafile();  //建立文件路径                                                                  
                    listClear();
                    //creatdatafile1();  //建立文件路径                                                                  

                    //DynamicpicInit();     //动态腐蚀图初始化、清图
                    selectokButton_Click(sender, e);  //动态腐蚀曲线加图例、清曲线  
                    Row = 0;
                    RRow = 0;
                    //SRow = 0;
                    //startflag = true;


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
                startButton1.Text = "开始";
                startButton1.BackColor = Color.Pink;
                linkButton1.Enabled = true;
                SaveButton.Enabled = true;
                runButton.Enabled = true;
                browseButton.Enabled = true;

                RatecomboBox.Enabled = true;
                thread1.Abort();
                byte[] bb1 = new byte[3] { 0x0, 0x0, 0x0 };
                udpClient.Send(bb1, bb1.Length, ipep);

                startflag = false;
                wireless_stopflag = true;
                browseflag = false;
                readflag = false;

            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (wireless_stopflag == true)
            {
                //有线//
                //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
                //无线//
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.8"), 20108);
                byte[] CC = new byte[3] { 0x0, 0x0, 0x0 };
                udpClient.Send(CC, CC.Length, ipep);
                wireless_num = wireless_num + 1;
                if (wireless_num == 3)
                {
                    wireless_num = 0;
                    wireless_stopflag = false;
                }
            }
        }

        List<byte[]> lst = new List<byte[]>();
        byte[] receiveBytes = new byte[40];
        long data;

        //接受磁场函数
        private void Revonce()
        {
            try
            {
                data = udpClient.Client.Available;
                if (data / 40 > 0)
                {
                    lst.Clear();
                    for (int i = 0; i < data / 40; i++)
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (NoText.Text == "")
            {
                MessageBox.Show("请输入工件编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (DisText.Text == "")
            {
                MessageBox.Show("请输入扫描距离！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (TLBox.Text == "")
            {
                MessageBox.Show("请输入管道埋深！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //////

            timestr = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            pathtxt = Application.StartupPath + "\\" + "datafile0" + "\\" + NoText.Text + "# at " + timestr + "数据" + ".txt";
            fs = File.Open(pathtxt, FileMode.Create, FileAccess.Write);
            swt = new StreamWriter(fs, Encoding.Default);
            string txt;
            txt = "工件号：" + " " + NoText.Text;
            swt.WriteLine(txt);
            timestr = DateTime.Now.ToString();
            txt = "检测时间：" + " " + timestr;
            swt.WriteLine(txt);
            txt = "扫查距离：" + " " + DisText.Text + " " + "管道外径：" + " " + DText.Text + " " + "管道壁厚：" + " " + TText.Text + " " + "管道埋深：" + " " + TLBox.Text ;
            swt.WriteLine(txt);
            txt = "缺陷1位置：" + " " + weizhiBox1.Text + " " + "缺陷1损失当量：" + " " + dangliangBox1.Text;
            swt.WriteLine(txt);
            txt = "缺陷2位置：" + " " + weizhiBox2.Text + " " + "缺陷2损失当量：" + " " + dangliangBox2.Text;
            swt.WriteLine(txt);
            txt = "缺陷3位置：" + " " + weizhiBox3.Text + " " + "缺陷3损失当量：" + " " + dangliangBox3.Text;
            swt.WriteLine(txt);
            txt = "缺陷4位置：" + " " + weizhiBox4.Text + " " + "缺陷4损失当量：" + " " + dangliangBox4.Text;
            swt.WriteLine(txt);
            txt = "缺陷5位置：" + " " + weizhiBox5.Text + " " + "缺陷5损失当量：" + " " + dangliangBox5.Text;
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
            list1.Add(Row, FRealData[0, Row]);
            list2.Add(Row, FRealData[1, Row]);
            list3.Add(Row, FRealData[2, Row]);
            list4.Add(Row, FRealData[3, Row]);
            list5.Add(Row, FRealData[4, Row]);
            list6.Add(Row, FRealData[5, Row]);
            list7.Add(Row, FRealData[6, Row]);
            list8.Add(Row, FRealData[7, Row]);
            list9.Add(Row, FRealData[8, Row]);
            list10.Add(Row, FRealData[9, Row]);
            list11.Add(Row, FRealData[10, Row]);
            list12.Add(Row, FRealData[11, Row]);

        }

        //队列移除数组
        private void listDeletedata()
        {
            list1.RemoveAt(0);
            list2.RemoveAt(0);
            list3.RemoveAt(0);
            list4.RemoveAt(0);
            list5.RemoveAt(0);
            list6.RemoveAt(0);
            list7.RemoveAt(0);
            list8.RemoveAt(0);
            list9.RemoveAt(0);
            list10.RemoveAt(0);
            list11.RemoveAt(0);
            list12.RemoveAt(0);
        }

        //队列清空
        private void listClear()
        {
            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();
            list5.Clear();
            list6.Clear();
            list7.Clear();
            list8.Clear();
            list9.Clear();
            list10.Clear();
            list11.Clear();
            list12.Clear();

        }

        //按时间显示并保存磁场数据
        private void Display()
        {
            FCalData.Cal_0 = new double[2, 6] { { -1066, -5474, -5030, -7311, -7304, -6493 }, { -6125, -7842, -4921, -6142, -6963, -4841 } };
            FCalData.Cal_n30000 = new double[2, 6] { { 1576187, 1567736, 1542847, 1558022, 1564538, 1557490 }, { 1575888, 1568063, 1554953, 1567310, 1562473, 1548592 } };
            FCalData.Cal_p30000 = new double[2, 6] { { -1578302, -1571003, -1545248, -1564996, -1571277, -1562874 }, { -1580230, -1576194, -1557147, -1571878, -1568494, -1550920 } };
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
                    FRealData[0, Row] = zhangliang[0, Row];
                    FRealData[1, Row] = zhangliang[1, Row];
                    FRealData[2, Row] = zhangliang[2, Row];
                    FRealData[3, Row] = zhangliang[3, Row];
                    FRealData[4, Row] = zhangliang[4, Row];
                    FRealData[5, Row] = zhangliang[5, Row];
                    FRealData[6, Row] = zhangliang[6, Row];
                    FRealData[7, Row] = zhangliang[7, Row];
                    FRealData[8, Row] = zhangliang[8, Row];

                    listAdddataRow();

                    Dydata[0, Row] = FRealData[0, Row];
                    Dydata[1, Row] = FRealData[1, Row];
                    Dydata[2, Row] = FRealData[2, Row];
                    Dydata[3, Row] = FRealData[3, Row];
                    Dydata[4, Row] = FRealData[4, Row];
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
        private void MainForm_Load(object sender, EventArgs e)
        {
            //埋地管道
          //  qccd = double.Parse(qccdtext.Text);  //
            //
            SaveButton.Enabled = false;
            startButton1.Enabled = false;
            runButton.Enabled = false;
            Control.CheckForIllegalCrossThreadCalls = false;
            double[] BLUE = new double[64] { 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5, 0.4375, 0.375, 0.3125, 0.25, 0.1875, 0.125, 0.0625, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] GREEN = new double[64] { 0, 0, 0, 0, 0, 0, 0, 0, 0.0625, 0.125, 0.1875, 0.25, 0.3125, 0.375, 0.4375, 0.5, 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5, 0.4375, 0.375, 0.3125, 0.25, 0.1875, 0.125, 0.0625, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] RED = new double[64] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0625, 0.125, 0.1875, 0.25, 0.3125, 0.375, 0.4375, 0.5, 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5 };
            System.Drawing.Color temp_C;
            for (int i = 0; i < 64; i++)
            {
                temp_C = System.Drawing.Color.FromArgb((int)(RED[i] * 255), (int)(GREEN[i] * 255), (int)(BLUE[i] * 255));
                list_Color.Add(temp_C);
            }
            RatecomboBox.SelectedIndex = 1;
            ZedGraphInit();

        }

        //退出按钮
        private void exitButton1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -s -t 60");
            udpClient.Close();
            WelcomeForm wForm = new WelcomeForm();
            wForm.Show();
            this.Hide();  //本窗体退出            
        }

        //zedgraph初始化
        private void ZedGraphInit()
        {
            //获取引用
            GraphPane myPane = zedGraphControl1.GraphPane;

            //清空原图像
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();
            zedGraphControl1.Refresh();

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




            //改变轴的刻度
            myPane.AxisChange();
            zedGraphControl1.Invalidate();

        }

        //全选

        //全选按钮
        private void selectallCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CH1checkBox.Checked = selectallCheckBox.Checked;
            CH2checkBox.Checked = selectallCheckBox.Checked;
            CH3checkBox.Checked = selectallCheckBox.Checked;
            CH4checkBox.Checked = selectallCheckBox.Checked;
            CH5checkBox.Checked = selectallCheckBox.Checked;
            CH6checkBox.Checked = selectallCheckBox.Checked;
            CH7checkBox.Checked = selectallCheckBox.Checked;
            CH8checkBox.Checked = selectallCheckBox.Checked;
            CH9checkBox.Checked = selectallCheckBox.Checked;
            CH10checkBox.Checked = selectallCheckBox.Checked;
            CH11checkBox.Checked = selectallCheckBox.Checked;
            CH12checkBox.Checked = selectallCheckBox.Checked;

        }
        private void selectokButton_Click(object sender, EventArgs e)
        {
            chselectokflag = false;
            chflag[0] = CH1checkBox.Checked;
            chflag[1] = CH2checkBox.Checked;
            chflag[2] = CH3checkBox.Checked;
            chflag[3] = CH4checkBox.Checked;
            chflag[4] = CH5checkBox.Checked;
            chflag[5] = CH6checkBox.Checked;
            chflag[6] = CH7checkBox.Checked;
            chflag[7] = CH8checkBox.Checked;
            chflag[8] = CH9checkBox.Checked;
            chflag[9] = CH10checkBox.Checked;
            chflag[10] = CH11checkBox.Checked;
            chflag[11] = CH12checkBox.Checked;

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
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            zedGraphControl1.Refresh();

            if (chflag[0] == true)
            {
                PointPairList lst1 = new PointPairList();
                LineItem mycurve1 = zedGraphControl1.GraphPane.AddCurve("CH1", lst1, Color.Blue, SymbolType.None);
            }

            if (chflag[1] == true)
            {
                PointPairList lst2 = new PointPairList();
                LineItem mycurve2 = zedGraphControl1.GraphPane.AddCurve("CH2", lst2, Color.Red, SymbolType.None);
            }

            if (chflag[2] == true)
            {
                PointPairList lst3 = new PointPairList();
                LineItem mycurve3 = zedGraphControl1.GraphPane.AddCurve("CH3", lst3, Color.Brown, SymbolType.None);
            }

            if (chflag[3] == true)
            {
                PointPairList lst4 = new PointPairList();
                LineItem mycurve4 = zedGraphControl1.GraphPane.AddCurve("CH4", lst4, Color.Green, SymbolType.None);
            }

            if (chflag[4] == true)
            {
                PointPairList lst5 = new PointPairList();
                LineItem mycurve5 = zedGraphControl1.GraphPane.AddCurve("CH5", lst5, Color.Black, SymbolType.None);
            }

            if (chflag[5] == true)
            {
                PointPairList lst6 = new PointPairList();
                LineItem mycurve6 = zedGraphControl1.GraphPane.AddCurve("CH6", lst6, Color.Pink, SymbolType.None);
            }

            if (chflag[6] == true)
            {
                PointPairList lst7 = new PointPairList();
                LineItem mycurve7 = zedGraphControl1.GraphPane.AddCurve("CH7", lst7, Color.Yellow, SymbolType.None);
            }

            if (chflag[7] == true)
            {
                PointPairList lst8 = new PointPairList();
                LineItem mycurve8 = zedGraphControl1.GraphPane.AddCurve("CH8", lst8, Color.Gray, SymbolType.None);
            }

            if (chflag[8] == true)
            {
                PointPairList lst9 = new PointPairList();
                LineItem mycurve9 = zedGraphControl1.GraphPane.AddCurve("CH9", lst9, Color.DarkGreen, SymbolType.None);
            }

            if (chflag[9] == true)
            {
                PointPairList lst10 = new PointPairList();
                LineItem mycurve10 = zedGraphControl1.GraphPane.AddCurve("CH10", lst10, Color.DarkRed, SymbolType.None);
            }

            if (chflag[10] == true)
            {
                PointPairList lst11 = new PointPairList();
                LineItem mycurve11 = zedGraphControl1.GraphPane.AddCurve("CH11", lst11, Color.DarkBlue, SymbolType.None);
            }

            if (chflag[11] == true)
            {
                PointPairList lst12 = new PointPairList();
                LineItem mycurve12 = zedGraphControl1.GraphPane.AddCurve("CH12", lst12, Color.Purple, SymbolType.None);
            }

            zedGraphControl1.GraphPane.XAxis.Scale.Min = 0;		//X轴最小值0
            zedGraphControl1.GraphPane.XAxis.Scale.Max = DXMAX;	//X轴最大100
            zedGraphControl1.GraphPane.XAxis.Scale.MinorStep = DXMAXStep / 5;//X轴小步长1,也就是小间隔
            zedGraphControl1.GraphPane.XAxis.Scale.MajorStep = DXMAXStep;//X轴大步长为5，也就是显示文字的大间隔 
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
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
                    LineItem curve0 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list0 = curve0.Points as IPointListEdit;
                    list0.Add(displayrow, FRealData[0, Row]);
                    if (list0.Count > DXMAX)
                    {
                        list0.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[1] == true)
                {
                    LineItem curve1 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list1 = curve1.Points as IPointListEdit;
                    list1.Add(displayrow, FRealData[1, Row]);
                    if (list1.Count > DXMAX)
                    {
                        list1.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[2] == true)
                {
                    LineItem curve2 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list2 = curve2.Points as IPointListEdit;
                    list2.Add(displayrow, FRealData[2, Row]);
                    if (list2.Count > DXMAX)
                    {
                        list2.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[3] == true)
                {
                    LineItem curve3 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list3 = curve3.Points as IPointListEdit;
                    list3.Add(displayrow, FRealData[3, Row]);
                    if (list3.Count > DXMAX)
                    {
                        list3.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[4] == true)
                {
                    LineItem curve4 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list4 = curve4.Points as IPointListEdit;
                    list4.Add(displayrow, FRealData[4, Row]);
                    if (list4.Count > DXMAX)
                    {
                        list4.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[5] == true)
                {
                    LineItem curve5 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list5 = curve5.Points as IPointListEdit;
                    list5.Add(displayrow, FRealData[5, Row]);
                    if (list5.Count > DXMAX)
                    {
                        list5.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[6] == true)
                {
                    LineItem curve6 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list6 = curve6.Points as IPointListEdit;
                    list6.Add(displayrow, FRealData[6, Row]);
                    if (list6.Count > DXMAX)
                    {
                        list6.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[7] == true)
                {
                    LineItem curve7 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list7 = curve7.Points as IPointListEdit;
                    list7.Add(displayrow, FRealData[7, Row]);
                    if (list7.Count > DXMAX)
                    {
                        list7.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[8] == true)
                {
                    LineItem curve8 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list8 = curve8.Points as IPointListEdit;
                    list8.Add(displayrow, FRealData[8, Row]);
                    if (list8.Count > DXMAX)
                    {
                        list8.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[9] == true)
                {
                    LineItem curve9 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list9 = curve9.Points as IPointListEdit;
                    list9.Add(displayrow, FRealData[9, Row]);
                    if (list9.Count > DXMAX)
                    {
                        list9.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[10] == true)
                {
                    LineItem curve10 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list10 = curve10.Points as IPointListEdit;
                    list10.Add(displayrow, FRealData[10, Row]);
                    if (list10.Count > DXMAX)
                    {
                        list10.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[11] == true)
                {
                    LineItem curve11 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list11 = curve11.Points as IPointListEdit;
                    list11.Add(displayrow, FRealData[11, Row]);
                    if (list11.Count > DXMAX)
                    {
                        list11.RemoveAt(0);
                    }
                    i++;
                }

                Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
                if (displayrow > xScale.Max)
                {
                    xScale.Max = displayrow;
                    xScale.Min = xScale.Max - DXMAX;
                }
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
        }
        //数据拟合//
        private void runButton_Click(object sender, EventArgs e)
        {
           







        }
        //读取数据//
        private void browseButton_Click(object sender, EventArgs e)
        {
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
                        FileText.Text = FilePath;
                        readdata();
                        runButton.Enabled = true;
                        
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
            NoText.Text =tempData_str[1];

            readflag = true;

            sr_ScatterData.ReadLine();   //第三行 检测时间
            temp_str = sr_ScatterData.ReadLine();  //第四行 规格和通道数
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            DisText.Text = tempData_str[1];
            lenofpiece1 = double.Parse(tempData_str[1]);
            lenofpiece = lenofpiece1 - kuangjiachangdu;

            DText.Text =tempData_str[3];
            TText.Text =tempData_str[5];
            TLBox.Text = tempData_str[7];
                      
            widthOfPiece =Math.PI*double.Parse(tempData_str[3]);
            ThickOftube = double.Parse(tempData_str[5]);            
            tongdao_num = 12;
            tiligaodu = double.Parse(tempData_str[7]);
            
            /////////////////////////////////
            //读取缺陷位置信息与当量信息
            temp_str = sr_ScatterData.ReadLine(); 
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            weizhiBox1.Text =tempData_str[1];
            dangliangBox1.Text=tempData_str[3];

            temp_str = sr_ScatterData.ReadLine(); 
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            weizhiBox2.Text =tempData_str[1];
            dangliangBox2.Text=tempData_str[3];

            temp_str = sr_ScatterData.ReadLine(); 
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            weizhiBox3.Text =tempData_str[1];
            dangliangBox3.Text=tempData_str[3];

            temp_str = sr_ScatterData.ReadLine(); 
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            weizhiBox4.Text =tempData_str[1];
            dangliangBox4.Text=tempData_str[3];

            temp_str = sr_ScatterData.ReadLine(); 
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            weizhiBox5.Text =tempData_str[1];
            dangliangBox5.Text=tempData_str[3];






            long k=0;
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

                      mean = (ysdata[2, k] + ysdata[5, k] + ysdata[8, k] + ysdata[11, k]) / 4;

                    ysdatanew[0, k]=ysdata[2, k]-mean;
                    ysdatanew[1, k]=ysdata[5, k]-mean;
                    ysdatanew[2, k]=ysdata[8, k]-mean;
                    ysdatanew[3, k]=ysdata[11, k]-mean;

                    k++;
                }

                //if (tempData_str.Count() == 2)
                //{
                //    DisText.Text = tempData_str[1];
                    

                //} 
            }
            original_num =k;
            sr_ScatterData.Close();
        }
        }





















    }

